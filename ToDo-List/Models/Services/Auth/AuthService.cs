
using Microsoft.Extensions.Options;
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository;
using ToDo_List.Models.DataBase.Repositories.UserRepositories;
using ToDo_List.Models.DTO;
using ToDo_List.Models.Services.Auth.Options;

namespace ToDo_List.Models.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly AuthOptionsModel _authOptions;
        private readonly ITokenService _tokenService;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IRefreshSessionWriteRepository _refreshSessionWriteRepository;
        private readonly IRefreshSessionReadRepository _refreshSessionReadRepository;
        public AuthService(
            ILogger<AuthService> logger,
            IOptions<AuthOptionsModel> authOptions,
            ITokenService tokenService,
            IUserReadRepository userReadRepository,
            IUserWriteRepository userWriteRepository,
        IRefreshSessionWriteRepository refreshSessionWriteRepository,
            IRefreshSessionReadRepository refreshSessionReadRepository)
        {
            _logger = logger;
            _authOptions = authOptions.Value;
            _tokenService = tokenService;
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _refreshSessionWriteRepository = refreshSessionWriteRepository;
            _refreshSessionReadRepository = refreshSessionReadRepository;
        }

        public async Task<TokenAggregateDto> Register(RegisterRequestModel model)
        {
            var alreadyExistingUser = await _userReadRepository.GetUserWithSessions(model.Email);

            if (alreadyExistingUser != null)
            {
                return null;
            }

            var user = await CreateUser(model);
            if(user == null)
            {
                return null;
            }

            var newSession = HandleRefreshSessions(user, model);
            user.RefreshSessions = new List<RefreshSession>
            {
                newSession
            };

            var tokensModel = new TokenAggregateDto();

            try
            {
                InitTokens(user, tokensModel, newSession);
                await SaveRefreshSession(newSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }

            return tokensModel;
        }

        public async Task<TokenAggregateDto> LogIn(AuthRequestModel model)
        {
            var user = await _userReadRepository.GetUserWithSessions(model.Email);
            if (user == null)
            {
                return null;
            }

            var isVerified = BCrypt.Net.BCrypt.Verify(model.Password + _authOptions.Pepper, user.Password);
            if (isVerified == false)
            {
                return null;
            }

            var newSession = HandleRefreshSessions(user, model);

            var tokensModel = new TokenAggregateDto();

            try
            {
                InitTokens(user, tokensModel, newSession);
                await SaveRefreshSession(newSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }

            return tokensModel;
        }

        public async Task<bool> LogOut(string refreshToken)
        {
            var currentSession = await _refreshSessionReadRepository.GetSessionByRefreshToken(Guid.Parse(refreshToken));

            if(currentSession == null)
            {
                _logger.LogInformation($"Cannot find the refresh session by given refresh-token: {refreshToken}");
                return false;
            }

            _refreshSessionWriteRepository.Delete(currentSession);
            return await _refreshSessionWriteRepository.SaveChangesAsync();
        }

        public async Task<TokenAggregateDto> RefreshTokens(string fingerPrint, string userAgent, string refreshToken)
        {
            var currentSession = await _refreshSessionReadRepository.GetSessionByRefreshToken(Guid.Parse(refreshToken));

            if (currentSession == null) 
            {
                _logger.LogWarning($"Cannot find refresh session by given refreshToken: {refreshToken}");
                return null;
            }

            if(currentSession.ExpiresIn < DateTime.Now)
            {
                _logger.LogInformation($"Refresh session with Id: {currentSession.Id} has now just expired");

                _refreshSessionWriteRepository.Delete(currentSession);
                return null;
            }

            if(currentSession.FingerPrint != fingerPrint && currentSession.UserAgent != userAgent)
            {
                _logger.LogWarning($"Presumably stolen refreshToken: {refreshToken} was received. Removing all user's refresh sessions.");

                var allSessions = await _refreshSessionReadRepository.GetAll();
                _refreshSessionWriteRepository.DeleteRange(allSessions);
                await _refreshSessionWriteRepository.SaveChangesAsync();

                return null;
            }

            currentSession.RefreshToken = Guid.NewGuid();
            currentSession.ExpiresIn = DateTime.Now + _authOptions.RefreshTokenDuration;
            currentSession.CreatedAt = DateTime.Now;

            _refreshSessionWriteRepository.Update(currentSession);
            await _refreshSessionWriteRepository.SaveChangesAsync();

            var tokens = new TokenAggregateDto()
            {
                AccessToken = _tokenService.GenerateAccessToken(currentSession.UserId),
                AccessTokenDuration = _authOptions.AccessTokenDuration,
                RefreshToken = currentSession.RefreshToken,
                RefreshTokenDuration = _authOptions.RefreshTokenDuration
            };

            if (tokens.AccessToken == null)
            {
                return null;
            }

            return tokens;
        }

        private async Task<User> CreateUser(RegisterRequestModel model)
        {
            var newUser =  new User()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password + _authOptions.Pepper)
            };

            _userWriteRepository.Add(newUser);
            var isUserAdded = await _userWriteRepository.SaveChangesAsync();

            if (!isUserAdded)
            {
                return null;
            }

            return newUser;
        }

        private void InitTokens(User user, TokenAggregateDto tokensModel, RefreshSession newSession)
        {
            tokensModel.AccessToken = _tokenService.GenerateAccessToken(user.Id);
            tokensModel.AccessTokenDuration = _authOptions.AccessTokenDuration;
            tokensModel.RefreshToken = newSession.RefreshToken;
            tokensModel.RefreshTokenDuration = _authOptions.RefreshTokenDuration;
        }

        private RefreshSession HandleRefreshSessions(User user, AuthRequestModel model)
        {
            var userHasRefreshSessions = user.RefreshSessions?.Any();
            if (userHasRefreshSessions == true)
            {
                var currentSession = user.RefreshSessions.Where(x => x.FingerPrint == model.FingerPrint && x.UserAgent == model.UserAgent).FirstOrDefault();
                if (currentSession != null)
                {
                    _refreshSessionWriteRepository.Delete(currentSession);
                    user.RefreshSessions.Remove(currentSession);
                }

                if (user.RefreshSessions.Count() >= _authOptions.MaxSessionsAmount)
                {
                    _refreshSessionWriteRepository.DeleteRange(user.RefreshSessions.ToList());
                }
            }

            var newSession = new RefreshSession()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RefreshToken = Guid.NewGuid(),
                FingerPrint = model.FingerPrint,
                UserAgent = model.UserAgent,
                ExpiresIn = DateTime.Now + _authOptions.RefreshTokenDuration,
                CreatedAt = DateTime.Now
            };

            return newSession;
        }

        private async Task SaveRefreshSession(RefreshSession newSession)
        {
            _refreshSessionWriteRepository.Add(newSession);
            await _refreshSessionWriteRepository.SaveChangesAsync();
        }
    }
}

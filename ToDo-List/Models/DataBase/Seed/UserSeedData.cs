
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Seed
{
    public static class UserSeedData
    {
        public static User GetData()
        {
            var user = new User()
            {
                Id = Guid.Parse("ab4360f5-c721-4119-9908-6bf1a50aee08"),
                Name = "User",
                Login = "example@test.com",
                Password = "$2a$11$RxAPh6vaKFA8SEmThlCaPuUY2GHd/gpa9eAvLn87E1YLPj6bXuii2" // "password"
            };

            return user;
        }
    }
}

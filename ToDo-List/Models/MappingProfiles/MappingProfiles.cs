
using AutoMapper;
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddNewCardRequestModel, TaskCard>();

            CreateMap<TaskCardDto, TaskCard>().ReverseMap();
        }
    }
}

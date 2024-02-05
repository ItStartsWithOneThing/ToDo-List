
using AutoMapper;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.Requests;

namespace ToDo_List.Models.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddNewCardRequestModel, TaskCard>();
        }
    }
}

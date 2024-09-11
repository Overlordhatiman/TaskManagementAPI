using AutoMapper;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Task = TaskManagementAPI.Models.Task;

namespace TaskManagementAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map UserRegisterDTO to User, excluding the PasswordHash in reverse mapping
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // No reverse map for UserLoginDTO as it doesn't directly map to a User entity
            CreateMap<User, UserLoginDTO>().ReverseMap();

            // Task mappings
            CreateMap<Task, TaskDTO>().ReverseMap();
        }
    }
}

using System.Runtime.InteropServices;
using AutoMapper;
using UniwayBackend.Models.Dtos;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Role
            CreateMap<Role, RoleDto>();
            CreateMap<MessageResponse<Role>, MessageResponse<RoleDto>>();

            // User
            CreateMap<User, UserDto>();
            CreateMap<AuthenticateResponse<User>, AuthenticateResponse<UserDto>>();

            // WorkshopTechnicalProfession
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionDto>().ReverseMap();
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionRequest>().ReverseMap();
            CreateMap<MessageResponse<WorkshopTechnicalProfession>, MessageResponse<WorkshopTechnicalProfessionDto>>().ReverseMap();
        }

    }
}

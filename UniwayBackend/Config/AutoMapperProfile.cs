using System.Runtime.InteropServices;
using AutoMapper;
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
            CreateMap<Role, RoleResponse>();
            CreateMap<MessageResponse<Role>, MessageResponse<RoleResponse>>();

            // User
            CreateMap<User, UserResponse>();
            CreateMap<AuthenticateResponse<User>, AuthenticateResponse<UserResponse>>();

            // WorkshopTechnicalProfession
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionResponse>().ReverseMap();
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionRequest>().ReverseMap();
            CreateMap<MessageResponse<WorkshopTechnicalProfession>, MessageResponse<WorkshopTechnicalProfessionResponse>>().ReverseMap();
        }

    }
}

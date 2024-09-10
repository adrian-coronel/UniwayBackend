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
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<MessageResponse<Role>, MessageResponse<RoleResponse>>().ReverseMap();

            // User
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<MessageResponse<User>, MessageResponse<UserResponse>>().ReverseMap();
            CreateMap<AuthenticateResponse<User>, AuthenticateResponse<UserResponse>>().ReverseMap();

            // WorkshopTechnicalProfession
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionResponse>().ReverseMap();
            CreateMap<WorkshopTechnicalProfession, WorkshopTechnicalProfessionRequest>().ReverseMap();
            CreateMap<MessageResponse<WorkshopTechnicalProfession>, MessageResponse<WorkshopTechnicalProfessionResponse>>().ReverseMap();

            // Review
            CreateMap<Review, ReviewResponse>().ReverseMap();
            CreateMap<MessageResponse<Review>, MessageResponse<ReviewResponse>>().ReverseMap();
        }

    }
}

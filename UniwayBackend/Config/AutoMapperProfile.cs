using AutoMapper;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Availability;
using UniwayBackend.Models.Payloads.Core.Response.CategoryService;
using UniwayBackend.Models.Payloads.Core.Response.Client;
using UniwayBackend.Models.Payloads.Core.Response.Experience;
using UniwayBackend.Models.Payloads.Core.Response.Profession;
using UniwayBackend.Models.Payloads.Core.Response.Review;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Response.Technical;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;
using UniwayBackend.Models.Payloads.Core.Response.TowingCar;
using UniwayBackend.Models.Payloads.Core.Response.UserTechnical;


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
            CreateMap<Review, ReviewResponseV2>().ReverseMap();
            CreateMap<MessageResponse<Review>, MessageResponse<ReviewResponse>>().ReverseMap();

            // Technical
            CreateMap<Technical, TechnicalResponseV2>().ReverseMap();
            CreateMap<MessageResponse<Technical>, MessageResponse<TechnicalResponseV2>>().ReverseMap();

            // UserTechnical
            CreateMap<UserTechnical, UserTechnicalResponse>().ReverseMap();
            CreateMap<MessageResponse<UserTechnical>, MessageResponse<UserTechnicalResponse>>().ReverseMap();

            // TechnicalProfession
            CreateMap<TechnicalProfession, TechnicalProfessionResponse>().ReverseMap();
            CreateMap<MessageResponse<TechnicalProfession>, MessageResponse<TechnicalProfessionResponse>>().ReverseMap();

            // TowingCar
            CreateMap<TowingCar, TowingCarResponse>().ReverseMap();
            CreateMap<MessageResponse<TowingCar>, MessageResponse<TowingCarResponse>>().ReverseMap();

            // Profession
            CreateMap<Profession, ProfessionResponse>().ReverseMap();
            CreateMap<MessageResponse<Profession>, MessageResponse<ProfessionResponse>>().ReverseMap();

            // Experience
            CreateMap<Experience, ExperienceResponse>().ReverseMap();
            CreateMap<MessageResponse<Experience>, MessageResponse<ExperienceResponse>>().ReverseMap(); 
            
            // Availability
            CreateMap<Availability, AvailabilityResponse>().ReverseMap();
            CreateMap<MessageResponse<Availability>, MessageResponse<AvailabilityResponse>>().ReverseMap();

            // TechnicalProfessionAvailability
            CreateMap<TechnicalProfessionAvailability, TechnicalProfessionAvailabilityResponse>().ReverseMap();
            CreateMap<MessageResponse<TechnicalProfessionAvailability>, MessageResponse<TechnicalProfessionAvailabilityResponse>>().ReverseMap();

            // Workshop
            CreateMap<Workshop, WorkshopResponse>().ReverseMap();
            CreateMap<MessageResponse<Workshop>, MessageResponse<WorkshopResponse>>().ReverseMap();

            // Client
            CreateMap<Client, ClientResponseV2>().ReverseMap();
            CreateMap<MessageResponse<Client>, MessageResponse<ClientResponseV2>>().ReverseMap();

            // CategoryService
            CreateMap<CategoryService, CategoryServiceResponse>().ReverseMap();
            CreateMap<MessageResponse<CategoryService>, MessageResponse<CategoryServiceResponse>>().ReverseMap();

            // ServicesTechnical
            CreateMap<ServiceTechnical, ServiceTechnicalResponse>().ReverseMap();
            CreateMap<MessageResponse<ServiceTechnicalResponse>, MessageResponse<ServiceTechnicalResponse>>().ReverseMap();
        }

    }
}

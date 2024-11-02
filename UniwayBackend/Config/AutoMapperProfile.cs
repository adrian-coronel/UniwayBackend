using AutoMapper;
using NetTopologySuite.Geometries;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request;
using UniwayBackend.Models.Payloads.Core.Request.Material;
using UniwayBackend.Models.Payloads.Core.Request.Request;
using UniwayBackend.Models.Payloads.Core.Request.ServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Request.ServiceTechnicalTypeCar;
using UniwayBackend.Models.Payloads.Core.Request.TechnicalResponse;
using UniwayBackend.Models.Payloads.Core.Request.Workshop;
using UniwayBackend.Models.Payloads.Core.Response;
using UniwayBackend.Models.Payloads.Core.Response.Availability;
using UniwayBackend.Models.Payloads.Core.Response.CategoryRequest;
using UniwayBackend.Models.Payloads.Core.Response.CategoryService;
using UniwayBackend.Models.Payloads.Core.Response.Client;
using UniwayBackend.Models.Payloads.Core.Response.Experience;
using UniwayBackend.Models.Payloads.Core.Response.ImageProblem;
using UniwayBackend.Models.Payloads.Core.Response.ImagesServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Response.Material;
using UniwayBackend.Models.Payloads.Core.Response.Profession;
using UniwayBackend.Models.Payloads.Core.Response.Request;
using UniwayBackend.Models.Payloads.Core.Response.Review;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnical;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnicalTypeCar;
using UniwayBackend.Models.Payloads.Core.Response.StateRequest;
using UniwayBackend.Models.Payloads.Core.Response.Technical;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfession;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailability;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalProfessionAvailabilityRequest;
using UniwayBackend.Models.Payloads.Core.Response.TechnicalResponse;
using UniwayBackend.Models.Payloads.Core.Response.TowingCar;
using UniwayBackend.Models.Payloads.Core.Response.TypeCar;
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
            CreateMap<Technical, TechnicalResponseV1>().ReverseMap();
            CreateMap<MessageResponse<Technical>, MessageResponse<TechnicalResponseV2>>().ReverseMap();
            CreateMap<MessageResponse<Technical>, MessageResponse<TechnicalResponseV1>>().ReverseMap();

            // UserTechnical
            CreateMap<UserTechnical, UserTechnicalResponse>().ReverseMap();
            CreateMap<UserTechnical, UserTechnicalResponseV2>().ReverseMap();
            CreateMap<MessageResponse<UserTechnical>, MessageResponse<UserTechnicalResponse>>().ReverseMap();

            // TechnicalProfession
            CreateMap<TechnicalProfession, TechnicalProfessionResponse>().ReverseMap();
            CreateMap<TechnicalProfession, TechnicalProfessionResponseV2>().ReverseMap();
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
            CreateMap<Workshop, WorkshopRequestV2>().ReverseMap();
            CreateMap<MessageResponse<Workshop>, MessageResponse<WorkshopResponse>>().ReverseMap();

            // Client
            CreateMap<Client, ClientResponseV2>().ReverseMap();
            CreateMap<MessageResponse<Client>, MessageResponse<ClientResponseV2>>().ReverseMap();

            // CategoryService
            CreateMap<CategoryService, CategoryServiceResponse>().ReverseMap();
            CreateMap<MessageResponse<CategoryService>, MessageResponse<CategoryServiceResponse>>().ReverseMap();

            // ServicesTechnical
            CreateMap<ServiceTechnical, ServiceTechnicalRequest>().ReverseMap();
            CreateMap<MessageResponse<ServiceTechnicalRequest>, MessageResponse<ServiceTechnicalRequest>>().ReverseMap();

            // StateRequest
            CreateMap<StateRequest, StateRequestResponse>().ReverseMap();
            CreateMap<MessageResponse<StateRequest>, MessageResponse<StateRequestResponse>>().ReverseMap();

            // CategoryRequest
            CreateMap<CategoryRequest, CategoryRequestResponse>().ReverseMap();
            CreateMap<MessageResponse<CategoryRequest>, MessageResponse<CategoryRequestResponse>>().ReverseMap();

            // Request
            CreateMap<RequestRequest, Request>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Lng, src.Lat) { SRID = 4326 }));
            CreateMap<RequestRequestV2, Request>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Lng, src.Lat) { SRID = 4326 }));
            CreateMap<RequestRequestV3, Request>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Lng, src.Lat) { SRID = 4326 }));
            CreateMap<RequestManyRequestV4, Request>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Lng, src.Lat) { SRID = 4326 }));
            CreateMap<RequestManyRequestV5, Request>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Lng, src.Lat) { SRID = 4326 }));
            CreateMap<Request, RequestResponse>().ReverseMap();
            CreateMap<Request, RequestResponseV3>().ReverseMap();
            CreateMap<UserRequest, RequestResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RequestId));
            CreateMap<ImagesProblemRequest, ImagesProblemRequestResponse>().ReverseMap();
            CreateMap<MessageResponse<Request>, MessageResponse<RequestResponse>>().ReverseMap();
            CreateMap<MessageResponse<Request>, MessageResponse<RequestResponseV3>>().ReverseMap();

            // Location

            // TechnicalResponse
            CreateMap<Models.Entities.TechnicalResponse, TechnicalResponseResponseV2> ().ReverseMap();
            CreateMap<MessageResponse<Models.Entities.TechnicalResponse>, MessageResponse<TechnicalResponseResponseV2>>().ReverseMap();
            CreateMap<Models.Entities.TechnicalResponse, TechnicalResponseRequest>().ReverseMap();
            CreateMap<Models.Entities.TechnicalResponse, TechnicalResponseResponseV3>().ReverseMap();
            CreateMap<Material, MaterialResponse>().ReverseMap();
            CreateMap<Material, MaterialRequest>().ReverseMap();

            // TechnicalProfessionAvailabilityRequest
            CreateMap<TechnicalProfessionAvailabilityRequestResponse, TechnicalProfessionAvailabilityRequestResponseV2>().ReverseMap();
            CreateMap<MessageResponse<TechnicalProfessionAvailabilityRequestResponse>, MessageResponse<TechnicalProfessionAvailabilityRequestResponseV2>>().ReverseMap();

            // ServiceTechnical
            CreateMap<ServiceTechnicalRequestV2, ServiceTechnical>().ReverseMap();
            CreateMap<ServiceTechnical, ServiceTechnicalResponse>().ReverseMap();
            CreateMap<ServiceTechnical, ServiceTechnicalResponse>().ReverseMap();
            CreateMap<MessageResponse<ServiceTechnical>, MessageResponse<ServiceTechnicalResponse>>().ReverseMap();
            CreateMap<ImagesServiceTechnical, ImagesServiceTechnicalResponse>().ReverseMap();
            CreateMap<MessageResponse<ImagesServiceTechnical>, MessageResponse<ImagesServiceTechnicalResponse>>().ReverseMap();
            CreateMap<ImagesServiceTechnical, ImagesServiceTechnicalResponse>().ReverseMap();

            // ServiceTechnicalTypeCar
            CreateMap<ServiceTechnicalTypeCar, ServiceTechnicalTypeCarResponse>().ReverseMap();
            CreateMap<ServiceTechnicalTypeCar, ServiceTechnicalTypeCarRequest>().ReverseMap();
            // TypeCar
            CreateMap<TypeCar, TypeCarResponse>().ReverseMap();
        }

    }
}

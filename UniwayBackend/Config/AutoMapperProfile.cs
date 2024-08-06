using System.Runtime.InteropServices;
using AutoMapper;
using UniwayBackend.Models.Dtos;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads;
using UniwayBackend.Models.Payloads.Auth;

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
        }

    }
}

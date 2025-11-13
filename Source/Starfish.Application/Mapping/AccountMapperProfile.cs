using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

internal class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<UserCreateDto, UserCreateCommand>();

        CreateMap<User, UserProfileDto>();
        CreateMap<User, UserDetailDto>();
        CreateMap<User, UserListDto>();

        CreateMap<Team, TeamDetailDto>();
        CreateMap<Team, TeamListDto>();
    }
}
using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

internal class AccountMapperProfile : Profile
{
    public AccountMapperProfile()
    {
        CreateMap<UserCreateRequestDto, UserCreateCommand>();

        CreateMap<User, UserProfileResultDto>();

        CreateMap<Team, TeamDetailDto>();
        CreateMap<Team, TeamListDto>();
    }
}
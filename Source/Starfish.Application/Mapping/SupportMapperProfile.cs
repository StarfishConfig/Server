using AutoMapper;
using Nerosoft.Starfish.Domain;
using Nerosoft.Starfish.Transit;

namespace Nerosoft.Starfish.Application;

/// <summary>
/// Mapping profile for support module.
/// </summary>
internal class SupportMapperProfile : Profile
{
	public SupportMapperProfile()
	{
		CreateMap<DictionaryRoot, DictionaryRootListDto>();
		CreateMap<DictionaryRoot, DictionaryRootDetailDto>();
		CreateMap<DictionaryItem, DictionaryItemListDto>();
		CreateMap<DictionaryItem, DictionaryItemDetailDto>();

		CreateMap<DictionaryRootCreateDto, DictionaryRootCreateCommand>();
		CreateMap<DictionaryItemCreateDto, DictionaryItemCreateCommand>();

		CreateMap<DictionaryRootUpdateDto, DictionaryRootUpdateCommand>();
		CreateMap<DictionaryItemUpdateDto, DictionaryItemUpdateCommand>();
	}
}
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Application.Termine.Dto;
using Mapster;

namespace LindebergsHealth.Application.Termine.Mapping
{
    public static class TerminMappingConfig
    {
        public static void Register()
        {
            // Entity -> ListDto
            TypeAdapterConfig<Termin, TerminListDto>.NewConfig();

            // Entity -> DetailDto
            TypeAdapterConfig<Termin, TerminDetailDto>.NewConfig();

            // CreateDto -> Entity
            TypeAdapterConfig<CreateTerminDto, Termin>.NewConfig()
                .Ignore(dest => dest.Id); // Id wird im Handler gesetzt

            // UpdateDto -> Entity
            TypeAdapterConfig<UpdateTerminDto, Termin>.NewConfig();
        }
    }
}

using LindebergsHealth.Application.Termine.Dto;

namespace LindebergsHealth.BlazorApp.Models;

public static class TerminUiModelMapping
{
    public static TerminUiModel ToUiModel(this TerminDetailDto dto)
    {
        return new TerminUiModel
        {
            Id = dto.Id,
            Titel = dto.Titel,
            Beschreibung = dto.Beschreibung,
            Datum = dto.Datum,
            DauerMinuten = dto.DauerMinuten,
            RaumName = dto.RaumName,
            PatientName = dto.PatientName
        };
    }

    public static List<TerminUiModel> ToUiModels(this IEnumerable<TerminDetailDto> dtos)
        => dtos.Select(ToUiModel).ToList();
}

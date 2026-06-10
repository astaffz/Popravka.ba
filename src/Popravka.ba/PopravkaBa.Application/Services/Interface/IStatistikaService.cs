using PopravkaBa.Application.DTOs;

namespace PopravkaBa.Application.Services.Interface;

public interface IStatistikaService
{
    Task<StatistikaDTO> DohvatiRangListu(StatistikaFilterDTO filter, CancellationToken ct = default);
}

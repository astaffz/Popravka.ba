using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;

public sealed class StatistikaSortResolver : IStatistikaSortResolver
{
    private readonly IReadOnlyDictionary<KoloneStatistike, IStatistikaStrategy> _map;
    private readonly IStatistikaStrategy _default;

    public StatistikaSortResolver(IEnumerable<IStatistikaStrategy> strategije)
    {
        _map = strategije.ToDictionary(s => s.KolonaSortiranja);
        _default = _map[KoloneStatistike.Ocjena];
    }

    public IStatistikaStrategy SortPo(KoloneStatistike kolona)
        => _map.TryGetValue(kolona, out var s) ? s : _default;
}
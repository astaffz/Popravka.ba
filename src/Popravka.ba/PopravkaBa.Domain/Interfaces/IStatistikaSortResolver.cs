using PopravkaBa.Domain.Enums;


namespace PopravkaBa.Application.Strategies.Interface
{

    public interface IStatistikaSortResolver
    {
        IStatistikaStrategy SortPo(KoloneStatistike kolona);
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Enums
{
    public enum Sortiranje
    {
        Ascending,
        Descending
    }
    public enum SortiranjeIzvrsilacaUsluga
    {
        ProsjecnaOcjena_Asc,
        ProsjecnaOcjena_Desc,
        MinCijena_Asc,
        MinCijena_Desc
    }

    public enum SortiranjeOglasaUsluge
    {
        MinBudzet_Asc,
        MinBudzet_Desc,
        MaxBudzet_Asc,
        MaxBudzet_Desc
    }

    public enum SortiranjeOglasaRadnoMjesto
    {
        MinPrihod_Asc,
        MinPrihod_Desc,
        MaxPrihod_Asc,
        MaxPrihod_Desc
    }
}

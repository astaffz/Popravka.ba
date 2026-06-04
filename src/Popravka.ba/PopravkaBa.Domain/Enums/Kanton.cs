using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PopravkaBa.Domain.Enums
{
    public enum Kanton
    {
        [Display(Name = "Unsko-sanski kanton")]
        UnskoSanski = 1,

        [Display(Name = "Posavski kanton")]
        Posavski = 2,

        [Display(Name = "Tuzlanski kanton")]
        Tuzlanski = 3,

        [Display(Name = "Zeničko-dobojski kanton")]
        ZenickoDobojski = 4,

        [Display(Name = "Bosansko-podrinjski kanton")]
        BosanskoPodrinjski = 5,

        [Display(Name = "Srednjobosanski kanton")]
        SrednjoBosanski = 6,

        [Display(Name = "Hercegovačko-neretvanski kanton")]
        HercegovackoNeretvanski = 7,

        [Display(Name = "Zapadnohercegovački kanton")]
        ZapadnoHercegovacki = 8,

        [Display(Name = "Kanton Sarajevo")]
        Sarajevski = 9,

        [Display(Name = "Kanton 10")]
        Kanton10 = 10,

        [Display(Name = "Republika Srpska")]
        RepublikaSrpska = 11,

        [Display(Name = "Brčko Distrikt")]
        BrckoDistrikt = 12
    }
}

using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IPretragaService
    {
        Task<IEnumerable<Oglas>> PretraziAsync(FilterPretrageDto filteri, KorisnickeUloge uloga);
    }
}

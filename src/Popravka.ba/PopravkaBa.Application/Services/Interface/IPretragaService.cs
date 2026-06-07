using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Interface;
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
        Task<RezultatPretrageDto> PretraziAsync(FilterPretrageDto filteri, IPretragaStrategy strategija);
    }
}

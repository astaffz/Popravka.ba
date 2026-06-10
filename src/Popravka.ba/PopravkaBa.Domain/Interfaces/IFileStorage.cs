using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IFileStorage 
    {
        Task<string> SpremiSlikuPublic(Stream sadrzaj, string contentType, CancellationToken ct = default);
        Task<string> SpremiSlikuPrivate(Stream sadrzaj, string imeFajla, string contentType, CancellationToken ct = default);
        string GetSignedURL(string key, TimeSpan trajanje);
        Task ObrisiPublic(string url, CancellationToken ct = default);
        Task ObrisiPrivate(string key, CancellationToken ct = default);
    }
}

using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Records;

namespace PopravkaBa.Infrastructure.Repositories;

public class MjesecnaStatistikaRepository : IMjesecnaStatistikaRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IStatistikaSortResolver _sort;

    public MjesecnaStatistikaRepository(ApplicationDbContext context, IStatistikaSortResolver sort)
    {
        _context = context;
        _sort = sort;
    }

    public async Task<IReadOnlyList<MjesecnaStatistikaKompozicija>> DohvatiFiltrirano(
        int godina, int mjesec, int? kategorijaId, int? mjestoId,
        KoloneStatistike kolona, Sortiranje smjer, CancellationToken ct = default)
    {
        var upit = _context.MjesecnaStatistikaKompozicija
            .AsNoTracking()
            .Where(s => s.Godina == godina && s.Mjesec == mjesec);

        if (kategorijaId.HasValue) upit = upit.Where(s => s.KategorijaID == kategorijaId.Value);
        if (mjestoId.HasValue) upit = upit.Where(s => s.MjestoID == mjestoId.Value);

        var poredano = _sort.SortPo(kolona).Primijeni(upit, smjer).ThenBy(s => s.RangStandardni);

        return await poredano.ToListAsync(ct);
    }

    public async Task<StatistikaMetaRecord> DohvatiMeta(int godina, int mjesec, CancellationToken ct = default)
    {
        var upit = _context.MjesecnaStatistikaKompozicija
            .AsNoTracking()
            .Where(s => s.Godina == godina && s.Mjesec == mjesec);

        var vrijemeAzuriranja = await upit.MaxAsync(s => (DateTime?)s.VrijemeAzuriranja, ct);

        var kategorijeZaSort = (await upit
                .Where(s => s.KategorijaID != null)
                .Select(s => new { Id = s.KategorijaID!.Value, s.KategorijaNaziv})
                .Distinct()
                .ToListAsync(ct))
            .OrderBy(x => x.KategorijaNaziv)
            .Select(x => new StatistikaRecord(x.Id, x.KategorijaNaziv))
            .ToList();

        var lokacijeZaSort = (await upit
                .Where(s => s.MjestoID != null)
                .Select(s => new { Id = s.MjestoID!.Value, s.MjestoNaziv })
                .Distinct()
                .ToListAsync(ct))
            .OrderBy(x => x.MjestoNaziv)
            .Select(x => new StatistikaRecord(x.Id, x.MjestoNaziv))
            .ToList();

        return new StatistikaMetaRecord(vrijemeAzuriranja, kategorijeZaSort, lokacijeZaSort);
    }

    public async Task ZamijeniSnapshot(
        int godina, int mjesec,
        IReadOnlyList<MjesecnaStatistikaKompozicija> redovi,
        CancellationToken ct = default)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        await _context.MjesecnaStatistikaKompozicija
            .Where(s => s.Godina == godina && s.Mjesec == mjesec)
            .ExecuteDeleteAsync(ct);

        if (redovi.Count > 0)
        {
            await _context.MjesecnaStatistikaKompozicija.AddRangeAsync(redovi, ct);
            await _context.SaveChangesAsync(ct);
        }

        await tx.CommitAsync(ct);
    }
}
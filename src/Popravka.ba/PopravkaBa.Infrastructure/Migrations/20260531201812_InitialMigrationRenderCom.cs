using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PopravkaBa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationRenderCom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: true),
                    Prezime = table.Column<string>(type: "text", nullable: true),
                    DatumRegistracije = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Slika = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Adresa = table.Column<string>(type: "text", nullable: true),
                    StambeniBroj = table.Column<string>(type: "text", nullable: true),
                    ProsjecnaOcjena = table.Column<double>(type: "double precision", nullable: true),
                    BrojZavrsenihPoslova = table.Column<int>(type: "integer", nullable: true),
                    Opis = table.Column<string>(type: "text", nullable: true),
                    NazivFirme = table.Column<string>(type: "text", nullable: true),
                    MinZaposlenih = table.Column<int>(type: "integer", nullable: true),
                    MaxZaposlenih = table.Column<int>(type: "integer", nullable: true),
                    StatusVerifikacije = table.Column<int>(type: "integer", nullable: true),
                    OtvorenoOd = table.Column<TimeSpan>(type: "interval", nullable: true),
                    OtvorenoDo = table.Column<TimeSpan>(type: "interval", nullable: true),
                    VelicinaFirme = table.Column<int>(type: "integer", nullable: true),
                    RadnoVrijeme = table.Column<string>(type: "text", nullable: true),
                    WebStranica = table.Column<string>(type: "text", nullable: true),
                    DatumOsnivanja = table.Column<DateOnly>(type: "date", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "text", nullable: false),
                    NadkategorijaID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Kategorija_Kategorija_NadkategorijaID",
                        column: x => x.NadkategorijaID,
                        principalTable: "Kategorija",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Mjesto",
                columns: table => new
                {
                    MjestoID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "text", nullable: false),
                    Kanton = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mjesto", x => x.MjestoID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioSlika",
                columns: table => new
                {
                    PortfolioSlikaID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IzvrsilacID = table.Column<string>(type: "text", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioSlika", x => x.PortfolioSlikaID);
                    table.ForeignKey(
                        name: "FK_PortfolioSlika_AspNetUsers_IzvrsilacID",
                        column: x => x.IzvrsilacID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzija",
                columns: table => new
                {
                    RecenzijaID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KlijentID = table.Column<string>(type: "text", nullable: false),
                    IzvrsilacID = table.Column<string>(type: "text", nullable: false),
                    Ocjena = table.Column<int>(type: "integer", nullable: false),
                    Komentar = table.Column<string>(type: "text", nullable: false),
                    DatumRecenzije = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Prijavljena = table.Column<bool>(type: "boolean", nullable: false),
                    RazlogPrijave = table.Column<string>(type: "text", nullable: true),
                    DatumPrijave = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusPrijave = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzija", x => x.RecenzijaID);
                    table.CheckConstraint("CK_Recenzija_Ocjena", "\"Ocjena\" >= 1 AND \"Ocjena\" <= 5");
                    table.ForeignKey(
                        name: "FK_Recenzija_AspNetUsers_IzvrsilacID",
                        column: x => x.IzvrsilacID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recenzija_AspNetUsers_KlijentID",
                        column: x => x.KlijentID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VerifikacijaFirme",
                columns: table => new
                {
                    VerifikacioniID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirmaID = table.Column<string>(type: "text", nullable: false),
                    NazivFirme = table.Column<string>(type: "text", nullable: false),
                    StatusVerifikacije = table.Column<int>(type: "integer", nullable: false),
                    SjedisteFirme = table.Column<string>(type: "text", nullable: false),
                    RadnoVrijeme = table.Column<string>(type: "text", nullable: true),
                    WebStranica = table.Column<string>(type: "text", nullable: true),
                    KontaktTelefon = table.Column<string>(type: "text", nullable: false),
                    OdgovornaOsobaIme = table.Column<string>(type: "text", nullable: false),
                    OdgovornaOsobaPrezime = table.Column<string>(type: "text", nullable: false),
                    OdgovornaOsobaEmail = table.Column<string>(type: "text", nullable: false),
                    OdgovornaOsobaPozicija = table.Column<string>(type: "text", nullable: false),
                    OdgovornaOsobaTelefon = table.Column<string>(type: "text", nullable: false),
                    Rjesenje = table.Column<string>(type: "text", nullable: false),
                    PoreznoUvjerenje = table.Column<string>(type: "text", nullable: false),
                    LicencaDjelatnosti = table.Column<string>(type: "text", nullable: true),
                    Logotip = table.Column<string>(type: "text", nullable: false),
                    DatumPodnosenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumObrade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifikacijaFirme", x => x.VerifikacioniID);
                    table.ForeignKey(
                        name: "FK_VerifikacijaFirme_AspNetUsers_FirmaID",
                        column: x => x.FirmaID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IzvrsilacKategorija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IzvrsilacID = table.Column<string>(type: "text", nullable: false),
                    KategorijaID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IzvrsilacKategorija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IzvrsilacKategorija_AspNetUsers_IzvrsilacID",
                        column: x => x.IzvrsilacID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IzvrsilacKategorija_Kategorija_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KorisnikMjesto",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KorisnikID = table.Column<string>(type: "text", nullable: false),
                    MjestoID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnikMjesto", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KorisnikMjesto_AspNetUsers_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KorisnikMjesto_Mjesto_MjestoID",
                        column: x => x.MjestoID,
                        principalTable: "Mjesto",
                        principalColumn: "MjestoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oglas",
                columns: table => new
                {
                    OglasID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naslov = table.Column<string>(type: "text", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: false),
                    DatumObjave = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MjestoID = table.Column<int>(type: "integer", nullable: false),
                    VlasnikOglasaID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oglas", x => x.OglasID);
                    table.ForeignKey(
                        name: "FK_Oglas_AspNetUsers_VlasnikOglasaID",
                        column: x => x.VlasnikOglasaID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oglas_Mjesto_MjestoID",
                        column: x => x.MjestoID,
                        principalTable: "Mjesto",
                        principalColumn: "MjestoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerifikacijaFirme",
                columns: table => new
                {
                    VerifikacioniID = table.Column<int>(type: "integer", nullable: false),
                    AdminEmail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifikacijaFirme", x => x.VerifikacioniID);
                    table.ForeignKey(
                        name: "FK_EmailVerifikacijaFirme_VerifikacijaFirme_VerifikacioniID",
                        column: x => x.VerifikacioniID,
                        principalTable: "VerifikacijaFirme",
                        principalColumn: "VerifikacioniID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifikacijaOglas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tekst = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifikacijaOglas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotifikacijaOglas_Oglas_OglasID",
                        column: x => x.OglasID,
                        principalTable: "Oglas",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OglasKategorija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    KategorijaID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasKategorija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OglasKategorija_Kategorija_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorija",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OglasKategorija_Oglas_OglasID",
                        column: x => x.OglasID,
                        principalTable: "Oglas",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OglasMajstora",
                columns: table => new
                {
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    MinCijena = table.Column<double>(type: "double precision", nullable: false),
                    TipIsplate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasMajstora", x => x.OglasID);
                    table.ForeignKey(
                        name: "FK_OglasMajstora_Oglas_OglasID",
                        column: x => x.OglasID,
                        principalTable: "Oglas",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OglasRadnoMjesto",
                columns: table => new
                {
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    BrojIzvrsilaca = table.Column<int>(type: "integer", nullable: false),
                    VrstaZaposlenja = table.Column<int>(type: "integer", nullable: false),
                    MinPrihod = table.Column<int>(type: "integer", nullable: false),
                    MaxPrihod = table.Column<int>(type: "integer", nullable: false),
                    TipIsplate = table.Column<int>(type: "integer", nullable: false),
                    BrojPrijava = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasRadnoMjesto", x => x.OglasID);
                    table.ForeignKey(
                        name: "FK_OglasRadnoMjesto_Oglas_OglasID",
                        column: x => x.OglasID,
                        principalTable: "Oglas",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OglasUsluge",
                columns: table => new
                {
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    MinBudzet = table.Column<int>(type: "integer", nullable: false),
                    MaxBudzet = table.Column<int>(type: "integer", nullable: false),
                    BrojPrijava = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasUsluge", x => x.OglasID);
                    table.ForeignKey(
                        name: "FK_OglasUsluge_Oglas_OglasID",
                        column: x => x.OglasID,
                        principalTable: "Oglas",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailNotifikacijaOglasa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    EmailPrimalac = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotifikacijaOglasa", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmailNotifikacijaOglasa_NotifikacijaOglas_ID",
                        column: x => x.ID,
                        principalTable: "NotifikacijaOglas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OglasVozackaDozvola",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VozackaDozvola = table.Column<int>(type: "integer", nullable: false),
                    OglasID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OglasVozackaDozvola", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OglasVozackaDozvola_OglasRadnoMjesto_OglasID",
                        column: x => x.OglasID,
                        principalTable: "OglasRadnoMjesto",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavaRadnoMjesto",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    MajstorID = table.Column<string>(type: "text", nullable: false),
                    VrijemePrijave = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusPrijave = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavaRadnoMjesto", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PrijavaRadnoMjesto_AspNetUsers_MajstorID",
                        column: x => x.MajstorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrijavaRadnoMjesto_OglasRadnoMjesto_OglasID",
                        column: x => x.OglasID,
                        principalTable: "OglasRadnoMjesto",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UvjetOglasa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OglasID = table.Column<int>(type: "integer", nullable: false),
                    TekstUvjeta = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UvjetOglasa", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UvjetOglasa_OglasRadnoMjesto_OglasID",
                        column: x => x.OglasID,
                        principalTable: "OglasRadnoMjesto",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PonudaUsluge",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IzvrsilacID = table.Column<string>(type: "text", nullable: false),
                    OglasUslugeID = table.Column<int>(type: "integer", nullable: false),
                    DatumSlanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumPocetkaUsluge = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumOcekivanogZavrsetka = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusPonude = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PonudaUsluge", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PonudaUsluge_AspNetUsers_IzvrsilacID",
                        column: x => x.IzvrsilacID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PonudaUsluge_OglasUsluge_OglasUslugeID",
                        column: x => x.OglasUslugeID,
                        principalTable: "OglasUsluge",
                        principalColumn: "OglasID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IzvrsilacKategorija_IzvrsilacID",
                table: "IzvrsilacKategorija",
                column: "IzvrsilacID");

            migrationBuilder.CreateIndex(
                name: "IX_IzvrsilacKategorija_KategorijaID",
                table: "IzvrsilacKategorija",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kategorija_NadkategorijaID",
                table: "Kategorija",
                column: "NadkategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikMjesto_KorisnikID",
                table: "KorisnikMjesto",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnikMjesto_MjestoID",
                table: "KorisnikMjesto",
                column: "MjestoID");

            migrationBuilder.CreateIndex(
                name: "IX_NotifikacijaOglas_OglasID",
                table: "NotifikacijaOglas",
                column: "OglasID");

            migrationBuilder.CreateIndex(
                name: "IX_Oglas_MjestoID",
                table: "Oglas",
                column: "MjestoID");

            migrationBuilder.CreateIndex(
                name: "IX_Oglas_VlasnikOglasaID",
                table: "Oglas",
                column: "VlasnikOglasaID");

            migrationBuilder.CreateIndex(
                name: "IX_OglasKategorija_KategorijaID",
                table: "OglasKategorija",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_OglasKategorija_OglasID",
                table: "OglasKategorija",
                column: "OglasID");

            migrationBuilder.CreateIndex(
                name: "IX_OglasVozackaDozvola_OglasID",
                table: "OglasVozackaDozvola",
                column: "OglasID");

            migrationBuilder.CreateIndex(
                name: "IX_PonudaUsluge_IzvrsilacID",
                table: "PonudaUsluge",
                column: "IzvrsilacID");

            migrationBuilder.CreateIndex(
                name: "IX_PonudaUsluge_OglasUslugeID",
                table: "PonudaUsluge",
                column: "OglasUslugeID");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioSlika_IzvrsilacID",
                table: "PortfolioSlika",
                column: "IzvrsilacID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaRadnoMjesto_MajstorID",
                table: "PrijavaRadnoMjesto",
                column: "MajstorID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavaRadnoMjesto_OglasID",
                table: "PrijavaRadnoMjesto",
                column: "OglasID");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzija_IzvrsilacID",
                table: "Recenzija",
                column: "IzvrsilacID");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzija_KlijentID",
                table: "Recenzija",
                column: "KlijentID");

            migrationBuilder.CreateIndex(
                name: "IX_UvjetOglasa_OglasID",
                table: "UvjetOglasa",
                column: "OglasID");

            migrationBuilder.CreateIndex(
                name: "IX_VerifikacijaFirme_FirmaID",
                table: "VerifikacijaFirme",
                column: "FirmaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmailNotifikacijaOglasa");

            migrationBuilder.DropTable(
                name: "EmailVerifikacijaFirme");

            migrationBuilder.DropTable(
                name: "IzvrsilacKategorija");

            migrationBuilder.DropTable(
                name: "KorisnikMjesto");

            migrationBuilder.DropTable(
                name: "OglasKategorija");

            migrationBuilder.DropTable(
                name: "OglasMajstora");

            migrationBuilder.DropTable(
                name: "OglasVozackaDozvola");

            migrationBuilder.DropTable(
                name: "PonudaUsluge");

            migrationBuilder.DropTable(
                name: "PortfolioSlika");

            migrationBuilder.DropTable(
                name: "PrijavaRadnoMjesto");

            migrationBuilder.DropTable(
                name: "Recenzija");

            migrationBuilder.DropTable(
                name: "UvjetOglasa");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NotifikacijaOglas");

            migrationBuilder.DropTable(
                name: "VerifikacijaFirme");

            migrationBuilder.DropTable(
                name: "Kategorija");

            migrationBuilder.DropTable(
                name: "OglasUsluge");

            migrationBuilder.DropTable(
                name: "OglasRadnoMjesto");

            migrationBuilder.DropTable(
                name: "Oglas");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Mjesto");
        }
    }
}

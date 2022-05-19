using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Models
{
    public partial class KlinikaContext : DbContext, IKlinikaContext
    {
        public KlinikaContext()
        {
        }

        public KlinikaContext(DbContextOptions<KlinikaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Choroba> Chorobas { get; set; }
        public virtual DbSet<ChorobaLek> ChorobaLeks { get; set; }
        public virtual DbSet<GodzinyPracy> GodzinyPracies { get; set; }
        public virtual DbSet<Harmonogram> Harmonograms { get; set; }
        public virtual DbSet<Klient> Klients { get; set; }
        public virtual DbSet<KlientZnizka> KlientZnizkas { get; set; }
        public virtual DbSet<Lek> Leks { get; set; }
        public virtual DbSet<LekWMagazynie> LekWMagazynies { get; set; }
        public virtual DbSet<LekWizytum> LekWizyta { get; set; }
        public virtual DbSet<Nagrodum> Nagroda { get; set; }
        public virtual DbSet<Osoba> Osobas { get; set; }
        public virtual DbSet<Pacjent> Pacjents { get; set; }
        public virtual DbSet<ReceptaLek> ReceptaLeks { get; set; }
        public virtual DbSet<Receptum> Recepta { get; set; }
        public virtual DbSet<Skierowanie> Skierowanies { get; set; }
        public virtual DbSet<Specjalizacja> Specjalizacjas { get; set; }
        public virtual DbSet<Szczepienie> Szczepienies { get; set; }
        public virtual DbSet<Szczepionka> Szczepionkas { get; set; }
        public virtual DbSet<Usluga> Uslugas { get; set; }
        public virtual DbSet<Weterynarz> Weterynarzs { get; set; }
        public virtual DbSet<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
        public virtual DbSet<WizytaChoroba> WizytaChorobas { get; set; }
        public virtual DbSet<WizytaUsluga> WizytaUslugas { get; set; }
        public virtual DbSet<Wizytum> Wizyta { get; set; }
        public virtual DbSet<Zdjecie> Zdjecies { get; set; }
        public virtual DbSet<Znizka> Znizkas { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Choroba>(entity =>
            {
                entity.HasKey(e => e.IdChoroba)
                    .HasName("Choroba_pk");

                entity.ToTable("Choroba");

                entity.Property(e => e.IdChoroba).HasColumnName("ID_choroba");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ChorobaLek>(entity =>
            {
                entity.HasKey(e => new { e.IdChoroba, e.IdLek })
                    .HasName("Choroba_lek_pk");

                entity.ToTable("Choroba_lek");

                entity.Property(e => e.IdChoroba).HasColumnName("ID_choroba");

                entity.Property(e => e.IdLek).HasColumnName("ID_lek");

                entity.HasOne(d => d.IdChorobaNavigation)
                    .WithMany(p => p.ChorobaLeks)
                    .HasForeignKey(d => d.IdChoroba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("choroba_lek_Choroba");

                entity.HasOne(d => d.IdLekNavigation)
                    .WithMany(p => p.ChorobaLeks)
                    .HasForeignKey(d => d.IdLek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("choroba_lek_Lek");
            });

            modelBuilder.Entity<GodzinyPracy>(entity =>
            {
                entity.HasKey(e => e.IdGodzinyPracy)
                    .HasName("Godziny_pracy_pk");

                entity.ToTable("Godziny_pracy");

                entity.Property(e => e.IdGodzinyPracy).HasColumnName("ID_godziny_pracy");

                entity.Property(e => e.DzienTygodnia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Dzien_tygodnia");

                entity.Property(e => e.GodzinaRozpoczecia)
                    .HasColumnType("time(0)")
                    .HasColumnName("Godzina_rozpoczecia");

                entity.Property(e => e.GodzinaZakonczenia)
                    .HasColumnType("time(0)")
                    .HasColumnName("Godzina_zakonczenia");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.GodzinyPracies)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_35_Weterynarz");
            });

            modelBuilder.Entity<Harmonogram>(entity =>
            {
                entity.HasKey(e => e.IdHarmonogram)
                    .HasName("Harmonogram_pk");

                entity.ToTable("Harmonogram");

                entity.Property(e => e.IdHarmonogram).HasColumnName("ID_harmonogram");

                entity.Property(e => e.DataRozpoczecia)
                    .HasColumnType("datetime")
                    .HasColumnName("Data_rozpoczecia");

                entity.Property(e => e.DataZakonczenia)
                    .HasColumnType("datetime")
                    .HasColumnName("Data_zakonczenia");

                entity.Property(e => e.IdPacjent).HasColumnName("ID_pacjent");

                entity.Property(e => e.KlientIdOsoba).HasColumnName("Klient_ID_osoba");

                entity.Property(e => e.WeterynarzIdOsoba).HasColumnName("Weterynarz_ID_osoba");

                entity.HasOne(d => d.IdPacjentNavigation)
                    .WithMany(p => p.Harmonograms)
                    .HasForeignKey(d => d.IdPacjent)
                    .HasConstraintName("Harmonogram_Pacjent");

                entity.HasOne(d => d.KlientIdOsobaNavigation)
                    .WithMany(p => p.Harmonograms)
                    .HasForeignKey(d => d.KlientIdOsoba)
                    .HasConstraintName("Harmonogram_Klient");

                entity.HasOne(d => d.WeterynarzIdOsobaNavigation)
                    .WithMany(p => p.Harmonograms)
                    .HasForeignKey(d => d.WeterynarzIdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Harmonogram_Weterynarz");
            });

            modelBuilder.Entity<Klient>(entity =>
            {
                entity.HasKey(e => e.IdOsoba)
                    .HasName("Klient_pk");

                entity.ToTable("Klient");

                entity.Property(e => e.IdOsoba)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_osoba");

                entity.Property(e => e.DataZalozeniaKonta)
                    .HasColumnType("date")
                    .HasColumnName("Data_zalozenia_konta");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithOne(p => p.Klient)
                    .HasForeignKey<Klient>(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Klient_Osoba");
            });

            modelBuilder.Entity<KlientZnizka>(entity =>
            {
                entity.HasKey(e => new { e.IdOsoba, e.IdZnizka })
                    .HasName("Klient_znizka_pk");

                entity.ToTable("Klient_znizka");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.IdZnizka).HasColumnName("ID_znizka");

                entity.Property(e => e.CzyWykorzystana).HasColumnName("Czy_wykorzystana");

                entity.Property(e => e.DataPrzyznania)
                    .HasColumnType("date")
                    .HasColumnName("Data_przyznania");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.KlientZnizkas)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Klient_znizka_Klient");

                entity.HasOne(d => d.IdZnizkaNavigation)
                    .WithMany(p => p.KlientZnizkas)
                    .HasForeignKey(d => d.IdZnizka)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Klient_znizka_Znizka");
            });

            modelBuilder.Entity<Lek>(entity =>
            {
                entity.HasKey(e => e.IdLek)
                    .HasName("Lek_pk");

                entity.ToTable("Lek");

                entity.HasIndex(e => e.Nazwa, "Lek_idx_1");

                entity.Property(e => e.IdLek).HasColumnName("ID_lek");

                entity.Property(e => e.JednostkaMiary)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Jednostka_miary");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LekWMagazynie>(entity =>
            {
                entity.HasKey(e => e.IdStanLeku)
                    .HasName("Lek_w_magazynie_pk");

                entity.ToTable("Lek_w_magazynie");

                entity.Property(e => e.IdStanLeku).HasColumnName("ID_stan_leku");

                entity.Property(e => e.DataWaznosci)
                    .HasColumnType("date")
                    .HasColumnName("Data_waznosci");

                entity.Property(e => e.IdLek).HasColumnName("ID_lek");

                entity.HasOne(d => d.IdLekNavigation)
                    .WithMany(p => p.LekWMagazynies)
                    .HasForeignKey(d => d.IdLek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Lek_w_magazynie_Lek");
            });

            modelBuilder.Entity<LekWizytum>(entity =>
            {
                entity.HasKey(e => new { e.IdWizyta, e.IdLek })
                    .HasName("Lek_wizyta_pk");

                entity.ToTable("Lek_wizyta");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.IdLek).HasColumnName("ID_lek");

                entity.HasOne(d => d.IdLekNavigation)
                    .WithMany(p => p.LekWizyta)
                    .HasForeignKey(d => d.IdLek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Lek_wizyta_Lek");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.LekWizyta)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Lek_wizyta_Wizyta");
            });

            modelBuilder.Entity<Nagrodum>(entity =>
            {
                entity.HasKey(e => e.IdNagroda)
                    .HasName("Nagroda_pk");

                entity.Property(e => e.IdNagroda).HasColumnName("ID_nagroda");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.Nagroda)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Nagroda_Weterynarz");
            });

            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.HasKey(e => e.IdOsoba)
                    .HasName("Osoba_pk");

                entity.ToTable("Osoba");

                entity.HasIndex(e => e.NazwaUzytkownika, "Nazwa_uzytkownika_index");

                entity.HasIndex(e => e.NazwaUzytkownika, "Osoba_ak_1")
                    .IsUnique();

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.DataBlokady)
                    .HasColumnType("datetime")
                    .HasColumnName("Data_blokady");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Haslo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LiczbaProb).HasColumnName("Liczba_prob");

                entity.Property(e => e.NazwaUzytkownika)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nazwa_uzytkownika");

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumerTelefonu)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Numer_telefonu");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshTokenExp).HasColumnType("date");

                entity.Property(e => e.Rola)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pacjent>(entity =>
            {
                entity.HasKey(e => e.IdPacjent)
                    .HasName("Pacjent_pk");

                entity.ToTable("Pacjent");

                entity.Property(e => e.IdPacjent).HasColumnName("ID_pacjent");

                entity.Property(e => e.DataUrodzenia)
                    .HasColumnType("date")
                    .HasColumnName("Data_urodzenia");

                entity.Property(e => e.Gatunek)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.Masc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Plec)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Rasa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Waga).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.Pacjents)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Pacjent_Klient");
            });

            modelBuilder.Entity<ReceptaLek>(entity =>
            {
                entity.HasKey(e => new { e.IdLek, e.IdWizyta })
                    .HasName("Recepta_lek_pk");

                entity.ToTable("Recepta_lek");

                entity.Property(e => e.IdLek).HasColumnName("ID_lek");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.HasOne(d => d.IdLekNavigation)
                    .WithMany(p => p.ReceptaLeks)
                    .HasForeignKey(d => d.IdLek)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recepta_lek_Lek");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.ReceptaLeks)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recepta_lek_Recepta");
            });

            modelBuilder.Entity<Receptum>(entity =>
            {
                entity.HasKey(e => e.IdWizyta)
                    .HasName("Recepta_pk");

                entity.Property(e => e.IdWizyta)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_wizyta");

                entity.Property(e => e.Zalecenia)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithOne(p => p.Receptum)
                    .HasForeignKey<Receptum>(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recepta_Wizyta");
            });

            modelBuilder.Entity<Skierowanie>(entity =>
            {
                entity.HasKey(e => new { e.IdUsluga, e.IdOsoba })
                    .HasName("Skierowanie_pk");

                entity.ToTable("Skierowanie");

                entity.Property(e => e.IdUsluga).HasColumnName("ID_usluga");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.DataWystawienia)
                    .HasColumnType("date")
                    .HasColumnName("Data_wystawienia");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.Skierowanies)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skierowanie_Klient");

                entity.HasOne(d => d.IdUslugaNavigation)
                    .WithMany(p => p.Skierowanies)
                    .HasForeignKey(d => d.IdUsluga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skierowanie_Usluga");
            });

            modelBuilder.Entity<Specjalizacja>(entity =>
            {
                entity.HasKey(e => e.IdSpecjalizacja)
                    .HasName("Specjalizacja_pk");

                entity.ToTable("Specjalizacja");

                entity.Property(e => e.IdSpecjalizacja).HasColumnName("ID_specjalizacja");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Szczepienie>(entity =>
            {
                entity.HasKey(e => new { e.IdSzczepionka, e.IdWizyta })
                    .HasName("Szczepienie_pk");

                entity.ToTable("Szczepienie");

                entity.Property(e => e.IdSzczepionka).HasColumnName("ID_szczepionka");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.DataWaznosci)
                    .HasColumnType("date")
                    .HasColumnName("Data_waznosci");

                entity.HasOne(d => d.IdSzczepionkaNavigation)
                    .WithMany(p => p.Szczepienies)
                    .HasForeignKey(d => d.IdSzczepionka)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Szczepienie_Pacjent_Szczepienie");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.Szczepienies)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Szczepienie_Wizyta");
            });

            modelBuilder.Entity<Szczepionka>(entity =>
            {
                entity.HasKey(e => e.IdSzczepionka)
                    .HasName("Szczepionka_pk");

                entity.ToTable("Szczepionka");

                entity.Property(e => e.IdSzczepionka).HasColumnName("ID_szczepionka");

                entity.Property(e => e.CzyObowiazkowa).HasColumnName("Czy_obowiazkowa");

                entity.Property(e => e.Nazwa)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zastosowanie)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usluga>(entity =>
            {
                entity.HasKey(e => e.IdUsluga)
                    .HasName("Usluga_pk");

                entity.ToTable("Usluga");

                entity.Property(e => e.IdUsluga).HasColumnName("ID_usluga");

                entity.Property(e => e.Cena).HasColumnType("money");

                entity.Property(e => e.Dolegliwosc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NazwaUslugi)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nazwa_uslugi");

                entity.Property(e => e.Opis)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Weterynarz>(entity =>
            {
                entity.HasKey(e => e.IdOsoba)
                    .HasName("Weterynarz_pk");

                entity.ToTable("Weterynarz");

                entity.Property(e => e.IdOsoba)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_osoba");

                entity.Property(e => e.DataUrodzenia)
                    .HasColumnType("date")
                    .HasColumnName("Data_urodzenia");

                entity.Property(e => e.DataZatrudnienia)
                    .HasColumnType("date")
                    .HasColumnName("Data_zatrudnienia");

                entity.Property(e => e.Pensja).HasColumnType("money");

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithOne(p => p.Weterynarz)
                    .HasForeignKey<Weterynarz>(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Weterynarz_Osoba");
            });

            modelBuilder.Entity<WeterynarzSpecjalizacja>(entity =>
            {
                entity.HasKey(e => new { e.IdOsoba, e.IdSpecjalizacja })
                    .HasName("Weterynarz_specjalizacja_pk");

                entity.ToTable("Weterynarz_specjalizacja");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.IdSpecjalizacja).HasColumnName("ID_specjalizacja");

                entity.Property(e => e.DataUzyskania)
                    .HasColumnType("date")
                    .HasColumnName("Data_uzyskania");

                entity.Property(e => e.Opis)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.WeterynarzSpecjalizacjas)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Weterynarz_specjalizacja_Weterynarz");

                entity.HasOne(d => d.IdSpecjalizacjaNavigation)
                    .WithMany(p => p.WeterynarzSpecjalizacjas)
                    .HasForeignKey(d => d.IdSpecjalizacja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Weterynarz_specjalizacja_Specjalizacja");
            });

            modelBuilder.Entity<WizytaChoroba>(entity =>
            {
                entity.HasKey(e => new { e.IdWizyta, e.IdChoroba })
                    .HasName("Wizyta_choroba_pk");

                entity.ToTable("Wizyta_choroba");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.IdChoroba).HasColumnName("ID_choroba");

                entity.HasOne(d => d.IdChorobaNavigation)
                    .WithMany(p => p.WizytaChorobas)
                    .HasForeignKey(d => d.IdChoroba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_choroba_Choroba");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.WizytaChorobas)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_choroba_Wizyta");
            });

            modelBuilder.Entity<WizytaUsluga>(entity =>
            {
                entity.HasKey(e => new { e.IdWizyta, e.IdUsluga })
                    .HasName("Wizyta_usluga_pk");

                entity.ToTable("Wizyta_usluga");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.IdUsluga).HasColumnName("ID_usluga");

                entity.HasOne(d => d.IdUslugaNavigation)
                    .WithMany(p => p.WizytaUslugas)
                    .HasForeignKey(d => d.IdUsluga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_usluga_Usluga");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.WizytaUslugas)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_usluga_Wizyta");
            });

            modelBuilder.Entity<Wizytum>(entity =>
            {
                entity.HasKey(e => e.IdWizyta)
                    .HasName("Wizyta_pk");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.Cena).HasColumnType("money");

                entity.Property(e => e.CenaZnizka)
                    .HasColumnType("money")
                    .HasColumnName("Cena_znizka");

                entity.Property(e => e.CzyOplacona).HasColumnName("Czy_oplacona");

                entity.Property(e => e.IdHarmonogram).HasColumnName("ID_harmonogram");

                entity.Property(e => e.IdZnizka).HasColumnName("ID_znizka");

                entity.Property(e => e.NotatkaKlient)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Notatka_klient");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdHarmonogramNavigation)
                    .WithMany(p => p.Wizyta)
                    .HasForeignKey(d => d.IdHarmonogram)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_Harmonogram");

                entity.HasOne(d => d.IdZnizkaNavigation)
                    .WithMany(p => p.Wizyta)
                    .HasForeignKey(d => d.IdZnizka)
                    .HasConstraintName("Wizyta_Znizka");
            });

            modelBuilder.Entity<Zdjecie>(entity =>
            {
                entity.HasKey(e => e.IdZdjecie)
                    .HasName("Zdjecie_pk");

                entity.ToTable("Zdjecie");

                entity.Property(e => e.IdZdjecie)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_zdjecie");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.Property(e => e.Zdjecie1)
                    .IsRequired()
                    .HasColumnType("image")
                    .HasColumnName("Zdjecie");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.Zdjecies)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zdjecie_Wizyta");
            });

            modelBuilder.Entity<Znizka>(entity =>
            {
                entity.HasKey(e => e.IdZnizka)
                    .HasName("Znizka_pk");

                entity.ToTable("Znizka");

                entity.Property(e => e.IdZnizka).HasColumnName("ID_znizka");

                entity.Property(e => e.DoKiedy)
                    .HasColumnType("date")
                    .HasColumnName("Do_kiedy");

                entity.Property(e => e.NazwaZnizki)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Nazwa_znizki");

                entity.Property(e => e.ProcentZnizki).HasColumnName("Procent_znizki");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PRO_API.Models
{
    public partial class KlinikaContext : DbContext
    {
        public KlinikaContext()
        {
        }

        public KlinikaContext(DbContextOptions<KlinikaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Badanie> Badanies { get; set; }
        public virtual DbSet<Klient> Klients { get; set; }
        public virtual DbSet<KlientZnizka> KlientZnizkas { get; set; }
        public virtual DbSet<Lek> Leks { get; set; }
        public virtual DbSet<LekWMagazynie> LekWMagazynies { get; set; }
        public virtual DbSet<LekWizytum> LekWizyta { get; set; }
        public virtual DbSet<Osoba> Osobas { get; set; }
        public virtual DbSet<Pacjent> Pacjents { get; set; }
        public virtual DbSet<ReceptaLek> ReceptaLeks { get; set; }
        public virtual DbSet<Receptum> Recepta { get; set; }
        public virtual DbSet<Skierowanie> Skierowanies { get; set; }
        public virtual DbSet<Specjalizacja> Specjalizacjas { get; set; }
        public virtual DbSet<Usluga> Uslugas { get; set; }
        public virtual DbSet<Weterynarz> Weterynarzs { get; set; }
        public virtual DbSet<WeterynarzSpecjalizacja> WeterynarzSpecjalizacjas { get; set; }
        public virtual DbSet<WizytaUsluga> WizytaUslugas { get; set; }
        public virtual DbSet<Wizytum> Wizyta { get; set; }
        public virtual DbSet<Zabieg> Zabiegs { get; set; }
        public virtual DbSet<Znizka> Znizkas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Badanie>(entity =>
            {
                entity.HasKey(e => e.IdUsluga)
                    .HasName("Badanie_pk");

                entity.ToTable("Badanie");

                entity.Property(e => e.IdUsluga)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_usluga");

                entity.Property(e => e.Dolegliwosc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUslugaNavigation)
                    .WithOne(p => p.Badanie)
                    .HasForeignKey<Badanie>(d => d.IdUsluga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Badanie_Usluga");
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

            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.HasKey(e => e.IdOsoba)
                    .HasName("Osoba_pk");

                entity.ToTable("Osoba");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.DataUrodzenia)
                    .HasColumnType("date")
                    .HasColumnName("Data_urodzenia");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Haslo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Imie)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nazwisko)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumerTelefonu)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Numer_telefonu");
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
                entity.HasKey(e => new { e.IdUsluga, e.IdWizyta })
                    .HasName("Skierowanie_pk");

                entity.ToTable("Skierowanie");

                entity.Property(e => e.IdUsluga).HasColumnName("ID_usluga");

                entity.Property(e => e.IdWizyta).HasColumnName("ID_wizyta");

                entity.HasOne(d => d.IdUslugaNavigation)
                    .WithMany(p => p.Skierowanies)
                    .HasForeignKey(d => d.IdUsluga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skierowanie_Usluga");

                entity.HasOne(d => d.IdWizytaNavigation)
                    .WithMany(p => p.Skierowanies)
                    .HasForeignKey(d => d.IdWizyta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Skierowanie_Wizyta");
            });

            modelBuilder.Entity<Specjalizacja>(entity =>
            {
                entity.HasKey(e => e.IdSpecjalizacja)
                    .HasName("Specjalizacja_pk");

                entity.ToTable("Specjalizacja");

                entity.Property(e => e.IdSpecjalizacja).HasColumnName("ID_specjalizacja");

                entity.Property(e => e.NazwaSpecjalizacji)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nazwa_specjalizacji");
            });

            modelBuilder.Entity<Usluga>(entity =>
            {
                entity.HasKey(e => e.IdUsluga)
                    .HasName("Usluga_pk");

                entity.ToTable("Usluga");

                entity.Property(e => e.IdUsluga).HasColumnName("ID_usluga");

                entity.Property(e => e.Cena).HasColumnType("money");

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

                entity.Property(e => e.DataGodzina)
                    .HasColumnType("datetime")
                    .HasColumnName("Data_godzina");

                entity.Property(e => e.IdOsoba).HasColumnName("ID_osoba");

                entity.Property(e => e.IdPacjent).HasColumnName("ID_pacjent");

                entity.Property(e => e.Koszt).HasColumnType("money");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOsobaNavigation)
                    .WithMany(p => p.Wizyta)
                    .HasForeignKey(d => d.IdOsoba)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_Weterynarz");

                entity.HasOne(d => d.IdPacjentNavigation)
                    .WithMany(p => p.Wizyta)
                    .HasForeignKey(d => d.IdPacjent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Wizyta_Pacjent");
            });

            modelBuilder.Entity<Zabieg>(entity =>
            {
                entity.HasKey(e => e.IdUsluga)
                    .HasName("Zabieg_pk");

                entity.ToTable("Zabieg");

                entity.Property(e => e.IdUsluga)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_usluga");

                entity.HasOne(d => d.IdUslugaNavigation)
                    .WithOne(p => p.Zabieg)
                    .HasForeignKey<Zabieg>(d => d.IdUsluga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Zabieg_Usluga");
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

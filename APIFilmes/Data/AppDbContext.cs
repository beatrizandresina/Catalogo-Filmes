using APIFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace APIFilmes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FilmeModel> Filmes { get; set; }
        public DbSet<AvaliacaoModel> Avaliacoes { get; set; }
        public DbSet<GeneroModel> Generos { get; set; }
        public DbSet<ProfissionalModel> Profissionais { get; set; }
        public DbSet<FilmeProfissionalModel> FilmeProfissionais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmeModel>()
                .HasMany(f => f.Avaliacoes)
                .WithOne(a => a.Filme)
                .HasForeignKey(a => a.FilmeId);

            modelBuilder.Entity<FilmeModel>()
                .HasMany(f => f.Generos)
                .WithMany(g => g.Filmes)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmeGenero",
                    j => j.HasOne<GeneroModel>().WithMany().HasForeignKey("GeneroId"),
                    j => j.HasOne<FilmeModel>().WithMany().HasForeignKey("FilmeId"));

            modelBuilder.Entity<FilmeProfissionalModel>()
                .HasKey(fp => new { fp.FilmeId, fp.ProfissionalId });

            modelBuilder.Entity<FilmeProfissionalModel>()
                .HasOne(fp => fp.Filme)
                .WithMany(f => f.Profissionais)
                .HasForeignKey(fp => fp.FilmeId);

            modelBuilder.Entity<FilmeProfissionalModel>()
                .HasOne(fp => fp.Profissional)
                .WithMany(p => p.Filmes)
                .HasForeignKey(fp => fp.ProfissionalId);
        }
    }
}

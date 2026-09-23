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
        }
    }
}

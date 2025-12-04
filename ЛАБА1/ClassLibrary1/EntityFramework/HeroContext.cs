using System.Collections.Generic;
using System.Data.Entity;

namespace DataAccessLayer.EntityFramework
{
    public class HeroContext : DbContext
    {
        public HeroContext() : base("name=HeroDatabase") { }

        public DbSet<Model.Domain.Hero> Heroes { get; set; }
        public DbSet<Model.Domain.Species> Species { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка таблицы Heroes
            modelBuilder.Entity<Model.Domain.Hero>().ToTable("Heroes");
            modelBuilder.Entity<Model.Domain.Hero>().HasKey(h => h.Id);
            modelBuilder.Entity<Model.Domain.Hero>().Property(h => h.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Model.Domain.Hero>().Property(h => h.Genre).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Model.Domain.Hero>().Property(h => h.TypeOfDamage).IsRequired().HasMaxLength(50);

            // Настройка таблицы Species
            modelBuilder.Entity<Model.Domain.Species>().ToTable("Species");
            modelBuilder.Entity<Model.Domain.Species>().HasKey(s => s.Id);
            modelBuilder.Entity<Model.Domain.Species>().Property(s => s.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Model.Domain.Species>().Property(s => s.Description).HasMaxLength(200);

            modelBuilder.Entity<Model.Domain.Hero>().HasRequired(h => h.Species)
                .WithMany(s => s.Heroes)
                .HasForeignKey(h => h.SpeciesId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
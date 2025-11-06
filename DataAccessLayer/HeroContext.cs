using System.Data.Entity;
using ЛАБА1;

namespace DataAccessLayer.EntityFramework
{
    public class HeroContext : DbContext
    {
        public HeroContext() : base("name=HeroDatabase") { }

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Species> Species { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Настройка таблицы Heroes
            modelBuilder.Entity<Hero>().ToTable("Heroes");
            modelBuilder.Entity<Hero>().HasKey(h => h.Id);
            modelBuilder.Entity<Hero>().Property(h => h.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Hero>().Property(h => h.Genre).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Hero>().Property(h => h.TypeOfDamage).IsRequired().HasMaxLength(50);

            // Настройка таблицы Species
            modelBuilder.Entity<Species>().ToTable("Species");
            modelBuilder.Entity<Species>().HasKey(s => s.Id);
            modelBuilder.Entity<Species>().Property(s => s.Name).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Species>().Property(s => s.Description).HasMaxLength(200);
            modelBuilder.Entity<Hero>().HasRequired(h => h.Species).WithMany(s => s.Heroes).HasForeignKey(h => h.SpeciesId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
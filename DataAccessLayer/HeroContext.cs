using System.Data.Entity;
using ЛАБА1;

namespace DataAccessLayer.EntityFramework
{
    public class HeroContext : DbContext//класс для работы с бдешками
    {
        public HeroContext() : base("name=HeroDatabase") { }//конструктор чтобы подкчить бд
        public DbSet<Hero> Heroes { get; set; }//создание таблички в базе данных
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {//настройки чтобы мапнуть класс на таблицу
            modelBuilder.Entity<Hero>().ToTable("Heroes");
            modelBuilder.Entity<Hero>().HasKey(h => h.Id);
            modelBuilder.Entity<Hero>().Property(h => h.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Hero>().Property(h => h.Species).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Hero>().Property(h => h.Genre).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Hero>().Property(h => h.TypeOfDamage).IsRequired().HasMaxLength(50);
            base.OnModelCreating(modelBuilder);
        }
    }
}
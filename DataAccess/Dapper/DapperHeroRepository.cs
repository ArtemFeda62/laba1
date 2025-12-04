using Domain.Models;
using Domain.Repositories;
using System.Data;
namespace DataAccess.Dapper
{
    public class DapperHeroRepository : IHeroRepository
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\ЛАБА1\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public void Add(Hero entity)
        {
            var sql = @"INSERT INTO Heroes (Name, SpeciesId, Genre, Strange, Hp, TypeOfDamage) 
                       VALUES (@Name, @SpeciesId, @Genre, @Strange, @Hp, @TypeOfDamage)";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public IEnumerable<Hero> GetAll()
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    splitOn: "SpeciesId");
            }
        }

        public (IEnumerable<Hero> heroes, int totalCount) GetAllWithPagination(int pageNumber, int pageSize)
        {
            var sql = @"
                SELECT 
                    h.*, 
                    s.Id as SpeciesId, 
                    s.Name as SpeciesName, 
                    s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                ORDER BY h.Id
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*) FROM Heroes;";

            using (var connection = CreateConnection())
            {
                using (var multi = connection.QueryMultiple(sql, new
                {
                    Offset = (pageNumber - 1) * pageSize,
                    PageSize = pageSize
                }))
                {
                    var heroes = multi.Read<Hero, Species, Hero>(
                        (hero, species) =>
                        {
                            hero.Species = species;
                            hero.SpeciesId = species.Id;
                            return hero;
                        },
                        splitOn: "SpeciesId"
                    ).ToList();

                    var totalCount = multi.ReadSingle<int>();

                    return (heroes, totalCount);
                }
            }
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Heroes WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public Hero GetById(int id)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id 
                WHERE h.Id = @Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { Id = id },
                    splitOn: "SpeciesId").FirstOrDefault();
            }
        }

        public void Update(Hero entity)
        {
            var sql = @"UPDATE Heroes SET Name = @Name, SpeciesId = @SpeciesId, Genre = @Genre, 
                       Strange = @Strange, Hp = @Hp, TypeOfDamage = @TypeOfDamage 
                       WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }
        public IEnumerable<Hero> GetBySpeciesId(int speciesId)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                WHERE h.SpeciesId = @SpeciesId";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { SpeciesId = speciesId },
                    splitOn: "SpeciesId");
            }
        }
        public IEnumerable<Hero> FindByName(string name)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                WHERE h.Name LIKE @NamePattern";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { NamePattern = $"%{name}%" },
                    splitOn: "SpeciesId");
            }
        }
        public IEnumerable<Hero> GetWithLowHp(double maxHp)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                WHERE h.Hp <= @MaxHp
                ORDER BY h.Hp";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { MaxHp = maxHp },
                    splitOn: "SpeciesId");
            }
        }
        public IEnumerable<Hero> GetStrongest(int count)
        {
            var sql = @"
                SELECT TOP (@Count) h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                ORDER BY h.Strange DESC";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { Count = count },
                    splitOn: "SpeciesId");
            }
        }
        public Dictionary<string, List<Hero>> GroupBySpecies()
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                ORDER BY s.Name";

            using (var connection = CreateConnection())
            {
                var heroes = connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    splitOn: "SpeciesId").ToList();

                return heroes
                    .GroupBy(h => h.Species.Name)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }
        }
        public Dictionary<string, List<Hero>> GroupByDamageType()
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
                ORDER BY h.TypeOfDamage";

            using (var connection = CreateConnection())
            {
                var heroes = connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    splitOn: "SpeciesId").ToList();

                return heroes
                    .GroupBy(h => h.TypeOfDamage)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }
        }

    }
}

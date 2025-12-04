using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DataAccessLayer.Dapper
{
    public class DapperSpeciesRepository : ISpeciesRepository
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\Model.Domain\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public void Add(Model.Domain.Species entity)
        {
            var sql = @"INSERT INTO Species (Name, Description) 
                       VALUES (@Name, @Description)";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Species WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public IEnumerable<Model.Domain.Species> ReadAll()
        {
            var sql = "SELECT * FROM Species";
            using (var connection = CreateConnection())
            {
                return connection.Query<Model.Domain.Species>(sql);
            }
        }

        public Model.Domain.Species ReadById(int id)
        {
            var sql = "SELECT * FROM Species WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<Model.Domain.Species>(sql, new { Id = id });
            }
        }

        public void Update(Model.Domain.Species entity)
        {
            var sql = @"UPDATE Species SET Name = @Name, Description = @Description 
                       WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public IEnumerable<Model.Domain.Species> GetAllOrderedByName()
        {
            var sql = "SELECT * FROM Species ORDER BY Name";
            using (var connection = CreateConnection())
            {
                return connection.Query<Model.Domain.Species>(sql);
            }
        }
    }
}
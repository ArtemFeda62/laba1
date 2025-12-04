using Domain;
using Domain.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Domain.Models;

namespace DataAccess.Dapper
{
    public class DapperSpeciesRepository : ISpeciesRepository
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\ЛАБА1\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public void Add(Species entity)
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

        public IEnumerable<Species> ReadAll()
        {
            var sql = "SELECT * FROM Species";
            using (var connection = CreateConnection())
            {
                return connection.Query<Species>(sql);
            }
        }

        public Species ReadById(int id)
        {
            var sql = "SELECT * FROM Species WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<Species>(sql, new { Id = id });
            }
        }

        public void Update(Species entity)
        {
            var sql = @"UPDATE Species SET Name = @Name, Description = @Description 
                       WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public IEnumerable<Species> GetAllOrderedByName()
        {
            var sql = "SELECT * FROM Species ORDER BY Name";
            using (var connection = CreateConnection())
            {
                return connection.Query<Species>(sql);
            }
        }
    }
}

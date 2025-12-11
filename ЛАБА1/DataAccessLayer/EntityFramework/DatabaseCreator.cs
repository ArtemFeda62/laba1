using System.Data.SqlClient;

public class DatabaseCreator
{
    private static string _masterConnectionString =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30";

    private static string _heroDbConnectionString =
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HeroDatabase;Integrated Security=True;Connect Timeout=30";

    public static void InitializeDatabase()
    {
        try
        {
            Console.WriteLine("=== Инициализация базы данных ===");

            // Шаг 1: Проверяем и создаем базу данных если нужно
            CreateDatabaseIfNotExists();

            // Шаг 2: Создаем таблицы если нужно
            CreateTablesIfNotExists();

            // Шаг 3: Добавляем тестовые данные если нужно
            SeedTestDataIfNeeded();

            Console.WriteLine("=== База данных готова к использованию ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            throw;
        }
    }

    private static void CreateDatabaseIfNotExists()
    {
        try
        {
            using (var connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();

                // Просто создаем базу данных без указания файлов
                var sql = @"
                IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'HeroDatabase')
                BEGIN
                    CREATE DATABASE HeroDatabase;
                    PRINT 'База данных HeroDatabase создана';
                END
                ELSE
                BEGIN
                    PRINT 'База данных HeroDatabase уже существует';
                END";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании базы данных: {ex.Message}");
            throw;
        }
    }

    private static void CreateTablesIfNotExists()
    {
        try
        {
            using (var connection = new SqlConnection(_heroDbConnectionString))
            {
                connection.Open();

                // Таблица Species
                var createSpeciesTable = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Species')
                BEGIN
                    CREATE TABLE Species (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(200)
                    );
                    PRINT 'Таблица Species создана';
                END
                ELSE
                BEGIN
                    PRINT 'Таблица Species уже существует';
                END";

                // Таблица Heroes
                var createHeroesTable = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Heroes')
                BEGIN
                    CREATE TABLE Heroes (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        SpeciesId INT NOT NULL,
                        Genre NVARCHAR(20) NOT NULL,
                        Strange INT NOT NULL,
                        Hp FLOAT NOT NULL,
                        TypeOfDamage NVARCHAR(50) NOT NULL,
                        FOREIGN KEY (SpeciesId) REFERENCES Species(Id)
                    );
                    PRINT 'Таблица Heroes создана';
                END
                ELSE
                BEGIN
                    PRINT 'Таблица Heroes уже существует';
                END";

                ExecuteNonQuery(connection, createSpeciesTable);
                ExecuteNonQuery(connection, createHeroesTable);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании таблиц: {ex.Message}");
            throw;
        }
    }

    private static void SeedTestDataIfNeeded()
    {
        try
        {
            using (var connection = new SqlConnection(_heroDbConnectionString))
            {
                connection.Open();

                // Проверяем, есть ли данные в таблице Species
                var checkSpeciesCount = "SELECT COUNT(*) FROM Species";
                var speciesCount = ExecuteScalar<int>(connection, checkSpeciesCount);

                if (speciesCount == 0)
                {
                    Console.WriteLine("Добавление тестовых данных в таблицу Species...");

                    var insertSpecies = @"
                    INSERT INTO Species (Name, Description) VALUES
                    ('Человек', 'Стандартная человеческая раса'),
                    ('Эльф', 'Древняя раса с острым слухом'),
                    ('Гном', 'Низкорослая, но сильная раса'),
                    ('Орк', 'Воинственная раса'),
                    ('Драконорожденный', 'Потомки древних драконов')";

                    ExecuteNonQuery(connection, insertSpecies);
                    Console.WriteLine("Добавлено 5 видов рас");
                }

                // Проверяем, есть ли данные в таблице Heroes
                var checkHeroesCount = "SELECT COUNT(*) FROM Heroes";
                var heroesCount = ExecuteScalar<int>(connection, checkHeroesCount);

                if (heroesCount == 0)
                {
                    Console.WriteLine("Добавление тестовых данных в таблицу Heroes...");

                    var insertHeroes = @"
                    INSERT INTO Heroes (Name, SpeciesId, Genre, Strange, Hp, TypeOfDamage) VALUES
                    ('Артур', 1, 'Мужской', 85, 100.0, 'Меч'),
                    ('Леголас', 2, 'Мужской', 70, 80.0, 'Лук'),
                    ('Гимли', 3, 'Мужской', 95, 120.0, 'Топор'),
                    ('Гвен', 1, 'Женский', 60, 70.0, 'Кинжалы'),
                    ('Довакин', 5, 'Мужской', 100, 90.0, 'Магия')";

                    ExecuteNonQuery(connection, insertHeroes);
                    Console.WriteLine("Добавлено 5 героев");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении тестовых данных: {ex.Message}");
            // Не прерываем выполнение, так как это не критическая ошибка
        }
    }

    private static void ExecuteNonQuery(SqlConnection connection, string sql)
    {
        using (var command = new SqlCommand(sql, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    private static T ExecuteScalar<T>(SqlConnection connection, string sql)
    {
        using (var command = new SqlCommand(sql, connection))
        {
            var result = command.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
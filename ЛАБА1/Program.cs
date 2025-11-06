using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ЛАБА1;

namespace ЛАБА1
{
    internal static class Program
    {
        private static Form1 _mainForm;
        private static ConsoleInterface _consoleInterface;
        private static Thread _formThread;
        private static Thread _consoleThread;
        private static bool _isRunning = true;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitializeDatabase();
            StartBothInterfaces();

            while (_isRunning)
            {
                Thread.Sleep(100);
                if ((_formThread == null || !_formThread.IsAlive) &&
                    (_consoleThread == null || !_consoleThread.IsAlive))
                {
                    _isRunning = false;
                }
            }
            Console.WriteLine("Приложение завершено.");
        }

        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        private static void InitializeDatabase()
        {
            try
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataAccessLayer.EntityFramework.HeroContext>());
                using (var context = new DataAccessLayer.EntityFramework.HeroContext())
                {
                    context.Database.Initialize(force: true);
                    Console.WriteLine("База данных инициализирована");
                    InitializeSpecies(context);
                    InitializeHeroes(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
                Console.WriteLine($"Детали: {ex.InnerException?.Message}");
                RecreateDatabase();
            }
        }

        /// <summary>
        /// Пересоздание базы данных
        /// </summary>
        private static void RecreateDatabase()
        {
            try
            {
                Console.WriteLine("Попытка пересоздания базы данных...");
                using (var context = new DataAccessLayer.EntityFramework.HeroContext())
                {
                    if (context.Database.Exists())
                    {
                        context.Database.Delete();
                        Console.WriteLine("Старая база данных удалена");
                    }

                    context.Database.Create();
                    Console.WriteLine("Новая база данных создана");

                    InitializeSpecies(context);
                    InitializeHeroes(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при пересоздании базы: {ex.Message}");
            }
        }

        /// <summary>
        /// Инициализация таблицы Species
        /// </summary>
        private static void InitializeSpecies(DataAccessLayer.EntityFramework.HeroContext context)
        {
            if (!context.Species.Any())
            {
                var species = new[]
                {
                    new Species { Name = "Человек", Description = "Универсальная раса с сбалансированными характеристиками" },
                    new Species { Name = "Эльф", Description = "Изящная раса с повышенной ловкостью и интеллектом" },
                    new Species { Name = "Гном", Description = "Выносливая раса с высокой силой и стойкостью" },
                    new Species { Name = "Орк", Description = "Сильная и агрессивная раса" },
                    new Species { Name = "Дварф", Description = "Мастерские навыки в ремеслах и бою" },
                    new Species { Name = "Гоблин", Description = "Малая, но хитрая раса" }
                };

                context.Species.AddRange(species);
                context.SaveChanges();
                Console.WriteLine($"Таблица Species инициализирована, добавлено {species.Length} рас");
            }
            else
            {
                Console.WriteLine($"Таблица Species уже содержит {context.Species.Count()} рас");
            }
        }

        /// <summary>
        /// Инициализация тестовых героев
        /// </summary>
        private static void InitializeHeroes(DataAccessLayer.EntityFramework.HeroContext context)
        {
            if (!context.Heroes.Any())
            {
                var species = context.Species.ToList();
                Console.WriteLine($"Найдено рас в базе: {species.Count}");

                // Находим ID для каждой расы
                var goblin = species.FirstOrDefault(s => s.Name == "Гоблин");
                var elf = species.FirstOrDefault(s => s.Name == "Эльф");
                var orc = species.FirstOrDefault(s => s.Name == "Орк");

                if (goblin == null || elf == null || orc == null)
                {
                    Console.WriteLine("Ошибка: не все расы найдены в базе");
                    return;
                }

                // Добавляем тестовых героев напрямую через контекст
                var heroes = new[]
                {
                    new Hero { Name = "Гоблин Гоша", SpeciesId = goblin.Id, Genre = "Транс", Strange = 20, TypeOfDamage = "Физический урон", Hp = 500 },
                    new Hero { Name = "Блум", SpeciesId = elf.Id, Genre = "Женский", Strange = 100, TypeOfDamage = "Магический урон", Hp = 100 },
                    new Hero { Name = "Орк Генадий", SpeciesId = orc.Id, Genre = "Мужской", Strange = 50, TypeOfDamage = "Кидается какашками", Hp = 250 }
                };

                context.Heroes.AddRange(heroes);
                context.SaveChanges();
                Console.WriteLine($"Добавлено {heroes.Length} тестовых героев");
            }
            else
            {
                Console.WriteLine($"База данных уже содержит {context.Heroes.Count()} героев");
            }
        }

        // Остальные методы без изменений...
        public static void StartBothInterfaces()
        {
            _formThread = new Thread(() =>
            {
                try
                {
                    _mainForm = new Form1();
                    _mainForm.FormClosed += (s, args) =>
                    {
                        Application.ExitThread();
                        _isRunning = false;
                    };
                    Application.Run(_mainForm);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в форме: {ex.Message}");
                }
            });
            _formThread.SetApartmentState(ApartmentState.STA);
            _formThread.IsBackground = true;
            _formThread.Start();

            _consoleThread = new Thread(() =>
            {
                try
                {
                    _consoleInterface = new ConsoleInterface();
                    _consoleInterface.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в консоли: {ex.Message}");
                }
            });
            _consoleThread.IsBackground = true;
            _consoleThread.Start();
        }

        public static void RefreshFormData()
        {
            if (_mainForm != null && !_mainForm.IsDisposed && _mainForm.IsHandleCreated)
            {
                try
                {
                    _mainForm.Invoke(new Action(() =>
                    {
                        if (!_mainForm.IsDisposed)
                            _mainForm.RefreshHeroesList();
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обновления формы: {ex.Message}");
                }
            }
        }

        public static void StopApplication()
        {
            _isRunning = false;

            try
            {
                _formThread?.Abort();
                _consoleThread?.Abort();
            }
            catch
            {
            }
        }

        public static Logic GetLogic()
        {
            return new Logic();
        }

        public static System.Collections.Generic.List<Species> GetSpecies()
        {
            using (var context = new DataAccessLayer.EntityFramework.HeroContext())
            {
                return context.Species.OrderBy(s => s.Name).ToList();
            }
        }
    }
}
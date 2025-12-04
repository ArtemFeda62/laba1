using Ninject;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using View.Forms;

namespace View
{
    internal static class Program
    {
        private static Form1 _mainForm;
        private static ConsoleInterface.ConsoleInterface _consoleInterface;
        private static Thread _formThread;
        private static Thread _consoleThread;
        private static bool _isRunning = true;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitializeDatabase();
            StartBothInterfaces();

            // Ждём завершения обоих потоков
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

        private static void InitializeDatabase()
        {
            try
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataAccess.EntityFramework.HeroContext>());
                using (var context = new DataAccess.EntityFramework.HeroContext())
                {
                    context.Database.Initialize(force: true);
                    Console.WriteLine("База данных инициализирована");

                    // Инициализируем тестовые данные
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
                    }

                    if (!context.Heroes.Any())
                    {
                        var species = context.Species.ToList();
                        var goblin = species.FirstOrDefault(s => s.Name == "Гоблин");
                        var elf = species.FirstOrDefault(s => s.Name == "Эльф");
                        var orc = species.FirstOrDefault(s => s.Name == "Орк");

                        if (goblin != null && elf != null && orc != null)
                        {
                            var heroes = new[]
                            {
                                new Hero { Name = "Гоблин Гоша", SpeciesId = goblin.Id, Genre = "Транс", Strange = 20, TypeOfDamage = "Физический урон", Hp = 500 },
                                new Hero { Name = "Блум", SpeciesId = elf.Id, Genre = "Женский", Strange = 100, TypeOfDamage = "Магический урон", Hp = 100 },
                                new Hero { Name = "Орк Генадий", SpeciesId = orc.Id, Genre = "Мужской", Strange = 50, TypeOfDamage = "Кидается какашками", Hp = 250 }
                            };

                            context.Heroes.AddRange(heroes);
                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
            }
        }

        public static void StartBothInterfaces()
        {
            // Запуск GUI в отдельном потоке
            _formThread = new Thread(() =>
            {
                try
                {
                    var kernel = new StandardKernel(new AppModule());
                    _mainForm = kernel.Get<Form1>();
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

            // Запуск консольного интерфейса в отдельном потоке
            _consoleThread = new Thread(() =>
            {
                try
                {
                    var kernel = new StandardKernel(new AppModule());
                    _consoleInterface = kernel.Get<ConsoleInterface.ConsoleInterface>();
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
                _consoleInterface?.Stop();
                _formThread?.Abort();
                _consoleThread?.Abort();
            }
            catch
            {
            }
        }
    }
}
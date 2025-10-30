using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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

            // Инициализация базы данных
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
                using (var context = new DataAccessLayer.EntityFramework.HeroContext())
                {
                    // Создаем базу данных, если она не существует
                    context.Database.CreateIfNotExists();

                    // Проверяем, есть ли данные в базе
                    if (!context.Heroes.Any())
                    {
                        // Добавляем тестовые данные
                        var logic = new Logic();
                        logic.CreateHero("Гоблин Гоша", "Транс", "Гоблин", 20, "Физический урон", 500);
                        logic.CreateHero("Блум", "ЖЕНЩИНА", "Фея Винкс", 100, "Магический урон", 100);
                        logic.CreateHero("Орк Генадий", "мужик", "Орк", 50, "Кидается какашками", 250);
                        Console.WriteLine("Тестовые данные добавлены в базу данных");
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
        }
    }
}
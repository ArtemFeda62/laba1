using System;
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

            StartBothInterfaces();

            // Ждем завершения работы
            while (_isRunning)
            {
                Thread.Sleep(100);

                // Проверяем, живы ли оба интерфейса
                if ((_formThread == null || !_formThread.IsAlive) &&
                    (_consoleThread == null || !_consoleThread.IsAlive))
                {
                    _isRunning = false;
                }
            }

            Console.WriteLine("Приложение завершено.");
        }

        public static void StartBothInterfaces()
        {
            // Запускаем форму в отдельном потоке
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

            // Запускаем консоль в отдельном потоке
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
            // Обновляем данные в форме из любого потока
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
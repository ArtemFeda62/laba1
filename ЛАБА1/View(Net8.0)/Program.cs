using System;
using System.Threading;
using System.Windows.Forms;
using DataAccessLayer.EntityFramework;

namespace View
{
    internal static class Program
    {
        private static MainForm _mainForm;
        private static ConsoleInterface _consoleInterface;
        private static Thread _formThread;
        private static Thread _consoleThread;
        private static bool _isRunning = true;

        [STAThread]
        static void Main(string[] args)
        {
            DatabaseCreator.InitializeDatabase();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
        }

        public static void StartBothInterfaces()
        {
            _formThread = new Thread(() =>
            {
                try
                {
                    _mainForm = new MainForm();
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

            Thread.Sleep(1000); // Даем форме время инициализироваться

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
    }
}
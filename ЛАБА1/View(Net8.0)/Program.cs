using System;
using System.Threading;
using System.Windows.Forms;

namespace View
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DatabaseCreator.InitializeDatabase();
            Thread formThread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            });
            formThread.SetApartmentState(ApartmentState.STA);
            formThread.Start();
            Thread.Sleep(1000);

            ConsoleInterface consoleInterface = new ConsoleInterface();
            consoleInterface.Run();
            if (formThread.IsAlive)
            {
                formThread.Interrupt();
            }
        }
    }
}
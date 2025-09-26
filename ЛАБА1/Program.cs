using System;
using System.Windows.Forms;

namespace ЛАБА1
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool running = true;

                while (running)
                {
                    Console.Clear();
                    Console.WriteLine("1) Консольное приложение");
                    Console.WriteLine("2) Виндоус форма");
                    Console.WriteLine("3) Сбросить данные к исходным");
                    Console.WriteLine("4) Выход");
                    Console.Write("Выберите опцию: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            RunConsoleApp();
                            break;
                        case "2":
                            RunWindowsForm();
                            break;
                        case "3":
                            SharedData.ResetData();
                            Console.WriteLine("Данные сброшены к исходным!");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case "4":
                            running = false;
                            Console.WriteLine("Выход из программы...");
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        static void RunConsoleApp()
        {
            var consoleApp = new ConsoleInterface();
            consoleApp.Run();
        }

        static void RunWindowsForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаем форму и запускаем ее
            using (var form = new Form1())
            {
                Application.Run(form);
            }
        }
    }
}
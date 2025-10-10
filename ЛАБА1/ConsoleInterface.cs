using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class ConsoleInterface
    {
        private Logic logic;
        private bool _isRunning;

        /// <summary>
        /// Инициализирует новый экземпляр консольного интерфейса
        /// </summary>
        public ConsoleInterface()
        {
            logic = new Logic();
            _isRunning = false;

            SetupFileWatcher();
        }

        /// <summary>
        /// Настраивает отслеживание изменений CSV файла с данными героев
        /// </summary>
        private void SetupFileWatcher()
        {
            var watcher = new FileSystemWatcher
            {
                Path = Directory.GetCurrentDirectory(),Filter = "heroes_data.csv",NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
            };
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Обрабатывает события изменения CSV файла с данными
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Данные о изменении файла</param>
        private void OnDataFileChanged(object sender, FileSystemEventArgs e)
        {
            SharedData.ReloadFromFile();
            Program.RefreshFormData();
        }

        /// <summary>
        /// Запускает главный цикл обработки команд консольного интерфейса
        /// </summary>
        public void Run()
        {
            _isRunning = true;

            while (_isRunning)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("1. Показать всех героев");
                Console.WriteLine("2. Добавить героя");
                Console.WriteLine("3. Найти героя по имени");
                Console.WriteLine("4. Группировка по расам");
                Console.WriteLine("5. Группировка по типу урона");
                Console.WriteLine("6. Раненые герои (HP < 50)");
                Console.WriteLine("7. Топ-3 самых сильных героя");
                Console.WriteLine("8. Нанести урон герою");
                Console.WriteLine("9. Убить героя");
                Console.WriteLine("0. Выход из консоли");
                Console.WriteLine("═══════════════════════════════════════");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllHeroes(); break;
                    case "2": AddNewHero(); break;
                    case "3": FindByName(); break;
                    case "4": ShowBySpecies(); break;
                    case "5": ShowByDamageType(); break;
                    case "6": ShowWoundedHeroes(); break;
                    case "7": ShowStrongestHeroes(); break;
                    case "8": HitHero(); break;
                    case "9": KillHero(); break;
                    case "0":
                        _isRunning = false;
                        Console.WriteLine("Консольный интерфейс завершает работу...");
                        Thread.Sleep(1000);
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        WaitForContinue();
                        break;
                }
            }
        }

        /// <summary>
        /// Останавливает работу консольного интерфейса
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Отображает список всех героев в системе
        /// </summary>
        private void ShowAllHeroes()
        {
            Console.WriteLine("\nВсе герои:");
            var heroes = logic.GetListHeros();
            if (heroes.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                foreach (var hero in heroes)
                {
                    Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)");
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Добавляет нового героя через ввод данных в консоли
        /// </summary>
        private void AddNewHero()
        {
            try
            {
                Console.Write("Имя: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Имя пустое.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Раса: ");
                var species = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(species))
                {
                    Console.WriteLine("Нужна расса , допустим негр.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Гендер: ");
                var genre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    Console.WriteLine("Личность небинарная?.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Сила: ");
                if (!int.TryParse(Console.ReadLine(), out int strange) || strange < 0)
                {
                    Console.WriteLine("ОШибка.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Тип урона: ");
                var damageType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(damageType))
                {
                    Console.WriteLine("Ошибка.");
                    WaitForContinue();
                    return;
                }

                Console.Write("HP: ");
                if (!double.TryParse(Console.ReadLine(), out double hp) || hp <= 0)
                {
                    Console.WriteLine("Error.");
                    WaitForContinue();
                    return;
                }
                logic.CreateHero(name, genre, species, hp, damageType, strange);
                Console.WriteLine("Герой добавлен!");
                SharedData.SaveFile();
                Program.RefreshFormData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении героя: {ex.Message}");
            }
            WaitForContinue();
        }

        /// <summary>
        /// Выполняет поиск героев по имени (регистронезависимый)
        /// </summary>
        private void FindByName()
        {
            Console.Write("Введите имя: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя для поиска пустое.");
                WaitForContinue();
                return;
            }

            var heroes = logic.FindHeroesByName(name);

            if (heroes.Count == 0)
            {
                Console.WriteLine($"Героев с именем '{name}' не найдено.");
            }
            else
            {
                Console.WriteLine($"\nГерои с именем '{name}':");
                foreach (var hero in heroes)
                {
                    Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)");
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Отображает героев с группировкой по расовой принадлежности
        /// </summary>
        private void ShowBySpecies()
        {
            var heroesBySpecies = logic.GroupHeroesBySpecies();

            if (heroesBySpecies.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                foreach (var species in heroesBySpecies)
                {
                    Console.WriteLine($"\n--- {species.Key} ---");
                    foreach (var hero in species.Value)
                    {
                        Console.WriteLine($"  {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}");
                    }
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Отображает героев с группировкой по типу наносимого урона
        /// </summary>
        private void ShowByDamageType()
        {
            var heroesByDamage = logic.GroupHeroesByDamageType();

            if (heroesByDamage.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                foreach (var damageType in heroesByDamage)
                {
                    Console.WriteLine($"\n--- {damageType.Key} ---");
                    foreach (var hero in damageType.Value)
                    {
                        Console.WriteLine($"  {hero.Name} ({hero.Species}) - HP: {hero.Hp}");
                    }
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Отображает список героев с низким уровнем здоровья (менее 50 HP)
        /// </summary>
        private void ShowWoundedHeroes()
        {
            var wounded = logic.GetHeroesWithLowHp(50);

            if (wounded.Count == 0)
            {
                Console.WriteLine("Раненых героев не найдено.");
            }
            else
            {
                Console.WriteLine("\nРаненые герои (HP < 50):");
                foreach (var hero in wounded)
                {
                    Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Hp} HP");
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Отображает топ-3 героев с наибольшим показателем силы
        /// </summary>
        private void ShowStrongestHeroes()
        {
            var strongest = logic.GetStrongestHeroes(3);

            if (strongest.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("\nТоп-3 самых сильных героя:");
                foreach (var hero in strongest)
                {
                    Console.WriteLine($"{hero.Id}) {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}");
                }
            }
            WaitForContinue();
        }

        /// <summary>
        /// Наносит урон выбранному герою и обновляет его здоровье
        /// </summary>
        private void HitHero()
        {
            ShowAllHeroes();
            try
            {
                Console.Write("Введите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = logic.GetHero(id);
                if (hero == null)
                {
                    Console.WriteLine("Герой с таким ID не найден.");
                    WaitForContinue();
                    return;
                }

                Console.Write($"Урон (текущее HP: {hero.Hp}): ");
                if (!double.TryParse(Console.ReadLine(), out double damage) || damage <= 0)
                {
                    Console.WriteLine("Некорректное значение урона.");
                    WaitForContinue();
                    return;
                }
                logic.HitHero(id, damage);

                SharedData.SaveFile();

                var updatedHero = logic.GetHero(id);
                Program.RefreshFormData();
                if (updatedHero.Hp > 0)
                {
                    Console.WriteLine($"Урон нанесен! Новое HP: {updatedHero.Hp}");
                }
                else
                {
                    Console.WriteLine("Герой сдох! Вы нанесли смертельный урон");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при нанесении урона: {ex.Message}");
            }
            WaitForContinue();
        }

        /// <summary>
        /// Удаляет героя из системы по указанному идентификатору
        /// </summary>
        private void KillHero()
        {
            try
            {
                Console.Write("Введите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = logic.GetHero(id);
                if (hero == null)
                {
                    Console.WriteLine("Герой с таким ID не найден.");
                    WaitForContinue();
                    return;
                }

                Console.Write($"Вы уверены, что хотите удалить героя '{hero.Name}'? (y/n): ");
                var confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
                {
                    logic.KillHero(id);
                    SharedData.SaveFile();
                    Console.WriteLine("Герой удален!");
                    Program.RefreshFormData();
                }
                else
                {
                    Console.WriteLine("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении героя: {ex.Message}");
            }
            WaitForContinue();
        }

        /// <summary>
        /// Ожидает нажатия любой клавиши для продолжения работы
        /// </summary>
        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
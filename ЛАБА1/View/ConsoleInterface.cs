// В проекте View, файл ConsoleInterface.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Presenter;
using Shared;

namespace View
{
    public class ConsoleInterface : IView
    {
        private MainPresenter _presenter;
        private bool _isRunning;

        public ConsoleInterface()
        {
            _presenter = new MainPresenter(this);
            _isRunning = false;
        }

        // Реализация IView
        public void RefreshHeroesList()
        {
            // В консоли мы сами управляем отображением
        }

        public void UpdateStatusBar(string status)
        {
            Console.Title = status;
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            Console.WriteLine($"\n[{title}]: {message}");
        }

        public void ShowError(string error, string title = "Ошибка")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[{title}]: {error}");
            Console.ResetColor();
        }

        public void Run()
        {
            _isRunning = true;
            _presenter.Initialize();

            while (_isRunning)
            {
                ShowMainMenu();
                ProcessChoice(Console.ReadLine());
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("      КОНСОЛЬНОЕ ПРИЛОЖЕНИЕ ГЕРОЕВ");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine($"Всего героев: {_presenter.TotalHeroes}");
            Console.WriteLine();
            Console.WriteLine("1.  Показать всех героев");
            Console.WriteLine("2.  Добавить героя");
            Console.WriteLine("3.  Найти героя по имени");
            Console.WriteLine("4.  Группировка по расам");
            Console.WriteLine("5.  Группировка по типу урона");
            Console.WriteLine("6.  Раненые герои (HP < 50)");
            Console.WriteLine("7.  Топ-3 самых сильных героя");
            Console.WriteLine("8.  Нанести урон герою");
            Console.WriteLine("9.  Убить героя");
            Console.WriteLine("10. Показать все расы");
            Console.WriteLine("11. Добавить расу");
            Console.WriteLine("12. Редактировать расу");
            Console.WriteLine("13. Удалить расу");
            Console.WriteLine("14. Статистика");
            Console.WriteLine("15. Обновить список");
            Console.WriteLine("0.  Выход");
            Console.WriteLine("═══════════════════════════════════════");
            Console.Write("Выберите действие: ");
        }

        private void ProcessChoice(string choice)
        {
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
                case "10": ShowAllSpecies(); break;
                case "11": AddNewSpecies(); break;
                case "12": EditSpecies(); break;
                case "13": DeleteSpecies(); break;
                case "14": ShowStatistics(); break;
                case "15": RefreshHeroes(); break;
                case "0":
                    _isRunning = false;
                    Console.WriteLine("Завершение работы консоли...");
                    Thread.Sleep(1000);
                    return;
                default:
                    ShowError("Неверный выбор. Попробуйте снова.");
                    WaitForContinue();
                    break;
            }
        }

        private void ShowAllHeroes()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("             ВСЕ ГЕРОИ");
            Console.WriteLine("═══════════════════════════════════════");

            var heroes = _presenter.GetAllHeroes();

            if (!heroes.Any())
            {
                ShowMessage("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5,-15} {6,-20}",
                    "ID", "Имя", "Раса", "HP", "Сила", "Гендер", "Тип урона");
                Console.WriteLine(new string('═', 100));

                foreach (var hero in heroes)
                {
                    var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                    Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5,-15} {6,-20}",
                        hero.Id,
                        hero.Name,
                        hero.Species?.Name ?? "Неизвестно",
                        $"{hpColor} {hero.Hp:F1}",
                        hero.Strange,
                        hero.Genre,
                        hero.TypeOfDamage);
                }
            }

            WaitForContinue();
        }

        private void AddNewHero()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("           ДОБАВЛЕНИЕ ГЕРОЯ");
                Console.WriteLine("═══════════════════════════════════════");

                // Показываем доступные расы
                ShowAllSpeciesBrief();

                Console.Write("\nИмя героя: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("ID расы: ");
                if (!int.TryParse(Console.ReadLine(), out int speciesId) || speciesId <= 0)
                {
                    ShowError("Некорректный ID расы.");
                    WaitForContinue();
                    return;
                }

                var species = _presenter.GetAllSpecies().FirstOrDefault(s => s.Id == speciesId);
                if (species == null)
                {
                    ShowError("Раса с таким ID не найдена.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Гендер: ");
                var genre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    ShowError("Гендер не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Сила (1-1000): ");
                if (!int.TryParse(Console.ReadLine(), out int strange) || strange < 1 || strange > 1000)
                {
                    ShowError("Некорректное значение силы.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Тип урона: ");
                var damageType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(damageType))
                {
                    ShowError("Тип урона не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("HP: ");
                if (!double.TryParse(Console.ReadLine(), out double hp) || hp <= 0)
                {
                    ShowError("Некорректное значение HP.");
                    WaitForContinue();
                    return;
                }

                _presenter.AddHero(name, speciesId, genre, strange, damageType, hp);
                ShowMessage("Герой успешно добавлен!", "Успех");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении героя: {ex.Message}");
            }

            WaitForContinue();
        }

        private void ShowAllSpeciesBrief()
        {
            var speciesList = _presenter.GetAllSpecies();
            if (speciesList.Any())
            {
                Console.WriteLine("\nДоступные расы:");
                Console.WriteLine("{0,-5} {1,-20} {2,-30}", "ID", "Название", "Описание");
                Console.WriteLine(new string('-', 60));

                foreach (var species in speciesList)
                {
                    var shortDescription = species.Description.Length > 30
                        ? species.Description.Substring(0, 27) + "..."
                        : species.Description;

                    Console.WriteLine("{0,-5} {1,-20} {2,-30}",
                        species.Id, species.Name, shortDescription);
                }
            }
        }

        private void ShowAllSpecies()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("               ВСЕ РАСЫ");
            Console.WriteLine("═══════════════════════════════════════");

            var speciesList = _presenter.GetAllSpecies();

            if (!speciesList.Any())
            {
                ShowMessage("Рас не найдено.");
            }
            else
            {
                foreach (var species in speciesList)
                {
                    Console.WriteLine($"\n🔹 {species.Name} (ID: {species.Id})");
                    Console.WriteLine($"   Описание: {species.Description}");

                    // Показываем количество героев этой расы
                    var heroesCount = _presenter.GetAllHeroes()
                        .Count(h => h.SpeciesId == species.Id);

                    Console.WriteLine($"   Героев этой расы: {heroesCount}");
                }
            }

            WaitForContinue();
        }

        private void AddNewSpecies()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("           ДОБАВЛЕНИЕ РАСЫ");
                Console.WriteLine("═══════════════════════════════════════");

                Console.Write("Название расы: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Название не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Описание: ");
                var description = Console.ReadLine();

                _presenter.AddSpecies(name, description);
                ShowMessage("Раса успешно добавлена!", "Успех");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении расы: {ex.Message}");
            }

            WaitForContinue();
        }

        private void EditSpecies()
        {
            try
            {
                var speciesList = _presenter.GetAllSpecies();
                if (!speciesList.Any())
                {
                    ShowMessage("Нет доступных рас для редактирования.");
                    WaitForContinue();
                    return;
                }

                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("         РЕДАКТИРОВАНИЕ РАСЫ");
                Console.WriteLine("═══════════════════════════════════════");

                ShowAllSpeciesBrief();

                Console.Write("\nID расы для редактирования: ");
                if (!int.TryParse(Console.ReadLine(), out int speciesId) || speciesId <= 0)
                {
                    ShowError("Некорректный ID расы.");
                    WaitForContinue();
                    return;
                }

                var species = speciesList.FirstOrDefault(s => s.Id == speciesId);
                if (species == null)
                {
                    ShowError("Раса с таким ID не найдена.");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine($"\nТекущие данные:");
                Console.WriteLine($"Название: {species.Name}");
                Console.WriteLine($"Описание: {species.Description}");
                Console.WriteLine(new string('-', 40));

                Console.Write("Новое название (оставьте пустым, чтобы не менять): ");
                var newName = Console.ReadLine();

                Console.Write("Новое описание (оставьте пустым, чтобы не менять): ");
                var newDescription = Console.ReadLine();

                var updatedSpecies = new Species
                {
                    Id = species.Id,
                    Name = string.IsNullOrWhiteSpace(newName) ? species.Name : newName.Trim(),
                    Description = string.IsNullOrWhiteSpace(newDescription) ? species.Description : newDescription.Trim()
                };

                _presenter.UpdateSpecies(updatedSpecies);
                ShowMessage("Раса успешно обновлена!", "Успех");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при редактировании расы: {ex.Message}");
            }

            WaitForContinue();
        }

        private void DeleteSpecies()
        {
            try
            {
                var speciesList = _presenter.GetAllSpecies();
                if (!speciesList.Any())
                {
                    ShowMessage("Нет доступных рас для удаления.");
                    WaitForContinue();
                    return;
                }

                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("            УДАЛЕНИЕ РАСЫ");
                Console.WriteLine("═══════════════════════════════════════");

                ShowAllSpeciesBrief();

                Console.Write("\nID расы для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int speciesId) || speciesId <= 0)
                {
                    ShowError("Некорректный ID расы.");
                    WaitForContinue();
                    return;
                }

                var species = speciesList.FirstOrDefault(s => s.Id == speciesId);
                if (species == null)
                {
                    ShowError("Раса с таким ID не найдена.");
                    WaitForContinue();
                    return;
                }

                // Проверяем, есть ли герои этой расы
                var heroesWithThisSpecies = _presenter.GetAllHeroes()
                    .Count(h => h.SpeciesId == speciesId);

                if (heroesWithThisSpecies > 0)
                {
                    ShowError($"Невозможно удалить расу '{species.Name}'. Существуют герои этой расы.");
                    WaitForContinue();
                    return;
                }

                Console.Write($"\nВы уверены, что хотите удалить расу '{species.Name}'? (да/нет): ");
                var confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
                {
                    _presenter.DeleteSpecies(speciesId);
                    ShowMessage("Раса успешно удалена!", "Успех");
                }
                else
                {
                    ShowMessage("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при удалении расы: {ex.Message}");
            }

            WaitForContinue();
        }

        private void FindByName()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("           ПОИСК ГЕРОЯ");
            Console.WriteLine("═══════════════════════════════════════");

            Console.Write("Введите имя для поиска: ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                ShowError("Имя для поиска пустое.");
                WaitForContinue();
                return;
            }

            var heroes = _presenter.FindHeroesByName(name);

            if (!heroes.Any())
            {
                ShowMessage($"Героев с именем '{name}' не найдено.");
            }
            else
            {
                Console.WriteLine($"\nНайдено {heroes.Count} героев:");
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10}",
                    "ID", "Имя", "Раса", "HP", "Сила");
                Console.WriteLine(new string('-', 65));

                foreach (var hero in heroes)
                {
                    var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                    Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10}",
                        hero.Id,
                        hero.Name,
                        hero.Species?.Name ?? "Неизвестно",
                        $"{hpColor} {hero.Hp:F1}",
                        hero.Strange);
                }
            }

            WaitForContinue();
        }

        private void ShowBySpecies()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("       ГРУППИРОВКА ПО РАСАМ");
            Console.WriteLine("═══════════════════════════════════════");

            var heroesBySpecies = _presenter.GroupHeroesBySpecies();

            if (!heroesBySpecies.Any())
            {
                ShowMessage("Героев не найдено.");
            }
            else
            {
                foreach (var species in heroesBySpecies.OrderBy(g => g.Key))
                {
                    Console.WriteLine($"\n🏹 {species.Key} ({species.Value.Count} героев):");
                    Console.WriteLine(new string('-', 50));

                    foreach (var hero in species.Value.OrderBy(h => h.Name))
                    {
                        var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                        Console.WriteLine($"  • {hero.Name} - HP: {hpColor} {hero.Hp:F1}, Сила: {hero.Strange}, Тип: {hero.TypeOfDamage}");
                    }
                }
            }

            WaitForContinue();
        }

        private void ShowByDamageType()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("    ГРУППИРОВКА ПО ТИПУ УРОНА");
            Console.WriteLine("═══════════════════════════════════════");

            var heroesByDamage = _presenter.GroupHeroesByDamageType();

            if (!heroesByDamage.Any())
            {
                ShowMessage("Героев не найдено.");
            }
            else
            {
                foreach (var damageType in heroesByDamage.OrderBy(g => g.Key))
                {
                    Console.WriteLine($"\n⚔️ {damageType.Key} ({damageType.Value.Count} героев):");
                    Console.WriteLine(new string('-', 50));

                    foreach (var hero in damageType.Value.OrderBy(h => h.Name))
                    {
                        var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                        Console.WriteLine($"  • {hero.Name} ({hero.Species?.Name}) - HP: {hpColor} {hero.Hp:F1}, Сила: {hero.Strange}");
                    }
                }
            }

            WaitForContinue();
        }

        private void ShowWoundedHeroes()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("          РАНЕНЫЕ ГЕРОИ");
            Console.WriteLine("═══════════════════════════════════════");

            var wounded = _presenter.GetWoundedHeroes();

            if (!wounded.Any())
            {
                ShowMessage("Раненых героев не найдено.");
            }
            else
            {
                Console.WriteLine($"Найдено {wounded.Count} раненых героев (HP < 50):");
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10}",
                    "ID", "Имя", "Раса", "HP", "Сила");
                Console.WriteLine(new string('-', 65));

                foreach (var hero in wounded.OrderBy(h => h.Hp))
                {
                    var hpColor = hero.Hp < 10 ? "🔴" : "🟡";
                    var status = hero.Hp <= 0 ? "💀 МЕРТВ" : "";
                    Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5}",
                        hero.Id,
                        hero.Name,
                        hero.Species?.Name ?? "Неизвестно",
                        $"{hpColor} {hero.Hp:F1}",
                        hero.Strange,
                        status);
                }
            }

            WaitForContinue();
        }

        private void ShowStrongestHeroes()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("       ТОП-3 САМЫХ СИЛЬНЫХ");
            Console.WriteLine("═══════════════════════════════════════");

            var strongest = _presenter.GetStrongestHeroes(3);

            if (!strongest.Any())
            {
                ShowMessage("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("🏆 Самые сильные герои:");
                int place = 1;

                foreach (var hero in strongest)
                {
                    string medal = place == 1 ? "🥇" : place == 2 ? "🥈" : "🥉";
                    var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                    Console.WriteLine($"\n{medal} {place} место:");
                    Console.WriteLine($"  Имя: {hero.Name}");
                    Console.WriteLine($"  Раса: {hero.Species?.Name ?? "Неизвестно"}");
                    Console.WriteLine($"  Сила: {hero.Strange} 💪");
                    Console.WriteLine($"  HP: {hpColor} {hero.Hp:F1} ❤️");
                    Console.WriteLine($"  Тип урона: {hero.TypeOfDamage}");
                    place++;
                }
            }

            WaitForContinue();
        }

        private void HitHero()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("         НАНЕСЕНИЕ УРОНА");
            Console.WriteLine("═══════════════════════════════════════");

            // Сначала показываем всех героев кратко
            var heroes = _presenter.GetAllHeroes();
            if (!heroes.Any())
            {
                ShowMessage("Нет героев для нанесения урона.");
                WaitForContinue();
                return;
            }

            Console.WriteLine("\nСписок героев:");
            Console.WriteLine("{0,-5} {1,-20} {2,-10}", "ID", "Имя", "HP");
            Console.WriteLine(new string('-', 40));

            foreach (var hero in heroes)
            {
                var hpColor = hero.Hp < 50 ? "🟡" : hero.Hp <= 0 ? "💀" : "🟢";
                Console.WriteLine("{0,-5} {1,-20} {2,-10}",
                    hero.Id, hero.Name, $"{hpColor} {hero.Hp:F1}");
            }

            try
            {
                Console.Write("\nВведите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    ShowError("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = heroes.FirstOrDefault(h => h.Id == id);
                if (hero == null)
                {
                    ShowError("Герой с таким ID не найден.");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine($"\nВыбран герой: {hero.Name}");
                Console.WriteLine($"Текущее HP: {hero.Hp:F1}");

                Console.Write($"Урон (0 - {hero.Hp:F1}): ");
                if (!double.TryParse(Console.ReadLine(), out double damage) || damage < 0)
                {
                    ShowError("Некорректное значение урона.");
                    WaitForContinue();
                    return;
                }

                if (damage > hero.Hp)
                {
                    Console.Write($"\n⚠️  Внимание: урон ({damage}) больше текущего HP ({hero.Hp:F1})!");
                    Console.Write(" Герой умрет. Продолжить? (да/нет): ");
                    var confirm = Console.ReadLine()?.ToLower();

                    if (confirm != "да" && confirm != "д" && confirm != "y" && confirm != "yes")
                    {
                        ShowMessage("Отменено.");
                        WaitForContinue();
                        return;
                    }
                }

                _presenter.HitHero(id, damage);

                // Обновляем данные героя
                var updatedHero = heroes.FirstOrDefault(h => h.Id == id);
                if (updatedHero != null)
                {
                    if (updatedHero.Hp > 0)
                    {
                        ShowMessage($"Урон нанесен! Новое HP: {updatedHero.Hp:F1}", "Успех");
                    }
                    else
                    {
                        ShowMessage($"Герой {hero.Name} погиб!", "Информация");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при нанесении урона: {ex.Message}");
            }

            WaitForContinue();
        }

        private void KillHero()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("          УДАЛЕНИЕ ГЕРОЯ");
            Console.WriteLine("═══════════════════════════════════════");

            var heroes = _presenter.GetAllHeroes();
            if (!heroes.Any())
            {
                ShowMessage("Нет героев для удаления.");
                WaitForContinue();
                return;
            }

            Console.WriteLine("\nСписок героев:");
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10}", "ID", "Имя", "Раса", "HP");
            Console.WriteLine(new string('-', 55));

            foreach (var hero in heroes)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10}",
                    hero.Id,
                    hero.Name,
                    hero.Species?.Name ?? "Неизвестно",
                    $"{hero.Hp:F1}");
            }

            try
            {
                Console.Write("\nВведите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    ShowError("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = heroes.FirstOrDefault(h => h.Id == id);
                if (hero == null)
                {
                    ShowError("Герой с таким ID не найден.");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine($"\nВыбран герой: {hero.Name}");
                Console.WriteLine($"Раса: {hero.Species?.Name}");
                Console.WriteLine($"HP: {hero.Hp:F1}, Сила: {hero.Strange}");

                Console.Write($"\n❌ Вы уверены, что хотите удалить героя '{hero.Name}'? (да/нет): ");
                var confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
                {
                    _presenter.DeleteHero(id);
                    ShowMessage("Герой удален из базы данных!", "Успех");
                }
                else
                {
                    ShowMessage("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при удалении героя: {ex.Message}");
            }

            WaitForContinue();
        }

        private void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("             СТАТИСТИКА");
            Console.WriteLine("═══════════════════════════════════════");

            try
            {
                var stats = _presenter.GetStatistics();

                if (stats == null)
                {
                    ShowError("Не удалось получить статистику.");
                    WaitForContinue();
                    return;
                }

                // Общая статистика
                Console.WriteLine("\n📊 ОБЩАЯ СТАТИСТИКА:");
                Console.WriteLine($"   Всего героев: {stats.TotalHeroes}");
                Console.WriteLine($"   Средняя сила: {stats.AverageStrength:F1}");
                Console.WriteLine($"   Среднее HP: {stats.AverageHp:F1}");
                Console.WriteLine($"   Максимальная сила: {stats.MaxStrength}");
                Console.WriteLine($"   Минимальное HP: {stats.MinHp:F1}");

                // Статистика по расам
                if (stats.SpeciesStats != null && stats.SpeciesStats.Any())
                {
                    Console.WriteLine("\n👥 СТАТИСТИКА ПО РАСАМ:");
                    foreach (var stat in stats.SpeciesStats.OrderByDescending(s => s.Count))
                    {
                        Console.WriteLine($"   {stat.Species}: {stat.Count} героев, " +
                                        $"ср. сила: {stat.AvgStrength:F1}, ср. HP: {stat.AvgHp:F1}");
                    }
                }

                // Статистика по типам урона
                if (stats.DamageTypeStats != null && stats.DamageTypeStats.Any())
                {
                    Console.WriteLine("\n⚔️ СТАТИСТИКА ПО ТИПАМ УРОНА:");
                    foreach (var stat in stats.DamageTypeStats.OrderByDescending(d => d.Count))
                    {
                        Console.WriteLine($"   {stat.DamageType}: {stat.Count} героев, " +
                                        $"общая сила: {stat.TotalStrength}");
                    }
                }

                // Статистика по гендерам
                if (stats.GenderStats != null && stats.GenderStats.Any())
                {
                    Console.WriteLine("\n🚻 СТАТИСТИКА ПО ГЕНДЕРАМ:");
                    foreach (var stat in stats.GenderStats.OrderByDescending(g => g.Count))
                    {
                        Console.WriteLine($"   {stat.Gender}: {stat.Count} героев ({stat.Percentage:F1}%)");
                    }
                }

                // Раненые герои
                if (stats.LowHpHeroes != null && stats.LowHpHeroes.Any())
                {
                    Console.WriteLine($"\n🏥 ГЕРОИ С НИЗКИМ HP (<50): {stats.LowHpHeroes.Count}");
                    foreach (var hero in stats.LowHpHeroes.Take(5))
                    {
                        Console.WriteLine($"   • {hero.Name} - {hero.Hp:F1} HP ({hero.Species?.Name})");
                    }
                    if (stats.LowHpHeroes.Count > 5)
                        Console.WriteLine($"   ... и еще {stats.LowHpHeroes.Count - 5}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при выводе статистики: {ex.Message}");
            }

            WaitForContinue();
        }

        private void RefreshHeroes()
        {
            _presenter.RefreshHeroesList();
            ShowMessage("Список героев обновлен!", "Успех");
            WaitForContinue();
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\n═══════════════════════════════════════");
            Console.Write("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
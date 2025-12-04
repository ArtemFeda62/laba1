using BusinessLogic.Services;

namespace ConsoleInterface
{
    public class ConsoleInterface
    {
        private readonly IHeroLogicService _logic;
        private bool _isRunning;

        public ConsoleInterface(IHeroLogicService logic)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));
            _isRunning = false;
        }

        public void Run()
        {
            _isRunning = true;

            while (_isRunning)
            {
                Console.Clear();
                DisplayMenu();

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
                    case "10": ShowAllSpecies(); break;
                    case "11": ShowStatistics(); break;
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

        private void DisplayMenu()
        {
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("        УПРАВЛЕНИЕ ГЕРОЯМИ");
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
            Console.WriteLine("10. Показать все расы");
            Console.WriteLine("11. Статистика");
            Console.WriteLine("0. Выход из консоли");
            Console.WriteLine("═══════════════════════════════════════");
            Console.Write("Выберите действие: ");
        }

        private void ShowAllHeroes()
        {
            Console.WriteLine("\n═══════════════════════════════════════");
            Console.WriteLine("             ВСЕ ГЕРОИ");
            Console.WriteLine("═══════════════════════════════════════");

            var heroes = _logic.GetAllHeroes();
            if (heroes.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                Console.WriteLine($"Всего героев: {heroes.Count}\n");
                Console.WriteLine("ID | Имя | Раса | HP | Сила | Гендер | Тип урона");
                Console.WriteLine("---|-----|------|----|------|--------|----------");

                foreach (var hero in heroes)
                {
                    string speciesName = hero.Species?.Name ?? "Неизвестно";
                    Console.WriteLine($"{hero.Id,3} | {hero.Name,-15} | {speciesName,-10} | {hero.Hp,5:F1} | {hero.Strange,6} | {hero.Genre,-8} | {hero.TypeOfDamage}");
                }
            }
            WaitForContinue();
        }

        private void ShowAllSpecies()
        {
            Console.WriteLine("\n═══════════════════════════════════════");
            Console.WriteLine("               ВСЕ РАСЫ");
            Console.WriteLine("═══════════════════════════════════════");

            var speciesList = _logic.GetAllSpecies();
            if (speciesList.Count == 0)
            {
                Console.WriteLine("Рас не найдено.");
            }
            else
            {
                foreach (var species in speciesList)
                {
                    Console.WriteLine($"{species.Id}. {species.Name}");
                    Console.WriteLine($"   Описание: {species.Description}");
                    Console.WriteLine($"   Героев этой расы: {_logic.GetAllHeroes().Count(h => h.SpeciesId == species.Id)}");
                    Console.WriteLine();
                }
            }
            WaitForContinue();
        }

        private void AddNewHero()
        {
            try
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("          ДОБАВЛЕНИЕ ГЕРОЯ");
                Console.WriteLine("═══════════════════════════════════════");

                // Показываем доступные расы
                ShowAllSpecies();

                Console.Write("\nИмя героя: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Имя не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("ID расы: ");
                if (!int.TryParse(Console.ReadLine(), out int speciesId) || speciesId <= 0)
                {
                    Console.WriteLine("Некорректный ID расы.");
                    WaitForContinue();
                    return;
                }

                var species = _logic.GetSpeciesById(speciesId);
                if (species == null)
                {
                    Console.WriteLine("Раса с таким ID не найдена.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Гендер: ");
                var genre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    Console.WriteLine("Гендер не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Сила: ");
                if (!int.TryParse(Console.ReadLine(), out int strange) || strange < 0)
                {
                    Console.WriteLine("Некорректное значение силы.");
                    WaitForContinue();
                    return;
                }

                Console.Write("Тип урона: ");
                var damageType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(damageType))
                {
                    Console.WriteLine("Тип урона не может быть пустым.");
                    WaitForContinue();
                    return;
                }

                Console.Write("HP: ");
                if (!double.TryParse(Console.ReadLine(), out double hp) || hp <= 0)
                {
                    Console.WriteLine("Некорректное значение HP.");
                    WaitForContinue();
                    return;
                }

                _logic.CreateHero(name, speciesId, genre, strange, damageType, hp);
                Console.WriteLine("\n✓ Герой успешно добавлен в базу данных!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при добавлении героя: {ex.Message}");
            }
            WaitForContinue();
        }

        private void FindByName()
        {
            Console.Write("\nВведите имя для поиска: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя для поиска пустое.");
                WaitForContinue();
                return;
            }

            var heroes = _logic.FindHeroesByName(name);

            if (heroes.Count == 0)
            {
                Console.WriteLine($"Героев с именем '{name}' не найдено.");
            }
            else
            {
                Console.WriteLine($"\nНайдено {heroes.Count} героев:");
                foreach (var hero in heroes)
                {
                    string speciesName = hero.Species?.Name ?? "Неизвестно";
                    Console.WriteLine($"{hero.Id}) {hero.Name} - {speciesName} ({hero.Hp} HP, сила: {hero.Strange})");
                }
            }
            WaitForContinue();
        }

        private void ShowBySpecies()
        {
            var heroesBySpecies = _logic.GroupHeroesBySpecies();

            if (heroesBySpecies.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("      ГЕРОИ ПО РАСАМ");
                Console.WriteLine("═══════════════════════════════════════");

                foreach (var speciesGroup in heroesBySpecies.OrderBy(g => g.Key))
                {
                    Console.WriteLine($"\n--- {speciesGroup.Key.ToUpper()} ({speciesGroup.Value.Count} героев) ---");
                    foreach (var hero in speciesGroup.Value)
                    {
                        Console.WriteLine($"  {hero.Name} - HP: {hero.Hp}, сила: {hero.Strange}, {hero.Genre}");
                    }
                }
            }
            WaitForContinue();
        }

        private void ShowByDamageType()
        {
            var heroesByDamage = _logic.GroupHeroesByDamageType();

            if (heroesByDamage.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("      ГЕРОИ ПО ТИПУ УРОНА");
                Console.WriteLine("═══════════════════════════════════════");

                foreach (var damageGroup in heroesByDamage.OrderBy(g => g.Key))
                {
                    Console.WriteLine($"\n--- {damageGroup.Key.ToUpper()} ({damageGroup.Value.Count} героев) ---");
                    foreach (var hero in damageGroup.Value)
                    {
                        string speciesName = hero.Species?.Name ?? "Неизвестно";
                        Console.WriteLine($"  {hero.Name} ({speciesName}) - HP: {hero.Hp}, сила: {hero.Strange}");
                    }
                }
            }
            WaitForContinue();
        }

        private void ShowWoundedHeroes()
        {
            var wounded = _logic.GetHeroesWithLowHp(50);

            if (wounded.Count == 0)
            {
                Console.WriteLine("Раненых героев не найдено (HP < 50).");
            }
            else
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("      РАНЕНЫЕ ГЕРОИ (HP < 50)");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine($"Всего раненых: {wounded.Count}\n");

                foreach (var hero in wounded.OrderBy(h => h.Hp))
                {
                    string speciesName = hero.Species?.Name ?? "Неизвестно";
                    Console.WriteLine($"{hero.Id}) {hero.Name} - {speciesName} - {hero.Hp:F1} HP (сила: {hero.Strange})");
                }
            }
            WaitForContinue();
        }

        private void ShowStrongestHeroes()
        {
            var strongest = _logic.GetStrongestHeroes(3);

            if (strongest.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
            }
            else
            {
                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("      ТОП-3 САМЫХ СИЛЬНЫХ ГЕРОЯ");
                Console.WriteLine("═══════════════════════════════════════");

                int place = 1;
                foreach (var hero in strongest)
                {
                    string speciesName = hero.Species?.Name ?? "Неизвестно";
                    Console.WriteLine($"{place}. {hero.Name} ({speciesName})");
                    Console.WriteLine($"   Сила: {hero.Strange}, HP: {hero.Hp:F1}, тип урона: {hero.TypeOfDamage}");
                    place++;
                }
            }
            WaitForContinue();
        }

        private void ShowStatistics()
        {
            try
            {
                var statistics = _logic.GetStatistics();

                Console.WriteLine("\n═══════════════════════════════════════");
                Console.WriteLine("             СТАТИСТИКА");
                Console.WriteLine("═══════════════════════════════════════");

                Console.WriteLine($"\nОБЩАЯ СТАТИСТИКА:");
                Console.WriteLine($"Всего героев: {statistics.TotalHeroes}");
                Console.WriteLine($"Средняя сила: {statistics.AverageStrength:F1}");
                Console.WriteLine($"Среднее HP: {statistics.AverageHp:F1}");
                Console.WriteLine($"Максимальная сила: {statistics.MaxStrength}");
                Console.WriteLine($"Минимальное HP: {statistics.MinHp:F1}");

                Console.WriteLine($"\nРАСПРЕДЕЛЕНИЕ ПО РАСАМ:");
                foreach (var stat in statistics.SpeciesStats.OrderByDescending(s => s.Count))
                {
                    Console.WriteLine($"  {stat.Species}: {stat.Count} героев (ср. сила: {stat.AvgStrength:F1}, ср. HP: {stat.AvgHp:F1})");
                }

                Console.WriteLine($"\nРАСПРЕДЕЛЕНИЕ ПО ТИПАМ УРОНА:");
                foreach (var stat in statistics.DamageTypeStats.OrderByDescending(s => s.Count))
                {
                    Console.WriteLine($"  {stat.DamageType}: {stat.Count} героев (общая сила: {stat.TotalStrength})");
                }

                Console.WriteLine($"\nРАСПРЕДЕЛЕНИЕ ПО ГЕНДЕРАМ:");
                foreach (var stat in statistics.GenderStats.OrderByDescending(s => s.Count))
                {
                    Console.WriteLine($"  {stat.Gender}: {stat.Count} героев ({stat.Percentage:F1}%)");
                }

                if (statistics.LowHpHeroes.Count > 0)
                {
                    Console.WriteLine($"\nГЕРОИ С НИЗКИМ HP (<50): {statistics.LowHpHeroes.Count}");
                    foreach (var hero in statistics.LowHpHeroes)
                    {
                        Console.WriteLine($"  {hero.Name} - {hero.Hp:F1} HP");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения статистики: {ex.Message}");
            }

            WaitForContinue();
        }

        private void HitHero()
        {
            ShowAllHeroes();
            try
            {
                Console.Write("\nВведите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = _logic.GetHero(id);
                if (hero == null)
                {
                    Console.WriteLine("Герой с таким ID не найден.");
                    WaitForContinue();
                    return;
                }

                Console.Write($"Урон (текущее HP: {hero.Hp:F1}): ");
                if (!double.TryParse(Console.ReadLine(), out double damage) || damage <= 0)
                {
                    Console.WriteLine("Некорректное значение урона.");
                    WaitForContinue();
                    return;
                }

                _logic.ApplyDamage(id, damage);
                var updatedHero = _logic.GetHero(id);

                if (updatedHero.Hp > 0)
                {
                    Console.WriteLine($"✓ Урон нанесен! Новое HP: {updatedHero.Hp:F1}");
                }
                else
                {
                    Console.WriteLine("✗ Герой погиб! Вы нанесли смертельный урон");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при нанесении урона: {ex.Message}");
            }
            WaitForContinue();
        }

        private void KillHero()
        {
            try
            {
                Console.Write("\nВведите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    WaitForContinue();
                    return;
                }

                var hero = _logic.GetHero(id);
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
                    _logic.DeleteHero(id);
                    Console.WriteLine("✓ Герой удален из базы данных!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Ошибка при удалении героя: {ex.Message}");
            }
            WaitForContinue();
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}

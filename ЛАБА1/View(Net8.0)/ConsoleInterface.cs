// View/ConsoleInterface.cs
using Presenter;
using Shared;
using Shared.Domain;
using Shared.Interfaces;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace View
{
    public class ConsoleInterface : IView
    {
        private MainPresenter _presenter;
        private bool _isRunning;

        // Реализация событий интерфейса IView
        public event Action<HeroAddedEventArgs> HeroAdded;
        public event Action<HeroDeletedEventArgs> HeroDeleted;
        public event Action<HeroDamagedEventArgs> HeroDamaged;
        public event Action<HeroSearchEventArgs> HeroSearch;
        public event Action<PageChangedEventArgs> PageChanged;
        public event Action RefreshRequested;
        public event Action<SpeciesAddedEventArgs> SpeciesAdded;
        public event Action<SpeciesDeletedEventArgs> SpeciesDeleted;
        public event Action<SpeciesUpdatedEventArgs> SpeciesUpdated;

        private List<Hero> _currentHeroes = new List<Hero>();
        private List<Species> _currentSpecies = new List<Species>();
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        public ConsoleInterface()
        {
            _presenter = new MainPresenter(this);
            _isRunning = false;
        }

        public void Run()
        {
            _isRunning = true;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Герои - Консольное приложение (MVP Architecture)";

            ShowWelcomeScreen();

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

        #region Реализация интерфейса IView

        public void RefreshHeroesList(List<Hero> heroes)
        {
            _currentHeroes = heroes ?? new List<Hero>();
            UpdateStatusBar($"Загружено {_currentHeroes.Count} героев");
        }

        public void UpdateStatusBar(string status)
        {
            Console.Title = $"Герои - {status}";
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            Console.WriteLine();
            Console.WriteLine($"╔{"".PadRight(78, '═')}╗");
            Console.WriteLine($"║ {title,-76} ║");
            Console.WriteLine($"╠{"".PadRight(78, '═')}╣");
            Console.WriteLine($"║ {"",-76} ║");

            // Разбиваем сообщение на строки по 76 символов
            var lines = SplitMessage(message, 76);
            foreach (var line in lines)
            {
                Console.WriteLine($"║ {line,-76} ║");
            }

            Console.WriteLine($"║ {"",-76} ║");
            Console.WriteLine($"╚{"".PadRight(78, '═')}╝");
            Console.WriteLine();
        }

        public void ShowError(string error, string title = "Ошибка")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"╔{"".PadRight(78, '═')}╗");
            Console.WriteLine($"║ {title,-76} ║");
            Console.WriteLine($"╠{"".PadRight(78, '═')}╣");

            var lines = SplitMessage(error, 76);
            foreach (var line in lines)
            {
                Console.WriteLine($"║ {line,-76} ║");
            }

            Console.WriteLine($"╚{"".PadRight(78, '═')}╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        public void SetPaginationInfo(int currentPage, int totalPages, int totalItems)
        {
            _currentPage = currentPage;
            _totalPages = totalPages;
            _totalItems = totalItems;
        }

        public void ShowHeroDetails(Hero hero)
        {
            if (hero == null)
            {
                ShowError("Герой не найден");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"╔{"".PadRight(78, '═')}╗");
            Console.WriteLine($"║ ДЕТАЛЬНАЯ ИНФОРМАЦИЯ О ГЕРОЕ {"".PadRight(40, ' ')}║");
            Console.WriteLine($"╠{"".PadRight(78, '═')}╣");
            Console.WriteLine($"║ {"",-76} ║");

            string[] details = {
                $"🏷️  Имя: {hero.Name}",
                $"🆔  ID: {hero.Id}",
                $"👥  Раса: {hero.Species?.Name ?? "Неизвестно"}",
                $"❤️  HP: {hero.Hp:F1}",
                $"💪  Сила: {hero.Strange}",
                $"👤  Гендер: {hero.Genre}",
                $"⚔️  Тип урона: {hero.TypeOfDamage}"
            };

            foreach (var detail in details)
            {
                Console.WriteLine($"║ {detail,-76} ║");
            }

            Console.WriteLine($"║ {"",-76} ║");
            Console.WriteLine($"╚{"".PadRight(78, '═')}╝");
            Console.WriteLine();
        }

        public void ShowStatistics(object statistics)
        {
            var stats = statistics as HeroStatistics;
            if (stats == null)
            {
                ShowError("Не удалось получить статистику");
                return;
            }

            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                             СТАТИСТИКА ГЕРОЕВ                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Общая статистика
            Console.WriteLine("📊 ОБЩАЯ СТАТИСТИКА");
            Console.WriteLine(new string('─', 80));
            Console.WriteLine($"  Всего героев: {stats.TotalHeroes}");
            Console.WriteLine($"  Средняя сила: {stats.AverageStrength:F2}");
            Console.WriteLine($"  Среднее HP: {stats.AverageHp:F2}");
            Console.WriteLine($"  Максимальная сила: {stats.MaxStrength}");
            Console.WriteLine($"  Минимальное HP: {stats.MinHp:F2}");
            Console.WriteLine();

            // Статистика по расам
            Console.WriteLine("👥 СТАТИСТИКА ПО РАСАМ");
            Console.WriteLine(new string('─', 80));
            if (stats.SpeciesStats != null && stats.SpeciesStats.Count > 0)
            {
                foreach (var stat in stats.SpeciesStats.OrderByDescending(s => s.Count))
                {
                    Console.WriteLine($"  {stat.Species}:");
                    Console.WriteLine($"    • Количество: {stat.Count} героев");
                    Console.WriteLine($"    • Средняя сила: {stat.AvgStrength:F1}");
                    Console.WriteLine($"    • Среднее HP: {stat.AvgHp:F1}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("  Нет данных");
                Console.WriteLine();
            }

            // Статистика по типам урона
            Console.WriteLine("⚔️ СТАТИСТИКА ПО ТИПАМ УРОНА");
            Console.WriteLine(new string('─', 80));
            if (stats.DamageTypeStats != null && stats.DamageTypeStats.Count > 0)
            {
                foreach (var stat in stats.DamageTypeStats.OrderByDescending(d => d.Count))
                {
                    Console.WriteLine($"  {stat.DamageType}:");
                    Console.WriteLine($"    • Количество: {stat.Count} героев");
                    Console.WriteLine($"    • Общая сила: {stat.TotalStrength}");
                    Console.WriteLine($"    • Средняя сила: {(double)stat.TotalStrength / stat.Count:F1}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("  Нет данных");
                Console.WriteLine();
            }

            // Статистика по гендерам
            Console.WriteLine("🚻 СТАТИСТИКА ПО ГЕНДЕРАМ");
            Console.WriteLine(new string('─', 80));
            if (stats.GenderStats != null && stats.GenderStats.Count > 0)
            {
                foreach (var stat in stats.GenderStats.OrderByDescending(g => g.Count))
                {
                    Console.WriteLine($"  {stat.Gender}: {stat.Count} героев ({stat.Percentage:F1}%)");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("  Нет данных");
                Console.WriteLine();
            }

            // Раненые герои
            Console.WriteLine("🏥 ГЕРОИ С НИЗКИМ HP (<50)");
            Console.WriteLine(new string('─', 80));
            if (stats.LowHpHeroes != null && stats.LowHpHeroes.Count > 0)
            {
                Console.WriteLine($"  Всего раненых: {stats.LowHpHeroes.Count}");
                Console.WriteLine();
                Console.WriteLine("  Список раненых:");
                int count = 1;
                foreach (var hero in stats.LowHpHeroes.OrderBy(h => h.Hp))
                {
                    string status = hero.Hp <= 0 ? "💀 МЕРТВ" : $"{hero.Hp:F1} HP";
                    Console.WriteLine($"  {count}. {hero.Name} - {status} ({hero.Species?.Name})");
                    count++;
                }
            }
            else
            {
                Console.WriteLine("  Раненых героев нет");
            }

            Console.WriteLine();
            Console.WriteLine(new string('═', 80));
            Console.WriteLine($"📅 Отчет сгенерирован: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");

            WaitForContinue();
        }

        public void ShowGroupedHeroes(Dictionary<string, List<Hero>> grouped, string title)
        {
            Console.Clear();
            Console.WriteLine($"╔{"".PadRight(78, '═')}╗");
            Console.WriteLine($"║ {title,-76} ║");
            Console.WriteLine($"╚{"".PadRight(78, '═')}╝");
            Console.WriteLine();

            if (!grouped.Any())
            {
                Console.WriteLine("Нет данных для отображения");
                WaitForContinue();
                return;
            }

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                Console.WriteLine($"🏷️  {group.Key} ({group.Value.Count} героев):");
                Console.WriteLine(new string('─', 80));

                int count = 1;
                foreach (var hero in group.Value.OrderBy(h => h.Name).Take(10))
                {
                    string hpStatus = GetHpStatusIcon(hero.Hp);
                    Console.WriteLine($"  {count}. {hero.Name} {hpStatus} HP: {hero.Hp:F1}, Сила: {hero.Strange}, Тип: {hero.TypeOfDamage}");
                    count++;
                }

                if (group.Value.Count > 10)
                {
                    Console.WriteLine($"  ... и еще {group.Value.Count - 10} героев");
                }
                Console.WriteLine();
            }

            WaitForContinue();
        }

        public void ShowSpeciesList(List<Species> species, Action<Species> onSelected = null)
        {
            _currentSpecies = species;

            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                 ВСЕ РАСЫ                                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (!species.Any())
            {
                Console.WriteLine("Рас не найдено");
                WaitForContinue();
                return;
            }

            Console.WriteLine("{0,-5} {1,-25} {2,-40}", "ID", "Название", "Описание");
            Console.WriteLine(new string('═', 80));

            foreach (var s in species.OrderBy(s => s.Name))
            {
                string shortDesc = s.Description;
                if (shortDesc.Length > 37)
                    shortDesc = shortDesc.Substring(0, 34) + "...";

                Console.WriteLine("{0,-5} {1,-25} {2,-40}", s.Id, s.Name, shortDesc);

                // Показываем количество героев этой расы
                var heroCount = _currentHeroes.Count(h => h.SpeciesId == s.Id);
                if (heroCount > 0)
                {
                    Console.WriteLine("     👥 Героев этой расы: {0}", heroCount);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"Всего рас: {species.Count}");

            if (onSelected != null)
            {
                Console.WriteLine();
                Console.Write("Введите ID расы для выбора (0 - отмена): ");
                if (int.TryParse(Console.ReadLine(), out int selectedId))
                {
                    if (selectedId == 0)
                        return;

                    var selectedSpecies = species.FirstOrDefault(s => s.Id == selectedId);
                    if (selectedSpecies != null)
                    {
                        onSelected.Invoke(selectedSpecies);
                    }
                    else
                    {
                        ShowError("Раса с таким ID не найдена");
                    }
                }
            }
            else
            {
                WaitForContinue();
            }
        }

        public void ShowHeroSelection(List<Hero> heroes, Action<Hero> onSelected = null)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                               ВЫБОР ГЕРОЯ                                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (!heroes.Any())
            {
                Console.WriteLine("Героев не найдено");
                WaitForContinue();
                return;
            }

            Console.WriteLine("{0,-5} {1,-25} {2,-20} {3,-10} {4,-10}",
                "№", "Имя", "Раса", "HP", "Сила");
            Console.WriteLine(new string('═', 80));

            int index = 1;
            foreach (var hero in heroes)
            {
                string hpStatus = GetHpStatusIcon(hero.Hp);
                Console.WriteLine("{0,-5} {1,-25} {2,-20} {3,-10} {4,-10}",
                    index,
                    hero.Name,
                    hero.Species?.Name ?? "Неизвестно",
                    $"{hpStatus} {hero.Hp:F1}",
                    hero.Strange);
                index++;
            }

            if (onSelected != null)
            {
                Console.WriteLine();
                Console.Write("Введите номер героя для выбора (0 - отмена): ");
                if (int.TryParse(Console.ReadLine(), out int selectedIndex))
                {
                    if (selectedIndex == 0)
                        return;

                    if (selectedIndex > 0 && selectedIndex <= heroes.Count)
                    {
                        var selectedHero = heroes[selectedIndex - 1];
                        onSelected.Invoke(selectedHero);
                    }
                    else
                    {
                        ShowError("Неверный номер героя");
                    }
                }
            }
            else
            {
                WaitForContinue();
            }
        }

        #endregion

        #region Методы отображения

        private void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║                    🏆 СИСТЕМА УПРАВЛЕНИЯ ГЕРОЯМИ 🏆                       ║");
            Console.WriteLine("║                          (MVP Architecture)                                 ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║                          Консольная версия                                   ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Загрузка данных...");

            // Запрашиваем обновление данных
            RefreshRequested?.Invoke();

            Thread.Sleep(1000);
        }

        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          ГЛАВНОЕ МЕНЮ                                       ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Страница: {0,2} из {1,2} | Всего героев: {2,3} | Размер страницы: {3,3}          ║",
                _currentPage, _totalPages, _totalItems, _pageSize);
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1.  Показать всех героев (текущая страница)                                ║");
            Console.WriteLine("║ 2.  Добавить нового героя                                                  ║");
            Console.WriteLine("║ 3.  Найти героя по имени                                                   ║");
            Console.WriteLine("║ 4.  Удалить героя                                                          ║");
            Console.WriteLine("║ 5.  Нанести урон герою                                                     ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║ 6.  Группировка по расам                                                   ║");
            Console.WriteLine("║ 7.  Группировка по типу урона                                              ║");
            Console.WriteLine("║ 8.  Показать раненых героев (HP < 50)                                      ║");
            Console.WriteLine("║ 9.  Показать топ-3 сильнейших героев                                       ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║ 10. Показать все расы                                                      ║");
            Console.WriteLine("║ 11. Добавить новую расу                                                    ║");
            Console.WriteLine("║ 12. Редактировать расу                                                     ║");
            Console.WriteLine("║ 13. Удалить расу                                                           ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║ 14. Показать статистику                                                    ║");
            Console.WriteLine("║ 15. Настройки пагинации                                                    ║");
            Console.WriteLine("║                                                                              ║");
            Console.WriteLine("║ 0.  Выход                                                                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Выберите действие: ");
        }

        private void ProcessChoice(string choice)
        {
            switch (choice)
            {
                case "1": ShowCurrentHeroes(); break;
                case "2": AddNewHero(); break;
                case "3": FindHeroByName(); break;
                case "4": DeleteHero(); break;
                case "5": HitHero(); break;
                case "6": RequestGroupBySpecies(); break;
                case "7": RequestGroupByDamageType(); break;
                case "8": RequestWoundedHeroes(); break;
                case "9": RequestStrongestHeroes(); break;
                case "10": RequestAllSpecies(); break;
                case "11": AddNewSpecies(); break;
                case "12": EditSpecies(); break;
                case "13": DeleteSpecies(); break;
                case "14": RequestStatistics(); break;
                case "15": ConfigurePagination(); break;
                case "0":
                    _isRunning = false;
                    Console.WriteLine("Завершение работы...");
                    Thread.Sleep(1000);
                    break;
                default:
                    ShowError("Неверный выбор. Попробуйте снова.");
                    WaitForContinue();
                    break;
            }
        }

        private void ShowCurrentHeroes()
        {
            Console.Clear();
            Console.WriteLine($"╔{"".PadRight(78, '═')}╗");
            Console.WriteLine($"║ ТЕКУЩАЯ СТРАНИЦА ГЕРОЕВ (Страница {_currentPage} из {_totalPages}) {"".PadRight(20, ' ')}║");
            Console.WriteLine($"╚{"".PadRight(78, '═')}╝");
            Console.WriteLine();

            if (!_currentHeroes.Any())
            {
                Console.WriteLine("На этой странице нет героев");
                WaitForContinue();
                return;
            }

            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10} {6,-15}",
                "ID", "Имя", "Раса", "HP", "Сила", "Гендер", "Тип урона");
            Console.WriteLine(new string('═', 90));

            foreach (var hero in _currentHeroes)
            {
                string hpStatus = GetHpStatusIcon(hero.Hp);
                string statusIcon = hero.Hp <= 0 ? "💀" : hero.Hp < 50 ? "⚠️" : "✅";

                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10} {6,-15}",
                    hero.Id,
                    $"{statusIcon} {hero.Name}",
                    hero.Species?.Name ?? "Неизвестно",
                    $"{hpStatus} {hero.Hp:F1}",
                    hero.Strange,
                    hero.Genre,
                    hero.TypeOfDamage);
            }

            Console.WriteLine();
            Console.WriteLine($"Показано героев: {_currentHeroes.Count}");

            WaitForContinue();
        }

        private void AddNewHero()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          ДОБАВЛЕНИЕ НОВОГО ГЕРОЯ                           ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // Сначала показываем доступные расы
                Console.WriteLine("Доступные расы:");
                if (_currentSpecies.Any())
                {
                    foreach (var species1 in _currentSpecies)
                    {
                        Console.WriteLine($"  {species1.Id}. {species1.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("  (Нет доступных рас. Сначала добавьте расы)");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine();

                // Ввод данных
                Console.Write("Имя героя: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым");
                    return;
                }

                Console.Write("ID расы: ");
                if (!int.TryParse(Console.ReadLine(), out int speciesId))
                {
                    ShowError("Некорректный ID расы");
                    return;
                }

                var species = _currentSpecies.FirstOrDefault(s => s.Id == speciesId);
                if (species == null)
                {
                    ShowError("Раса с таким ID не найдена");
                    return;
                }

                Console.Write("Гендер: ");
                var genre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    ShowError("Гендер не может быть пустым");
                    return;
                }

                Console.Write("Сила (1-1000): ");
                if (!int.TryParse(Console.ReadLine(), out int strength) || strength < 1 || strength > 1000)
                {
                    ShowError("Некорректное значение силы");
                    return;
                }

                Console.Write("Тип урона: ");
                var damageType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(damageType))
                {
                    ShowError("Тип урона не может быть пустым");
                    return;
                }

                Console.Write("HP (1-10000): ");
                if (!double.TryParse(Console.ReadLine(), out double hp) || hp < 1 || hp > 10000)
                {
                    ShowError("Некорректное значение HP");
                    return;
                }

                // Генерируем событие для Presenter
                HeroAdded?.Invoke(new HeroAddedEventArgs
                {
                    Name = name,
                    SpeciesId = speciesId,
                    Genre = genre,
                    Strange = strength,
                    DamageType = damageType,
                    Hp = hp
                });

                ShowMessage($"Герой '{name}' успешно добавлен!", "Успех");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении героя: {ex.Message}");
            }

            WaitForContinue();
        }

        private void FindHeroByName()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                             ПОИСК ГЕРОЯ                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            Console.Write("Введите имя для поиска: ");
            var searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ShowError("Поисковый запрос не может быть пустым");
                WaitForContinue();
                return;
            }

            // Генерируем событие для Presenter
            HeroSearch?.Invoke(new HeroSearchEventArgs
            {
                SearchTerm = searchTerm
            });

            WaitForContinue();
        }

        private void DeleteHero()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                            УДАЛЕНИЕ ГЕРОЯ                                  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (!_currentHeroes.Any())
            {
                ShowError("Нет героев для удаления");
                WaitForContinue();
                return;
            }

            Console.WriteLine("Текущие герои на странице:");
            Console.WriteLine("{0,-5} {1,-20} {2,-15}", "ID", "Имя", "Раса");
            Console.WriteLine(new string('─', 45));

            foreach (var hero in _currentHeroes)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-15}", hero.Id, hero.Name, hero.Species?.Name ?? "Неизвестно");
            }

            Console.WriteLine();
            Console.Write("Введите ID героя для удаления: ");

            if (!int.TryParse(Console.ReadLine(), out int heroId))
            {
                ShowError("Некорректный ID героя");
                WaitForContinue();
                return;
            }

            var heroToDelete = _currentHeroes.FirstOrDefault(h => h.Id == heroId);
            if (heroToDelete == null)
            {
                ShowError("Герой с таким ID не найден на текущей странице");
                WaitForContinue();
                return;
            }

            Console.WriteLine();
            Console.Write($"Вы уверены, что хотите удалить героя '{heroToDelete.Name}'? (да/нет): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
            {
                // Генерируем событие для Presenter
                HeroDeleted?.Invoke(new HeroDeletedEventArgs
                {
                    HeroId = heroId
                });

                ShowMessage($"Герой '{heroToDelete.Name}' успешно удален", "Успех");
            }
            else
            {
                ShowMessage("Удаление отменено", "Отмена");
            }

            WaitForContinue();
        }

        private void HitHero()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          НАНЕСЕНИЕ УРОНА ГЕРОЮ                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (!_currentHeroes.Any())
            {
                ShowError("Нет героев для нанесения урона");
                WaitForContinue();
                return;
            }

            Console.WriteLine("Текущие герои на странице:");
            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10}", "ID", "Имя", "Раса", "HP");
            Console.WriteLine(new string('─', 55));

            foreach (var hero in _currentHeroes)
            {
                string hpStatus = GetHpStatusIcon(hero.Hp);
                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-10}",
                    hero.Id, hero.Name, hero.Species?.Name ?? "Неизвестно", $"{hpStatus} {hero.Hp:F1}");
            }

            Console.WriteLine();
            Console.Write("Введите ID героя: ");

            if (!int.TryParse(Console.ReadLine(), out int heroId))
            {
                ShowError("Некорректный ID героя");
                WaitForContinue();
                return;
            }

            var heroToHit = _currentHeroes.FirstOrDefault(h => h.Id == heroId);
            if (heroToHit == null)
            {
                ShowError("Герой с таким ID не найден на текущей странице");
                WaitForContinue();
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Выбран герой: {heroToHit.Name}");
            Console.WriteLine($"Текущее HP: {heroToHit.Hp:F1}");

            Console.Write($"Урон (0 - {heroToHit.Hp:F1}): ");
            if (!double.TryParse(Console.ReadLine(), out double damage) || damage < 0)
            {
                ShowError("Некорректное значение урона");
                WaitForContinue();
                return;
            }

            if (damage > heroToHit.Hp)
            {
                Console.WriteLine();
                Console.Write($"⚠️ ВНИМАНИЕ: урон ({damage:F1}) больше текущего HP ({heroToHit.Hp:F1})!");
                Console.Write(" Герой умрет. Продолжить? (да/нет): ");
                var confirm = Console.ReadLine()?.ToLower();

                if (!(confirm == "да" || confirm == "д" || confirm == "y" || confirm == "yes"))
                {
                    ShowMessage("Отменено", "Отмена");
                    WaitForContinue();
                    return;
                }
            }

            // Генерируем событие для Presenter
            HeroDamaged?.Invoke(new HeroDamagedEventArgs
            {
                HeroId = heroId,
                Damage = damage
            });

            WaitForContinue();
        }

        private void AddNewSpecies()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          ДОБАВЛЕНИЕ НОВОЙ РАСЫ                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.Write("Название расы: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Название не может быть пустым");
                    return;
                }

                Console.Write("Описание (необязательно): ");
                var description = Console.ReadLine();

                // Генерируем событие для Presenter
                SpeciesAdded?.Invoke(new SpeciesAddedEventArgs
                {
                    Name = name,
                    Description = description
                });

                ShowMessage($"Раса '{name}' успешно добавлена!", "Успех");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при добавлении расы: {ex.Message}");
            }

            WaitForContinue();
        }

        private void EditSpecies()
        {
            // Запрос списка рас у Presenter
            RequestAllSpecies();
        }

        private void DeleteSpecies()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                            УДАЛЕНИЕ РАСЫ                                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (!_currentSpecies.Any())
            {
                ShowError("Нет рас для удаления");
                WaitForContinue();
                return;
            }

            Console.WriteLine("Доступные расы:");
            foreach (var species in _currentSpecies)
            {
                Console.WriteLine($"  {species.Id}. {species.Name}");
            }

            Console.WriteLine();
            Console.Write("Введите ID расы для удаления: ");

            if (!int.TryParse(Console.ReadLine(), out int speciesId))
            {
                ShowError("Некорректный ID расы");
                WaitForContinue();
                return;
            }

            var speciesToDelete = _currentSpecies.FirstOrDefault(s => s.Id == speciesId);
            if (speciesToDelete == null)
            {
                ShowError("Раса с таким ID не найдена");
                WaitForContinue();
                return;
            }

            // Проверяем, есть ли герои этой расы
            var heroesWithSpecies = _currentHeroes.Count(h => h.SpeciesId == speciesId);
            if (heroesWithSpecies > 0)
            {
                ShowError($"Невозможно удалить расу '{speciesToDelete.Name}'. Существуют герои этой расы ({heroesWithSpecies} шт.).");
                WaitForContinue();
                return;
            }

            Console.WriteLine();
            Console.Write($"Вы уверены, что хотите удалить расу '{speciesToDelete.Name}'? (да/нет): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
            {
                // Генерируем событие для Presenter
                SpeciesDeleted?.Invoke(new SpeciesDeletedEventArgs
                {
                    SpeciesId = speciesId
                });

                ShowMessage($"Раса '{speciesToDelete.Name}' успешно удалена", "Успех");
            }
            else
            {
                ShowMessage("Удаление отменено", "Отмена");
            }

            WaitForContinue();
        }

        private void ConfigurePagination()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                         НАСТРОЙКИ ПАГИНАЦИИ                               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine($"Текущие настройки:");
            Console.WriteLine($"  • Текущая страница: {_currentPage}");
            Console.WriteLine($"  • Всего страниц: {_totalPages}");
            Console.WriteLine($"  • Размер страницы: {_pageSize}");
            Console.WriteLine($"  • Всего героев: {_totalItems}");
            Console.WriteLine();

            Console.WriteLine("Доступные размеры страниц: 5, 10, 20, 50, 100");
            Console.Write("Введите новый размер страницы: ");

            if (int.TryParse(Console.ReadLine(), out int newPageSize))
            {
                if (newPageSize > 0 && newPageSize <= 1000)
                {
                    // Генерируем событие для Presenter
                    PageChanged?.Invoke(new PageChangedEventArgs
                    {
                        PageSize = newPageSize,
                        PageNumber = 1
                    });

                    ShowMessage($"Размер страницы изменен на {newPageSize}", "Успех");
                }
                else
                {
                    ShowError("Размер страницы должен быть от 1 до 1000");
                }
            }
            else
            {
                ShowError("Некорректное значение");
            }

            WaitForContinue();
        }

        #endregion

        #region Запросы к Presenter

        private void RequestGroupBySpecies()
        {
            // В реальном приложении нужно вызвать соответствующий метод Presenter
            ShowMessage("Группировка по расам будет выполнена", "Информация");
        }

        private void RequestGroupByDamageType()
        {
            ShowMessage("Группировка по типу урона будет выполнена", "Информация");
        }

        private void RequestWoundedHeroes()
        {
            ShowMessage("Список раненых героев будет показан", "Информация");
        }

        private void RequestStrongestHeroes()
        {
            ShowMessage("Топ-3 сильнейших героев будет показан", "Информация");
        }

        private void RequestAllSpecies()
        {
            ShowMessage("Список всех рас будет показан", "Информация");
        }

        private void RequestStatistics()
        {
            ShowMessage("Статистика будет показана", "Информация");
        }

        #endregion

        #region Вспомогательные методы

        private string[] SplitMessage(string message, int maxLength)
        {
            var lines = new List<string>();
            var words = message.Split(' ');
            var currentLine = "";

            foreach (var word in words)
            {
                if ((currentLine + " " + word).Length > maxLength)
                {
                    lines.Add(currentLine.Trim());
                    currentLine = word;
                }
                else
                {
                    currentLine += (currentLine.Length == 0 ? "" : " ") + word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine.Trim());

            return lines.ToArray();
        }

        private string GetHpStatusIcon(double hp)
        {
            if (hp <= 0) return "💀";
            if (hp < 20) return "🔴";
            if (hp < 50) return "🟡";
            if (hp < 100) return "🟢";
            return "✅";
        }

        private void WaitForContinue()
        {
            Console.WriteLine();
            Console.WriteLine(new string('═', 80));
            Console.Write("Нажмите любую клавишу для продолжения...");
            Console.ReadKey(true);
        }

        private int _totalItems = 0;

        #endregion
    }
}
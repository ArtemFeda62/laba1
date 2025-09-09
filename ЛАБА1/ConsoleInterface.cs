using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class ConsoleInterface
    {
        private Logic logic;

        /// <summary>
        /// Инициализирует консольный интерфейс и добавляет тестовые данные
        /// </summary>
        public ConsoleInterface()
        {
            logic = new Logic();
            AddSampleData();
        }

        /// <summary>
        /// Добавляет тестовых героев в систему
        /// </summary>
        private void AddSampleData()
        {
            logic.CreateHero("Гоблин Гоша", "Транс", "Гоблин", 500, "Физический урон", 20);
            logic.CreateHero("Блум", "ЖЕНЩИНА", "Фея Винкс", 100, "Магический урон", 100);
            logic.CreateHero("Орк Генадий", "мужик", "Орк", 250, "Кидается какашками", 50);
            logic.CreateHero("Мальфит", "Бинарный", "Камень", 1000, "Камни", 1);
        }

        /// <summary>
        /// Запускает главный цикл приложения
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=-=-= СИСТЕМА УПРАВЛЕНИЯ ГЕРОЯМИ =-=-=");
                Console.WriteLine("1. Показать всех героев");
                Console.WriteLine("2. Добавить героя");
                Console.WriteLine("3. Найти героя по имени");
                Console.WriteLine("4. Группировка по расам");
                Console.WriteLine("5. Группировка по типу урона");
                Console.WriteLine("6. Раненые герои (HP < 50)");
                Console.WriteLine("7. Топ-3 самых сильных героя");
                Console.WriteLine("8. Нанести урон герою");
                Console.WriteLine("9. Убить героя");
                Console.WriteLine("0. Выход");
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
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор. Попробуйте снова."); break;
                }
            }
        }

        /// <summary>
        /// Отображает всех героев в системе
        /// </summary>
        private void ShowAllHeroes()
        {
            Console.WriteLine("\nВсе герои:");
            var heroes = logic.GetListHeros();
            if (heroes.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
                return;
            }

            foreach (var hero in heroes)
            {
                Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)");
            }
        }

        /// <summary>
        /// Добавляет нового героя через консоль
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
                    return;
                }

                Console.Write("Раса: ");
                var species = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(species))
                {
                    Console.WriteLine("Нужна расса , допустим негр.");
                    return;
                }

                Console.Write("Гендер: ");
                var genre = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(genre))
                {
                    Console.WriteLine("Личность небинарная?.");
                    return;
                }

                Console.Write("Сила: ");
                if (!int.TryParse(Console.ReadLine(), out int strange) || strange < 0)
                {
                    Console.WriteLine("ОШибка.");
                    return;
                }

                Console.Write("Тип урона: ");
                var damageType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(damageType))
                {
                    Console.WriteLine("Ошибка.");
                    return;
                }

                Console.Write("HP: ");
                if (!double.TryParse(Console.ReadLine(), out double hp) || hp <= 0)
                {
                    Console.WriteLine("Error.");
                    return;
                }

                logic.CreateHero(name, genre, species, hp, damageType, strange);
                Console.WriteLine("Герой добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении героя: {ex.Message}");
            }
        }

        /// <summary>
        /// Ищет героев по имени
        /// </summary>
        private void FindByName()
        {
            Console.Write("Введите имя: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя для поиска пустое.");
                return;
            }

            var heroes = logic.FindHeroesByName(name);

            if (heroes.Count == 0)
            {
                Console.WriteLine($"Героев с именем '{name}' не найдено.");
                return;
            }

            Console.WriteLine($"\nГерои с именем '{name}':");
            foreach (var hero in heroes)
            {
                Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)");
            }
        }

        /// <summary>
        /// Отображает героев с группировкой по расам
        /// </summary>
        private void ShowBySpecies()
        {
            var heroesBySpecies = logic.GroupHeroesBySpecies();

            if (heroesBySpecies.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
                return;
            }

            foreach (var species in heroesBySpecies)
            {
                Console.WriteLine($"\n--- {species.Key} ---");
                foreach (var hero in species.Value)
                {
                    Console.WriteLine($"  {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}");
                }
            }
        }

        /// <summary>
        /// Отображает героев с группировкой по типу урона
        /// </summary>
        private void ShowByDamageType()
        {
            var heroesByDamage = logic.GroupHeroesByDamageType();

            if (heroesByDamage.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
                return;
            }

            foreach (var damageType in heroesByDamage)
            {
                Console.WriteLine($"\n--- {damageType.Key} ---");
                foreach (var hero in damageType.Value)
                {
                    Console.WriteLine($"  {hero.Name} ({hero.Species}) - HP: {hero.Hp}");
                }
            }
        }

        /// <summary>
        /// Отображает героев с низким здоровьем
        /// </summary>
        private void ShowWoundedHeroes()
        {
            var wounded = logic.GetHeroesWithLowHp(50);

            if (wounded.Count == 0)
            {
                Console.WriteLine("Раненых героев не найдено.");
                return;
            }

            Console.WriteLine("\nРаненые герои (HP < 50):");
            foreach (var hero in wounded)
            {
                Console.WriteLine($"{hero.Id}) {hero.Name} - {hero.Hp} HP");
            }
        }

        /// <summary>
        /// Отображает самых сильных героев
        /// </summary>
        private void ShowStrongestHeroes()
        {
            var strongest = logic.GetStrongestHeroes(3);

            if (strongest.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
                return;
            }

            Console.WriteLine("\nТоп-3 самых сильных героя:");
            foreach (var hero in strongest)
            {
                Console.WriteLine($"{hero.Id}) {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}");
            }
        }

        /// <summary>
        /// Наносит урон выбранному герою
        /// </summary>
        private void HitHero()
        {
            try
            {
                Console.Write("Введите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    return;
                }

                var hero = logic.GetHero(id);
                if (hero == null)
                {
                    Console.WriteLine("Герой с таким ID не найден.");
                    return;
                }

                Console.Write($"Урон (текущее HP: {hero.Hp}): ");
                if (!double.TryParse(Console.ReadLine(), out double damage) || damage <= 0)
                {
                    Console.WriteLine("Некорректное значение урона.");
                    return;
                }

                logic.HitHero(id, damage);
                var updatedHero = logic.GetHero(id);
                Console.WriteLine($"Урон нанесен! Новое HP: {updatedHero.Hp}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при нанесении урона: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет героя из системы
        /// </summary>
        private void KillHero()
        {
            try
            {
                Console.Write("Введите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Некорректный ID героя.");
                    return;
                }

                var hero = logic.GetHero(id);
                if (hero == null)
                {
                    Console.WriteLine("Герой с таким ID не найден.");
                    return;
                }

                Console.Write($"Вы уверены, что хотите удалить героя '{hero.Name}'? (y/n): ");
                var confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
                {
                    logic.KillHero(id);
                    Console.WriteLine("Герой удален!");
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
        }
    }
}
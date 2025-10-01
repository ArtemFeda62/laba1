using System;
using System.Collections.Generic;
using System.Linq;

namespace ЛАБА1
{
    internal class Logic
    {
        private HerosLibrary library;

        /// <summary>
        /// Инициализирует новый экземпляр логики с общей библиотекой героев
        /// </summary>
        public Logic()
        {
            library = SharedData.GetSharedLibrary();
        }
        /// <summary>
        /// Инициализирует новый экземпляр логики с указанной библиотекой
        /// </summary>
        /// <param name="sharedLibrary">Общая библиотека героев</param>
        public Logic(HerosLibrary sharedLibrary)
        {
            library = sharedLibrary;
        }
        // Все остальные методы остаются без изменений...
        /// <summary>
        /// Создает нового героя и добавляет его в библиотеку
        /// </summary>
        public void CreateHero(string name, string genre, string species, double hp, string typeofdamage, int strange)
        {
            var hero = new Hero(name, species, genre, strange, typeofdamage, hp);
            library.AddHero(hero);
        }

        /// <summary>
        /// Получает героя по идентификатору
        /// </summary>
        public Hero GetHero(int id) => library.GetHero(id);

        /// <summary>
        /// Получает список всех героев
        /// </summary>
        public List<Hero> GetListHeros() => library.GetListHeros();

        /// <summary>
        /// Обновляет данные героя
        /// </summary>
        public void UpdateHero(Hero hero) => library.UpdateHero(hero);

        /// <summary>
        /// Удаляет героя по идентификатору
        /// </summary>
        public void KillHero(int id) => library.KillHero(id);

        /// <summary>
        /// Наносит урон герою
        /// </summary>
        public void HitHero(int id, double damage) => library.HitHero(id, damage);

        /// <summary>
        /// Группирует героев по расам
        /// </summary>
        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            return library.GetListHeros()
                .GroupBy(h => h.Species)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Группирует героев по типу урона
        /// </summary>
        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return library.GetListHeros()
                .GroupBy(h => h.TypeOfDamage)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Ищет героев по имени (регистронезависимо)
        /// </summary>
        public List<Hero> FindHeroesByName(string name)
        {
            return library.GetListHeros()
                .Where(h => h.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        /// <summary>
        /// Получает героев с здоровьем меньше или равным указанному
        /// </summary>
        public List<Hero> GetHeroesWithLowHp(double maxHp)
        {
            return library.GetListHeros()
                .Where(h => h.Hp <= maxHp)
                .ToList();
        }

        /// <summary>
        /// Получает самых сильных героев
        /// </summary>
        public List<Hero> GetStrongestHeroes(int count)
        {
            return library.GetListHeros()
                .OrderByDescending(h => h.Strange)
                .Take(count)
                .ToList();
        }
    }
}
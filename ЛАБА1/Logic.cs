using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class Logic
    {
        private HerosLibrary library;

        /// <summary>
        /// Инициализирует новый экземпляр логики с пустой библиотекой героев
        /// </summary>
        public Logic()
        {
            library = new HerosLibrary();
        }

        /// <summary>
        /// Создает нового героя и добавляет его в библиотеку
        /// </summary>
        /// <param name="name">Имя героя</param>
        /// <param name="genre">Гендер героя</param>
        /// <param name="species">Раса героя</param>
        /// <param name="hp">Здоровье героя</param>
        /// <param name="typeofdamage">Тип урона героя</param>
        /// <param name="strange">Сила героя</param>
        public void CreateHero(string name, string genre, string species, double hp, string typeofdamage, int strange)
        {
            var Hero = new Hero(name, species, genre, strange, typeofdamage);
            Hero.Hp = hp;
            library.AddHero(Hero);
        }

        /// <summary>
        /// Получает героя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор героя</param>
        /// <returns>Найденный герой или null</returns>
        public Hero GetHero(int id) => library.GetHero(id);

        /// <summary>
        /// Получает список всех героев
        /// </summary>
        /// <returns>Список всех героев</returns>
        public List<Hero> GetListHeros() => library.GetListHeros();

        /// <summary>
        /// Обновляет данные героя
        /// </summary>
        /// <param name="hero">Объект с обновленными данными</param>
        public void UpdateHero(Hero hero) => library.UpdateHero(hero);

        /// <summary>
        /// Удаляет героя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор героя</param>
        public void KillHero(int id) => library.KillHero(id);

        /// <summary>
        /// Наносит урон герою
        /// </summary>
        /// <param name="id">Идентификатор героя</param>
        /// <param name="damage">Количество урона</param>
        public void HitHero(int id, double damage) => library.HitHero(id, damage);

        /// <summary>
        /// Группирует героев по расам
        /// </summary>
        /// <returns>Словарь с группировкой героев по расам</returns>
        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            return library.GetListHeros()
                .GroupBy(h => h.Species)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Группирует героев по типу урона
        /// </summary>
        /// <returns>Словарь с группировкой героев по типу урона</returns>
        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return library.GetListHeros()
                .GroupBy(h => h.TypeOfDamage)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        /// Ищет героев по имени (регистронезависимо)
        /// </summary>
        /// <param name="name">Часть имени для поиска</param>
        /// <returns>Список найденных героев</returns>
        public List<Hero> FindHeroesByName(string name)
        {
            return library.GetListHeros()
                .Where(h => h.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        /// <summary>
        /// Получает героев с здоровьем меньше или равным указанному
        /// </summary>
        /// <param name="maxHp">Максимальное значение здоровья</param>
        /// <returns>Список героев с низким здоровьем</returns>
        public List<Hero> GetHeroesWithLowHp(double maxHp)
        {
            return library.GetListHeros()
                .Where(h => h.Hp <= maxHp)
                .ToList();
        }

        /// <summary>
        /// Получает самых сильных героев
        /// </summary>
        /// <param name="count">Количество героев для возврата</param>
        /// <returns>Список самых сильных героев</returns>
        public List<Hero> GetStrongestHeroes(int count)
        {
            return library.GetListHeros()
                .OrderByDescending(h => h.Strange)
                .Take(count)
                .ToList();
        }
    }
}
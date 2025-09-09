using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class HerosLibrary
    {
        private List<Hero> heros = new List<Hero>();

        /// <summary>
        /// Добавляет героя в библиотеку
        /// </summary>
        /// <param name="hero">Объект героя для добавления</param>
        public void AddHero(Hero hero)
        {
            hero.Id = 1 + heros.Count;
            heros.Add(hero);
        }

        /// <summary>
        /// Получает героя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор героя</param>
        /// <returns>Найденный герой или null если не найден</returns>
        public Hero GetHero(int id)
        {
            return heros.FirstOrDefault(h => h.Id == id);
        }

        /// <summary>
        /// Получает список всех героев
        /// </summary>
        /// <returns>Список всех героев</returns>
        public List<Hero> GetListHeros()
        {
            return heros;
        }

        /// <summary>
        /// Обновляет данные героя
        /// </summary>
        /// <param name="newHero">Объект с обновленными данными героя</param>
        public void UpdateHero(Hero newHero)
        {
            var hero = heros.FirstOrDefault(h => h.Id == newHero.Id);
            if (hero != null)
            {
                hero.Id = newHero.Id;
                hero.Name = newHero.Name;
                hero.Species = newHero.Species;
                hero.Strange = newHero.Strange;
                hero.TypeOfDamage = newHero.TypeOfDamage;
                hero.Genre = newHero.Genre;
                hero.Hp = newHero.Hp;
            }
        }

        /// <summary>
        /// Наносит урон герою
        /// </summary>
        /// <param name="id">Идентификатор героя</param>
        /// <param name="damage">Количество урона</param>
        public void HitHero(int id, double damage)
        {
            Hero _hero = heros.FirstOrDefault(h => h.Id == id);
            if (_hero != null)
            {
                _hero.Hp -= damage;
            }
        }

        /// <summary>
        /// Удаляет героя из библиотеки
        /// </summary>
        /// <param name="id">Идентификатор героя для удаления</param>
        public void KillHero(int id)
        {
            var hero = heros.FirstOrDefault(h => h.Id == id);
            if (hero != null)
            {
                heros.Remove(hero);
            }
        }
    }
}
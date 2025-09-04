using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class HerosLibrary
    {
        private List<Hero> heros = new List<Hero>();
        public void AddHero(Hero hero)
        {
            hero.Id = 1 + heros.Count;
            heros.Add(hero);
        }
        public Hero GetHero(int id)
        {
            return heros.FirstOrDefault(h => h.Id == id);
        }
        public List<Hero> GetListHeros()
        {
            return heros;
        }
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
        public void HitHero(int id, double damage)
        {
            Hero _hero = heros.FirstOrDefault(h => h.Id ==id);
            _hero.Hp -= damage;
        }
        public void KillHero(int id)
        {
            heros.Remove(heros.FirstOrDefault(h => h.Id == id));
        }
    }
}

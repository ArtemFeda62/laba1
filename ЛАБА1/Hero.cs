using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Genre { get; set; }
        public int Strange { get; set; }
        public double Hp { get; set; }
        public string TypeOfDamage { get; set; }

        /// <summary>
        /// Создает нового героя с указанными параметрами
        /// </summary>
        /// <param name="name">Имя героя</param>
        /// <param name="species">Раса героя</param>
        /// <param name="genre">Гендер героя</param>
        /// <param name="strange">Сила героя</param>
        /// <param name="typeOfDamage">Тип урона героя</param>
        public Hero(string name, string species, string genre, int strange, string typeOfDamage)
        {
            Name = name;
            Species = species;
            Genre = genre;
            Strange = strange;
            TypeOfDamage = typeOfDamage;
            Hp = 100;
        }
    }
}
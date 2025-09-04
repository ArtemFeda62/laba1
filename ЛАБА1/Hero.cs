using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class Hero
    {
        public int Id { get; set; }//айди
        public string Name { get; set; }//Имя
        public string Species { get; set; }//Раса
        public int Genre { get; set; }//Гендер
        public string Strange { get; set; }//Сила
        public double Hp { get; set; } //Хп
        public string TypeOfDamage { get; set; }//Тип урона
        public Hero(int id, string name, string species, int genre, string strange, string typeOfDamage)
        {
            Id = id;
            Name = name;
            Species = species;
            Genre = genre;
            Strange = strange;
            TypeOfDamage = typeOfDamage;
        }
    }
}

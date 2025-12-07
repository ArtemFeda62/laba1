using Shared.Domain;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Hero : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int Strange { get; set; }
        public double Hp { get; set; }
        public string TypeOfDamage { get; set; }
        public int SpeciesId { get; set; }
        public virtual Species Species { get; set; }

        public Hero() { }

        public Hero(string name, int speciesId, string genre, int strange, string typeOfDamage, double hp)
        {
            Name = name;
            SpeciesId = speciesId;
            Genre = genre;
            Strange = strange;
            TypeOfDamage = typeOfDamage;
            Hp = hp;
        }
    }
}
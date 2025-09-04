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
        public Logic() 
        {
            library = new HerosLibrary();
        }
        public void CreateHero(string name, string genre, string species, double hp, string typeofdamage, int strange)
        {
            var Hero = new Hero(name, species, genre, strange, typeofdamage);
            library.AddHero(Hero);
        }
    }
}

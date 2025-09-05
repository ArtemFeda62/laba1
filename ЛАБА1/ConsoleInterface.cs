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

        public ConsoleInterface()
        {
            logic = new Logic();
            AddSampleData();
        }

        private void AddSampleData()
        {
            logic.CreateHero("Гоблин Гоша","Транс","Гоблин",500,"Физический урон",20);
            logic.CreateHero("Блум","ЖЕНЩИНА","Фея Винкс",100,"Магический урон",100);
            logic.CreateHero("Орк Генадий","мужик","Орк",250,"Кидается какашками",50);
            logic.CreateHero("Мальфит","Бинарный","Камень",1000,"Камни",1);
        }

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
                    default: Console.WriteLine("Error"); break;
                }
            }
        }

        private void ShowAllHeroes()
        {
            Console.WriteLine("\nВсе герои:");
            foreach (var hero in logic.GetListHeros())
            {
                Console.WriteLine(hero.Id+")"+hero.Name);
            }
        }

        private void AddNewHero()
        {
            Console.Write("Имя: ");
            var name = Console.ReadLine();
            Console.Write("Раса: ");
            var species = Console.ReadLine();
            Console.Write("Гендер: ");
            var genre = Console.ReadLine();
            Console.Write("Сила: ");
            var strange = int.Parse(Console.ReadLine());
            Console.Write("Тип урона: ");
            var damageType = Console.ReadLine();
            Console.Write("хп: ");
            var hp = double.Parse(Console.ReadLine());

            logic.CreateHero(name,genre,species,hp,damageType,strange);
            Console.WriteLine("Герой добавлен!");
        }

        private void FindByName()
        {
            Console.Write("Введите имя: ");
            var name = Console.ReadLine();
            var heroes = logic.FindHeroesByName(name);

            Console.WriteLine($"\nГерои с именем '{name}':");
            foreach (var hero in heroes)
            {
                Console.WriteLine(hero);
            }
        }

        private void ShowBySpecies()
        {
            var heroesBySpecies = logic.GroupHeroesBySpecies();

            foreach (var species in heroesBySpecies)
            {
                Console.WriteLine($"\n--- {species.Key} ---");
                foreach (var hero in species.Value)
                {
                    Console.WriteLine($"  {hero.Name} - Сила: {hero.Strange}");
                }
            }
        }

        private void ShowByDamageType()
        {
            var heroesByDamage = logic.GroupHeroesByDamageType();

            foreach (var damageType in heroesByDamage)
            {
                Console.WriteLine($"\n--- {damageType.Key} урон ---");
                foreach (var hero in damageType.Value)
                {
                    Console.WriteLine($"  {hero.Name} ({hero.Species})");
                }
            }
        }

        private void ShowWoundedHeroes()
        {
            var wounded = logic.GetHeroesWithLowHp(50);
            Console.WriteLine("\nРаненые герои (HP < 50):");
            foreach (var hero in wounded)
            {
                Console.WriteLine(hero);
            }
        }

        private void ShowStrongestHeroes()
        {
            var strongest = logic.GetStrongestHeroes(3);
            Console.WriteLine("\nТоп-3 самых сильных героя:");
            foreach (var hero in strongest)
            {
                Console.WriteLine(hero);
            }
        }

        private void HitHero()
        {
            Console.Write("Введите ID героя: ");
            var id = int.Parse(Console.ReadLine());
            Console.Write("Урон: ");
            var damage = double.Parse(Console.ReadLine());
            Console.WriteLine("Урон нанесен!");
        }

        private void KillHero()
        {
            Console.Write("Введите ID героя для удаления: ");
            var id = int.Parse(Console.ReadLine());
            Console.WriteLine("Герой убит!");
        }
    }
}


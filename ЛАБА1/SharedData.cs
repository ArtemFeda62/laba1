using System;
using System.Collections.Generic;

namespace ЛАБА1
{
    public static class SharedData
    {
        private static HerosLibrary _sharedLibrary;
        private static readonly object _lockObject = new object();

        static SharedData()
        {
            _sharedLibrary = new HerosLibrary();
            LoadSampleData();
        }

        public static HerosLibrary GetSharedLibrary()
        {
            lock (_lockObject)
            {
                return _sharedLibrary;
            }
        }

        public static void ResetData()
        {
            lock (_lockObject)
            {
                _sharedLibrary = new HerosLibrary();
                LoadSampleData();
            }
        }

        private static void LoadSampleData()
        {
            var tempLogic = new Logic(_sharedLibrary);
            tempLogic.CreateHero("Гоблин Гоша", "Транс", "Гоблин", 500, "Физический урон", 20);
            tempLogic.CreateHero("Блум", "ЖЕНЩИНА", "Фея Винкс", 100, "Магический урон", 100);
            tempLogic.CreateHero("Орк Генадий", "мужик", "Орк", 250, "Кидается какашками", 50);
            tempLogic.CreateHero("Мальфит", "Бинарный", "Камень", 1000, "Камни", 1);
            tempLogic.CreateHero("Крип-маг", "мужик", "Крип", 10, "Магический урон", 1);
            tempLogic.CreateHero("Хорнет", "женщина", "паук", 6, "SHAWWWW!", 100000);
        }
    }
}

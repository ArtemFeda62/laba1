using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    public static class SharedData
    {
        private static HerosLibrary _sharedLibrary;
        private static readonly object _lockObject = new object();
        private static readonly string _dataFilePath = "heroes_data.csv";  //файл для сохранения данных

        static SharedData()
        {
            LoadFile();
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
                SaveFile();
            }
        }
        public static void SaveFile()
        {
            try
            {
                lock (_lockObject)
                {
                    var heroes = _sharedLibrary.GetListHeros();
                    var lines = new List<string>();
                    lines.Add("Id,Name,Species,Genre,Strange,Hp,TypeOfDamage");

                    foreach (var hero in heroes)
                    {
                        var name = hero.Name.Contains(",") ? $"\"{hero.Name}\"" : hero.Name;
                        var species = hero.Species.Contains(",") ? $"\"{hero.Species}\"" : hero.Species;
                        var genre = hero.Genre.Contains(",") ? $"\"{hero.Genre}\"" : hero.Genre;
                        var damageType = hero.TypeOfDamage.Contains(",") ? $"\"{hero.TypeOfDamage}\"" : hero.TypeOfDamage;
                        var line = $"{hero.Id},{name},{species},{genre},{hero.Strange},{hero.Hp},{damageType}";
                        lines.Add(line);
                    }

                    File.WriteAllLines(_dataFilePath, lines, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        public static void ReloadFromFile()
        {
            LoadFile();
        }

        private static void LoadFile()
        {
            lock (_lockObject)
            {
                if (File.Exists(_dataFilePath))
                {
                    try
                    {
                        var lines = File.ReadAllLines(_dataFilePath, Encoding.UTF8);
                        _sharedLibrary = new HerosLibrary();

                        for (int i = 1; i < lines.Length; i++)
                        {
                            var line = lines[i].Trim();
                            if (string.IsNullOrEmpty(line)) continue;
                            var hero = ParseLine(line);
                            if (hero != null)
                            {
                                _sharedLibrary.AddHero(hero);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        _sharedLibrary = new HerosLibrary();
                        LoadSampleData();
                    }
                }
                else
                {
                    _sharedLibrary = new HerosLibrary();
                    LoadSampleData();
                    SaveFile();
                }
            }
        }

        private static Hero ParseLine(string csvLine)
        {
            try
            {
                var fields = new List<string>();
                var currentField = new StringBuilder();
                bool inQuotes = false;

                for (int i = 0; i < csvLine.Length; i++)
                {
                    char c = csvLine[i];

                    if (c == '"')
                    {
                        inQuotes = !inQuotes;
                    }
                    else if (c == ',' && !inQuotes)
                    {
                        fields.Add(currentField.ToString());
                        currentField.Clear();
                    }
                    else
                    {
                        currentField.Append(c);
                    }
                }

                fields.Add(currentField.ToString());

                if (fields.Count >= 7)
                {
                    return new Hero(
                        name: fields[1].Trim(),
                        species: fields[2].Trim(),
                        genre: fields[3].Trim(),
                        strange: int.Parse(fields[4].Trim()),
                        typeOfDamage: fields[6].Trim(),
                        hp: double.Parse(fields[5].Trim())
                    )
                    {
                        Id = int.Parse(fields[0].Trim())
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return null;
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
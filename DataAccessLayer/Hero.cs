using DataAccessLayer;

namespace ЛАБА1
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
        // нужен для EF
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
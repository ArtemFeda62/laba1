namespace Shared.Dtos
{
    public class HeroDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpeciesName { get; set; }
        public string Genre { get; set; }
        public int Strange { get; set; }
        public double Hp { get; set; }
        public string TypeOfDamage { get; set; }
        public int SpeciesId { get; set; }
    }

    public class SpeciesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
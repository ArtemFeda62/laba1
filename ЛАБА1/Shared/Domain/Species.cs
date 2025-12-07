using Shared.Interfases;


namespace Shared.Domain
{
    public class Species : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Hero> Heroes { get; set; }

        public Species()
        {
            Heroes = new List<Hero>();
        }
    }
}

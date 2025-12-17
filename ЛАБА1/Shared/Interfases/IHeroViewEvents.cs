namespace Shared.Interfaces
{
    public interface IHeroViewEvents
    {
        event Action<HeroAddedEventArgs> HeroAdded;
        event Action<HeroDeletedEventArgs> HeroDeleted;
        event Action<HeroDamagedEventArgs> HeroDamaged;
        event Action<HeroSearchEventArgs> HeroSearch;
        event Action<PageChangedEventArgs> PageChanged;
        event Action RefreshRequested;
    }

    public interface ISpeciesViewEvents
    {
        event Action<SpeciesAddedEventArgs> SpeciesAdded;
        event Action<SpeciesDeletedEventArgs> SpeciesDeleted;
        event Action<SpeciesUpdatedEventArgs> SpeciesUpdated;
    }

    public class HeroAddedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public string Genre { get; set; }
        public int Strange { get; set; }
        public string DamageType { get; set; }
        public double Hp { get; set; }
    }

    public class HeroDeletedEventArgs : EventArgs
    {
        public int HeroId { get; set; }
    }

    public class HeroDamagedEventArgs : EventArgs
    {
        public int HeroId { get; set; }
        public double Damage { get; set; }
    }

    public class HeroSearchEventArgs : EventArgs
    {
        public string SearchTerm { get; set; }
    }

    public class PageChangedEventArgs : EventArgs
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class SpeciesAddedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SpeciesDeletedEventArgs : EventArgs
    {
        public int SpeciesId { get; set; }
    }

    public class SpeciesUpdatedEventArgs : EventArgs
    {
        public int SpeciesId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
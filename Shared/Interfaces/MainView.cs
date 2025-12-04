using System;
using System.Collections.Generic;
using Shared.Dtos;

namespace Shared.Interfaces
{
    public interface IMainView
    {
        // События
        event EventHandler LoadHeroes;
        event EventHandler AddHero;
        event EventHandler DeleteHero;
        event EventHandler<int> HeroSelected;
        event EventHandler<double> HeroDamaged;
        event EventHandler ShowStatistics;
        event EventHandler<int> PageChanged;
        event EventHandler<int> PageSizeChanged;
        event EventHandler FindHeroes;
        event EventHandler ShowAllSpecies;

        // Методы для обновления View
        void DisplayHeroes(List<HeroDto> heroes);
        void DisplayStatistics(string statistics);
        void ShowMessage(string message, string title);
        void UpdatePagination(int currentPage, int totalPages, int totalCount);
        void ShowSpecies(List<SpeciesDto> species);

        // Свойства
        int SelectedHeroId { get; }
        string SearchName { get; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }

    public interface IHeroDialogView
    {
        event EventHandler<HeroDto> SaveHero;
        event EventHandler Cancel;

        // Свойства
        string HeroName { get; set; }
        int SpeciesId { get; set; }
        string Genre { get; set; }
        int Strange { get; set; }
        string DamageType { get; set; }
        double Hp { get; set; }

        // Методы
        void SetSpecies(List<SpeciesDto> species);
        void ShowDialog();
        void CloseDialog();
    }

    public interface IHitHeroView
    {
        event EventHandler<double> ApplyDamage;
        event EventHandler Cancel;

        double DamageAmount { get; }
        void SetHeroInfo(string heroName, double currentHp);
        void ShowDialog();
        void CloseDialog();
    }
}

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

namespace Shared.Events
{
    public class HeroUpdatedEventArgs : EventArgs
    {
        public int HeroId { get; }
        public double NewHp { get; }

        public HeroUpdatedEventArgs(int heroId, double newHp)
        {
            HeroId = heroId;
            NewHp = newHp;
        }
    }
}
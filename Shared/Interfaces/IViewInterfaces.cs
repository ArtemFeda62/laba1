using System;
using System.Collections.Generic;
using Shared.Dtos;

namespace Shared.Interfaces
{
    public interface IMainView
    {
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

        void DisplayHeroes(List<HeroDto> heroes);
        void DisplayStatistics(string statistics);
        void ShowMessage(string message, string title);
        void UpdatePagination(int currentPage, int totalPages, int totalCount);
        void ShowSpecies(List<SpeciesDto> species);

        int SelectedHeroId { get; }
        string SearchName { get; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
    }

    public interface IHeroDialogView
    {
        event EventHandler<HeroDto> SaveHero;
        event EventHandler Cancel;

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

    public interface IStatisticsView
    {
        event EventHandler RefreshStatistics;
        event EventHandler Close;

        void DisplayStatistics(string statistics);
        void ShowError(string errorMessage);
        void CloseView();
        void ShowDialog();
    }
}
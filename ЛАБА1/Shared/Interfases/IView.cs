// Shared/Interfaces/IView.cs
using System;
using System.Collections.Generic;
using Shared.Domain;

namespace Shared.Interfaces
{
    public interface IView : IHeroViewEvents, ISpeciesViewEvents
    {
        void RefreshHeroesList(List<Hero> heroes);
        void UpdateStatusBar(string status);
        void ShowMessage(string message, string title = "Информация");
        void ShowError(string error, string title = "Ошибка");
        void ShowHeroDetails(Hero hero);
        void ShowStatistics(object statistics);
        void ShowGroupedHeroes(Dictionary<string, List<Hero>> grouped, string title);
        void SetPaginationInfo(int currentPage, int totalPages, int totalItems);

        void ShowSpeciesList(List<Species> species, Action<Species> onSelected = null);
        void ShowHeroSelection(List<Hero> heroes, Action<Hero> onSelected = null);
    }
}
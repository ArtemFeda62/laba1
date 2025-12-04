using BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IHeroView
    {
        event EventHandler LoadHeroesRequested;
        event EventHandler<int> HeroSelected;
        event EventHandler AddHeroRequested;
        event EventHandler<int> DeleteHeroRequested;
        event EventHandler<(int heroId, double damage)> DamageHeroRequested;
        event EventHandler ShowStatisticsRequested;
        event EventHandler<int> PageChanged;
        event EventHandler<int> PageSizeChanged;

        void DisplayHeroes(List<HeroDisplayItem> heroes);
        void DisplayStatistics(HeroStatistics statistics);
        void ShowMessage(string message, string title);
        void UpdatePagination(int currentPage, int totalPages, int totalHeroes);
    }
}

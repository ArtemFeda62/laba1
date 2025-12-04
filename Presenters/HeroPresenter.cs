using BusinessLogic.Services;
using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenters
{
    public class HeroPresenter
    {
        private readonly IHeroView _view;
        private readonly IHeroLogicService _logicService;
        private readonly ISpeciesService _speciesService;

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount = 0;
        private List<Hero> _currentHeroes = new List<Hero>();

        public HeroPresenter(IHeroListView view, IHeroLogicService logicService, ISpeciesService speciesService = null)
        {
            _view = view;
            _logicService = logicService;
            _speciesService = speciesService;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.LoadHeroesRequested += OnLoadHeroesRequested;
            _view.PageChanged += OnPageChanged;
            _view.PageSizeChanged += OnPageSizeChanged;
            _view.AddHeroRequested += OnAddHeroRequested;
            _view.DeleteHeroRequested += OnDeleteHeroRequested;
            _view.DamageHeroRequested += OnDamageHeroRequested;
            _view.ShowStatisticsRequested += OnShowStatisticsRequested;
            _view.FindHeroesRequested += OnFindHeroesRequested;
            _view.GroupBySpeciesRequested += OnGroupBySpeciesRequested;
            _view.GroupByDamageTypeRequested += OnGroupByDamageTypeRequested;
            _view.ShowWoundedHeroesRequested += OnShowWoundedHeroesRequested;
            _view.ShowStrongestHeroesRequested += OnShowStrongestHeroesRequested;
            _view.ShowAllSpeciesRequested += OnShowAllSpeciesRequested;
        }

        private void OnLoadHeroesRequested(object sender, EventArgs e)
        {
            LoadHeroes();
        }

        private void LoadHeroes()
        {
            try
            {
                var (heroes, totalCount) = _logicService.GetHeroesWithDapperPagination(_currentPage, _pageSize);
                _currentHeroes = heroes.ToList();
                _totalCount = totalCount;

                var displayItems = heroes.Select(HeroDisplayItem.FromHero).ToList();
                _view.DisplayHeroes(displayItems);

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка загрузки героев: {ex.Message}", "Ошибка");
            }
        }

        private void UpdatePaginationInfo()
        {
            int totalPages = _totalCount > 0 ? (int)Math.Ceiling((double)_totalCount / _pageSize) : 1;
            _view.UpdatePagination(_currentPage, totalPages, _totalCount);
        }

        private void OnPageChanged(object sender, int page)
        {
            if (page < 1) page = 1;

            int totalPages = _totalCount > 0 ? (int)Math.Ceiling((double)_totalCount / _pageSize) : 1;
            if (page > totalPages) page = totalPages;

            _currentPage = page;
            LoadHeroes();
        }

        private void OnPageSizeChanged(object sender, int pageSize)
        {
            _pageSize = pageSize;
            _currentPage = 1;
            LoadHeroes();
        }

        private void OnAddHeroRequested(object sender, EventArgs e)
        {
            var species = _logicService.GetAllSpecies();
            using (var addForm = new AddHeroForm(species))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _logicService.CreateHero(
                            addForm.HeroName,
                            addForm.HeroSpeciesId,
                            addForm.HeroGenre,
                            addForm.HeroStrange,
                            addForm.HeroDamageType,
                            addForm.HeroHp
                        );

                        LoadHeroes();
                        _view.ShowMessage("Герой успешно добавлен!", "Успех");
                    }
                    catch (Exception ex)
                    {
                        _view.ShowMessage($"Ошибка при добавлении героя: {ex.Message}", "Ошибка");
                    }
                }
            }
        }

        private void OnDeleteHeroRequested(object sender, int heroId)
        {
            try
            {
                var hero = _logicService.GetHero(heroId);
                if (hero == null)
                {
                    _view.ShowMessage($"Герой с ID {heroId} не найден", "Ошибка");
                    return;
                }

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить героя '{hero.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    _logicService.DeleteHero(heroId);
                    LoadHeroes();
                    _view.ShowMessage("Герой удален", "Успех");
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка при удалении героя: {ex.Message}", "Ошибка");
            }
        }

        private void OnDamageHeroRequested(object sender, (int heroId, double damage) args)
        {
            try
            {
                var hero = _logicService.GetHero(args.heroId);
                if (hero == null)
                {
                    _view.ShowMessage($"Герой с ID {args.heroId} не найден", "Ошибка");
                    return;
                }
                if (args.damage <= 0)
                {
                    _view.ShowMessage("Урон должен быть положительным числом", "Ошибка");
                    return;
                }

                _logicService.ApplyDamage(args.heroId, args.damage);

                var updatedHero = _logicService.GetHero(args.heroId);

                if (updatedHero.Hp <= 0)
                {
                    _view.ShowMessage($"Герой {hero.Name} погиб!", "Информация");
                }

                LoadHeroes();
                _view.ShowMessage($"Герою {hero.Name} нанесено {args.damage} урона", "Успех");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка при нанесении урона: {ex.Message}", "Ошибка");
            }
        }

        private void OnShowStatisticsRequested(object sender, EventArgs e)
        {
            try
            {
                var statistics = _logicService.GetStatistics();
                _view.DisplayStatistics(statistics);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка получения статистики: {ex.Message}", "Ошибка");
            }
        }

        private void OnFindHeroesRequested(object sender, string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadHeroes();
                    return;
                }

                var heroes = _logicService.FindHeroesByName(searchText);
                var displayItems = heroes.Select(HeroDisplayItem.FromHero).ToList();

                if (displayItems.Count == 0)
                {
                    _view.ShowMessage($"Герои по запросу '{searchText}' не найдены", "Информация");
                }
                else
                {
                    _view.DisplaySearchResults(displayItems);
                    _view.ShowMessage($"Найдено {displayItems.Count} героев", "Результаты поиска");
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка поиска: {ex.Message}", "Ошибка");
            }
        }

        private void OnGroupBySpeciesRequested(object sender, EventArgs e)
        {
            try
            {
                var groupedHeroes = _logicService.GroupHeroesBySpecies();
                var displayGrouped = groupedHeroes.ToDictionary(
                    g => g.Key,
                    g => g.Value.Select(HeroDisplayItem.FromHero).ToList()
                );

                _view.DisplayGroupedBySpecies(displayGrouped);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка группировки по расам: {ex.Message}", "Ошибка");
            }
        }

        private void OnGroupByDamageTypeRequested(object sender, EventArgs e)
        {
            try
            {
                var groupedHeroes = _logicService.GroupHeroesByDamageType();
                var displayGrouped = groupedHeroes.ToDictionary(
                    g => g.Key,
                    g => g.Value.Select(HeroDisplayItem.FromHero).ToList()
                );

                _view.DisplayGroupedByDamageType(displayGrouped);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка группировки по типу урона: {ex.Message}", "Ошибка");
            }
        }

        private void OnShowWoundedHeroesRequested(object sender, EventArgs e)
        {
            try
            {
                var woundedHeroes = _logicService.GetHeroesWithLowHp(50);
                var displayItems = woundedHeroes.Select(HeroDisplayItem.FromHero).ToList();

                if (displayItems.Count == 0)
                {
                    _view.ShowMessage("Раненых героев не найдено (HP < 50)", "Информация");
                }
                else
                {
                    _view.DisplayWoundedHeroes(displayItems);
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void OnShowStrongestHeroesRequested(object sender, EventArgs e)
        {
            try
            {
                var strongestHeroes = _logicService.GetStrongestHeroes(3);
                var displayItems = strongestHeroes.Select(HeroDisplayItem.FromHero).ToList();

                if (displayItems.Count == 0)
                {
                    _view.ShowMessage("Героев не найдено", "Информация");
                }
                else
                {
                    _view.DisplayStrongestHeroes(displayItems);
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void OnShowAllSpeciesRequested(object sender, EventArgs e)
        {
            try
            {
                var species = _logicService.GetAllSpecies();
                var speciesInfo = string.Join("\n",
                    species.Select(s => $"{s.Id}. {s.Name} - {s.Description}"));

                _view.ShowMessage($"Доступные расы:\n{speciesInfo}", "Список рас");
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Ошибка загрузки рас: {ex.Message}", "Ошибка");
            }
        }
    }
}

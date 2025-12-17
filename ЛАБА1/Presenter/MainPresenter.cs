using BusinessLogicLayer;
using Ninject;
using Shared;
using Shared.Domain;
using Shared.Interfaces;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presenter
{
    public class MainPresenter : IDisposable
    {
        private readonly IView _view;
        private readonly Logic _logic;
        private readonly IHeroService _heroService;

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalHeroes = 0;
        private int _totalPages = 0;

        public MainPresenter(IView view)
        {
            _view = view;


            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            _logic = ninjectKernel.Get<Logic>();
            _heroService = ninjectKernel.Get<IHeroService>();


            SubscribeToViewEvents();

            Initialize();
        }

        private void SubscribeToViewEvents()
        {

            _view.HeroAdded += OnHeroAdded;
            _view.HeroDeleted += OnHeroDeleted;
            _view.HeroDamaged += OnHeroDamaged;
            _view.HeroSearch += OnHeroSearch;
            _view.PageChanged += OnPageChanged;
            _view.RefreshRequested += OnRefreshRequested;
            _view.SpeciesAdded += OnSpeciesAdded;
            _view.SpeciesDeleted += OnSpeciesDeleted;
            _view.SpeciesUpdated += OnSpeciesUpdated;
        }

        private void UnsubscribeFromViewEvents()
        {
            _view.HeroAdded -= OnHeroAdded;
            _view.HeroDeleted -= OnHeroDeleted;
            _view.HeroDamaged -= OnHeroDamaged;
            _view.HeroSearch -= OnHeroSearch;
            _view.PageChanged -= OnPageChanged;
            _view.RefreshRequested -= OnRefreshRequested;

            _view.SpeciesAdded -= OnSpeciesAdded;
            _view.SpeciesDeleted -= OnSpeciesDeleted;
            _view.SpeciesUpdated -= OnSpeciesUpdated;
        }

        private void Initialize()
        {
            LoadHeroes();
        }

        private void LoadHeroes()
        {
            try
            {
                _totalHeroes = _logic.GetTotalHeroesCount();
                _totalPages = (int)Math.Ceiling((double)_totalHeroes / _pageSize);

                if (_currentPage > _totalPages && _totalPages > 0)
                    _currentPage = _totalPages;
                else if (_totalPages == 0)
                    _currentPage = 1;

                var heroes = _logic.GetHeroesWithPagination(_currentPage, _pageSize);

                // Передаем данные во View
                _view.RefreshHeroesList(heroes);
                _view.SetPaginationInfo(_currentPage, _totalPages, _totalHeroes);
                _view.UpdateStatusBar($"Всего героев: {_totalHeroes} | Страница: {_currentPage} из {_totalPages}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при загрузке героев: {ex.Message}");
            }
        }
        private void OnHeroAdded(HeroAddedEventArgs e)
        {
            try
            {
                _logic.CreateHero(e.Name, e.SpeciesId, e.Genre, e.Strange, e.DamageType, e.Hp);
                _view.ShowMessage($"Герой {e.Name} успешно добавлен!", "Успех");
                LoadHeroes();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при добавлении героя: {ex.Message}");
            }
        }

        private void OnHeroDeleted(HeroDeletedEventArgs e)
        {
            try
            {
                var hero = _logic.GetHero(e.HeroId);
                if (hero != null)
                {
                    _logic.KillHero(e.HeroId);
                    _view.ShowMessage($"Герой {hero.Name} удален", "Успех");
                    LoadHeroes();
                }
                else
                {
                    _view.ShowError("Герой не найден");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при удалении героя: {ex.Message}");
            }
        }

        private void OnHeroDamaged(HeroDamagedEventArgs e)
        {
            try
            {
                var hero = _logic.GetHero(e.HeroId);
                if (hero != null)
                {
                    var oldHp = hero.Hp;
                    _logic.HitHero(e.HeroId, e.Damage);

                    var updatedHero = _logic.GetHero(e.HeroId);
                    if (updatedHero.Hp <= 0)
                    {
                        _view.ShowMessage($"Герой {hero.Name} погиб!", "Информация");
                    }
                    else
                    {
                        _view.ShowMessage(
                            $"Герою {hero.Name} нанесен урон {e.Damage}. " +
                            $"Осталось HP: {updatedHero.Hp:F1}", "Урон нанесен");
                    }

                    LoadHeroes();
                }
                else
                {
                    _view.ShowError("Герой не найден");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при нанесении урона: {ex.Message}");
            }
        }

        private void OnHeroSearch(HeroSearchEventArgs e)
        {
            try
            {
                var heroes = _logic.FindHeroesByName(e.SearchTerm);
                if (heroes.Any())
                {
                    _view.RefreshHeroesList(heroes);
                    _view.UpdateStatusBar($"Найдено героев: {heroes.Count}");
                }
                else
                {
                    _view.ShowMessage($"Героев по запросу '{e.SearchTerm}' не найдено");
                    LoadHeroes();
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при поиске: {ex.Message}");
            }
        }

        private void OnPageChanged(PageChangedEventArgs e)
        {
            _pageSize = e.PageSize;
            _currentPage = e.PageNumber;
            LoadHeroes();
        }

        private void OnRefreshRequested()
        {
            LoadHeroes();
        }

        private void OnSpeciesAdded(SpeciesAddedEventArgs e)
        {
            try
            {
                _logic.AddSpecies(e.Name, e.Description);
                _view.ShowMessage($"Раса {e.Name} успешно добавлена!", "Успех");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при добавлении расы: {ex.Message}");
            }
        }

        private void OnSpeciesDeleted(SpeciesDeletedEventArgs e)
        {
            try
            {
                var species = _logic.GetSpeciesById(e.SpeciesId);
                if (species != null)
                {
                    _logic.DeleteSpecies(e.SpeciesId);
                    _view.ShowMessage($"Раса {species.Name} удалена", "Успех");
                    LoadHeroes(); 
                }
                else
                {
                    _view.ShowError("Раса не найдена");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при удалении расы: {ex.Message}");
            }
        }

        private void OnSpeciesUpdated(SpeciesUpdatedEventArgs e)
        {
            try
            {
                var species = new Species
                {
                    Id = e.SpeciesId,
                    Name = e.Name,
                    Description = e.Description
                };

                _logic.UpdateSpecies(species);
                _view.ShowMessage($"Раса {e.Name} обновлена", "Успех");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при обновлении расы: {ex.Message}");
            }
        }

        public void RequestStatistics()
        {
            try
            {
                var stats = _heroService.GetStatistics();
                _view.ShowStatistics(stats);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при получении статистики: {ex.Message}");
            }
        }

        public void RequestGroupBySpecies()
        {
            try
            {
                var grouped = _logic.GroupHeroesBySpecies();
                _view.ShowGroupedHeroes(grouped, "Герои по расам");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при группировке: {ex.Message}");
            }
        }

        public void RequestGroupByDamageType()
        {
            try
            {
                var grouped = _logic.GroupHeroesByDamageType();
                _view.ShowGroupedHeroes(grouped, "Герои по типу урона");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при группировке: {ex.Message}");
            }
        }

        public void RequestWoundedHeroes()
        {
            try
            {
                var wounded = _logic.GetHeroesWithLowHp(50);
                _view.ShowHeroSelection(wounded, hero =>
                {
                    _view.ShowHeroDetails(hero);
                });
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при получении раненых героев: {ex.Message}");
            }
        }

        public void RequestStrongestHeroes()
        {
            try
            {
                var strongest = _logic.GetStrongestHeroes(3);
                _view.ShowHeroSelection(strongest, hero =>
                {
                    _view.ShowHeroDetails(hero);
                });
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при получении сильнейших героев: {ex.Message}");
            }
        }

        public void RequestAllSpecies()
        {
            try
            {
                var species = _logic.GetAllSpecies();
                _view.ShowSpeciesList(species);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при получении рас: {ex.Message}");
            }
        }

        public void Dispose()
        {
            UnsubscribeFromViewEvents();
        }
    }
}
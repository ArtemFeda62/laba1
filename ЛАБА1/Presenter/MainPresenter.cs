using BusinessLogicLayer;
using Ninject;
using Shared;
using Shared.Domain;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class MainPresenter
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
        }

        public void Initialize()
        {
            RefreshHeroesList();
        }

        // Пагинация
        public void SetPageSize(int pageSize)
        {
            _pageSize = pageSize;
            _currentPage = 1;
            RefreshHeroesList();
        }

        public void GoToFirstPage()
        {
            _currentPage = 1;
            RefreshHeroesList();
        }

        public void GoToPreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                RefreshHeroesList();
            }
        }

        public void GoToNextPage()
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                RefreshHeroesList();
            }
        }

        public void GoToLastPage()
        {
            _currentPage = _totalPages;
            RefreshHeroesList();
        }

        // Герои
        public void RefreshHeroesList()
        {
            try
            {
                _totalHeroes = _logic.GetListHeros().Count;
                _totalPages = (int)Math.Ceiling((double)_totalHeroes / _pageSize);

                if (_currentPage > _totalPages && _totalPages > 0)
                    _currentPage = _totalPages;
                else if (_totalPages == 0)
                    _currentPage = 1;

                var pagedHeroes = _logic.GetHeroesWithPagination(_currentPage, _pageSize);

                var displayData = pagedHeroes.Select(h => new
                {
                    h.Id,
                    h.Name,
                    SpeciesName = h.Species?.Name ?? "Неизвестно",
                    h.Hp,
                    h.Strange,
                    h.Genre,
                    h.TypeOfDamage
                }).ToList();

                _view.RefreshHeroesList();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                // Здесь можно вызвать метод View для отображения ошибки
                Console.WriteLine($"Ошибка при обновлении списка героев: {ex.Message}");
            }
        }

        private void UpdateStatusBar()
        {
            _view.UpdateStatusBar($"Всего героев: {_totalHeroes} | Страница: {_currentPage} из {_totalPages}");
        }

        public void AddHero(string name, int speciesId, string genre, int strange, string damageType, double hp)
        {
            try
            {
                _logic.CreateHero(name, speciesId, genre, strange, damageType, hp);
                RefreshHeroesList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении героя: {ex.Message}");
            }
        }

        public void DeleteHero(int id)
        {
            try
            {
                _logic.KillHero(id);
                RefreshHeroesList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении героя: {ex.Message}");
            }
        }

        public void HitHero(int id, double damage)
        {
            try
            {
                _logic.HitHero(id, damage);
                RefreshHeroesList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при нанесении урона: {ex.Message}");
            }
        }

        // Расы
        public List<Species> GetAllSpecies()
        {
            return _logic.GetAllSpecies();
        }

        public void AddSpecies(string name, string description)
        {
            try
            {
                _logic.AddSpecies(name, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении расы: {ex.Message}");
            }
        }

        public void UpdateSpecies(Species species)
        {
            try
            {
                _logic.UpdateSpecies(species);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении расы: {ex.Message}");
            }
        }

        public void DeleteSpecies(int id)
        {
            try
            {
                _logic.DeleteSpecies(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении расы: {ex.Message}");
            }
        }

        // Поиск и группировка
        public List<Hero> FindHeroesByName(string name)
        {
            return _logic.FindHeroesByName(name);
        }

        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            return _logic.GroupHeroesBySpecies();
        }

        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return _logic.GroupHeroesByDamageType();
        }

        public List<Hero> GetWoundedHeroes()
        {
            return _logic.GetHeroesWithLowHp(50);
        }

        public List<Hero> GetStrongestHeroes(int count = 3)
        {
            return _logic.GetStrongestHeroes(count);
        }

        // Статистика
        public HeroStatistics GetStatistics()
        {
            return _heroService.GetStatistics();
        }
        public List<Hero> GetAllHeroes()
        {
            try
            {
                return _logic.GetListHeros();
            }
            catch (Exception ex)
            {
                // Используем метод View для отображения ошибки
                _view.ShowError($"Ошибка при получении списка героев: {ex.Message}");
                return new List<Hero>();
            }
        }
        // Свойства для View
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        public int TotalHeroes => _totalHeroes;
        public bool CanGoToPreviousPage => _currentPage > 1;
        public bool CanGoToNextPage => _currentPage < _totalPages;
    }
}

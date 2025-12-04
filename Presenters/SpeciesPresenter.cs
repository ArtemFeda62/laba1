using BusinessLogic.Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenters
{
    public class SpeciesPresenter
    {
        private readonly ISpeciesView _view;
        private readonly IHeroLogicService _logicService;

        public SpeciesPresenter(ISpeciesView view, IHeroLogicService logicService)
        {
            _view = view;
            _logicService = logicService;
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.LoadSpeciesRequested += OnLoadSpeciesRequested;
            _view.AddSpeciesRequested += OnAddSpeciesRequested;
            _view.DeleteSpeciesRequested += OnDeleteSpeciesRequested;
            _view.UpdateSpeciesRequested += OnUpdateSpeciesRequested;
        }

        private void OnLoadSpeciesRequested(object sender, EventArgs e)
        {
            LoadSpecies();
        }

        private void LoadSpecies()
        {
            try
            {
                var species = _logicService.GetAllSpecies();
                _view.DisplaySpecies(species);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рас: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAddSpeciesRequested(object sender, EventArgs e)
        {
            var addForm = new AddSpeciesForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(addForm.SpeciesName))
                    {
                        MessageBox.Show("Название расы не может быть пустым", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _logicService.AddSpecies(addForm.SpeciesName, addForm.SpeciesDescription);
                    LoadSpecies();

                    MessageBox.Show("Раса успешно добавлена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления расы: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnDeleteSpeciesRequested(object sender, int speciesId)
        {
            try
            {
                var species = _logicService.GetSpeciesById(speciesId);
                if (species == null)
                {
                    MessageBox.Show($"Раса с ID {speciesId} не найдена", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var heroesWithSpecies = _logicService.GetAllHeroes()
                    .Where(h => h.SpeciesId == speciesId)
                    .ToList();

                if (heroesWithSpecies.Any())
                {
                    MessageBox.Show(
                        $"Нельзя удалить расу '{species.Name}', так как есть герои этой расы.\n" +
                        $"Сначала удалите или измените расу у этих героев.",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить расу '{species.Name}'?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    _logicService.DeleteSpecies(speciesId);
                    LoadSpecies();

                    MessageBox.Show("Раса успешно удалена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления расы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnUpdateSpeciesRequested(object sender, (int id, string name, string description) args)
        {
            try
            {
                var species = _logicService.GetSpeciesById(args.id);
                if (species == null)
                {
                    MessageBox.Show($"Раса с ID {args.id} не найдена", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(args.name))
                {
                    MessageBox.Show("Название расы не может быть пустым", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var updatedSpecies = new Species
                {
                    Id = args.id,
                    Name = args.name,
                    Description = args.description
                };

                _logicService.UpdateSpecies(updatedSpecies);
                LoadSpecies();

                MessageBox.Show("Раса успешно обновлена", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления расы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

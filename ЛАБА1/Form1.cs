using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ЛАБА1
{
    public partial class Form1 : Form
    {
        private Logic logic;
        private List<Hero> currentHeroes;

        public Form1()
        {
            InitializeComponent();
            logic = new Logic();
            currentHeroes = new List<Hero>();
            LoadSampleData();
            RefreshHeroesList();
        }

        private void LoadSampleData()
        {
            logic.CreateHero("Гоблин Гоша", "Транс", "Гоблин", 500, "Физический урон", 20);
            logic.CreateHero("Блум", "ЖЕНЩИНА", "Фея Винкс", 100, "Магический урон", 100);
            logic.CreateHero("Орк Генадий", "мужик", "Орк", 250, "Кидается какашками", 50);
            logic.CreateHero("Мальфит", "Бинарный", "Камень", 1000, "Камни", 1);
            logic.CreateHero("Крип-маг", "мужик", "Крип", 10, "Магический урон", 1);
            logic.CreateHero("Хорнет", "женщина", "паук", 6, "SHAWWWW!", 100000);
        }

        private void RefreshHeroesList()
        {
            currentHeroes = logic.GetListHeros();
            listBoxHeroes.Items.Clear();

            foreach (var hero in currentHeroes)
            {
                listBoxHeroes.Items.Add($"{hero.Id}: {hero.Name} - {hero.Species} ({hero.Hp} HP)");
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            RefreshHeroesList();
            txtOutput.Text = "Все герои:\n";
            foreach (var hero in currentHeroes)
            {
                txtOutput.Text += $"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)\n";
            }
        }

        private void btnAddHero_Click(object sender, EventArgs e)
        {
            using (var form = new AddHeroForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logic.CreateHero(form.HeroName, form.HeroGenre, form.HeroSpecies,
                                   form.HeroHp, form.HeroDamageType, form.HeroStrange);
                    RefreshHeroesList();
                    txtOutput.Text = "Герой успешно добавлен!";
                }
            }
        }

        private void btnFindByName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Введите имя для поиска");
                return;
            }

            var foundHeroes = logic.FindHeroesByName(txtSearch.Text);
            txtOutput.Text = $"Найдено героев: {foundHeroes.Count}\n";

            foreach (var hero in foundHeroes)
            {
                txtOutput.Text += $"{hero.Id}) {hero.Name} - {hero.Species} ({hero.Hp} HP)\n";
            }
        }

        private void btnGroupBySpecies_Click(object sender, EventArgs e)
        {
            var grouped = logic.GroupHeroesBySpecies();
            txtOutput.Text = "Группировка по расам:\n";

            foreach (var group in grouped)
            {
                txtOutput.Text += $"\n--- {group.Key} ---\n";
                foreach (var hero in group.Value)
                {
                    txtOutput.Text += $"  {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}\n";
                }
            }
        }

        private void btnGroupByDamage_Click(object sender, EventArgs e)
        {
            var grouped = logic.GroupHeroesByDamageType();
            txtOutput.Text = "Группировка по типу урона:\n";

            foreach (var group in grouped)
            {
                txtOutput.Text += $"\n--- {group.Key} ---\n";
                foreach (var hero in group.Value)
                {
                    txtOutput.Text += $"  {hero.Name} ({hero.Species}) - HP: {hero.Hp}\n";
                }
            }
        }

        private void btnWounded_Click(object sender, EventArgs e)
        {
            var wounded = logic.GetHeroesWithLowHp(50);
            txtOutput.Text = "Раненые герои (HP < 50):\n";

            foreach (var hero in wounded)
            {
                txtOutput.Text += $"{hero.Id}) {hero.Name} - {hero.Hp} HP\n";
            }
        }

        private void btnStrongest_Click(object sender, EventArgs e)
        {
            var strongest = logic.GetStrongestHeroes(3);
            txtOutput.Text = "Топ-3 самых сильных героя:\n";

            foreach (var hero in strongest)
            {
                txtOutput.Text += $"{hero.Id}) {hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}\n";
            }
        }

        private void btnHitHero_Click(object sender, EventArgs e)
        {
            if (listBoxHeroes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите героя из списка");
                return;
            }

            var selectedHero = currentHeroes[listBoxHeroes.SelectedIndex];

            using (var form = new HitHeroForm(selectedHero))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logic.HitHero(selectedHero.Id, form.DamageAmount);
                    RefreshHeroesList();

                    var updatedHero = logic.GetHero(selectedHero.Id);
                    if (updatedHero.Hp > 0)
                    {
                        txtOutput.Text = $"Урон нанесен! Новое HP: {updatedHero.Hp}";
                    }
                    else
                    {
                        txtOutput.Text = "Герой сдох! Вы нанесли смертельный урон";
                    }
                }
            }
        }

        private void btnDeleteHero_Click(object sender, EventArgs e)
        {
            if (listBoxHeroes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите героя из списка");
                return;
            }

            var selectedHero = currentHeroes[listBoxHeroes.SelectedIndex];

            var result = MessageBox.Show($"Вы уверены, что хотите удалить героя '{selectedHero.Name}'?",
                                       "Подтверждение удаления",
                                       MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                logic.KillHero(selectedHero.Id);
                RefreshHeroesList();
                txtOutput.Text = "Герой успешно удален!";
            }
        }
    }
}
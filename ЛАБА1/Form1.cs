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
        private System.Windows.Forms.Timer refreshTimer;

        public Form1()
        {
            InitializeComponent();
            logic = new Logic();
            currentHeroes = new List<Hero>();

            // Настраиваем таймер для автоматического обновления
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 500; // Обновление каждые 500 мс
            refreshTimer.Tick += (s, e) => SafeRefreshHeroesList();
            refreshTimer.Start();

            RefreshHeroesList();

            this.Text = "Система управления героями (Одновременно с консолью)";
        }

        private void SafeRefreshHeroesList()
        {
            if (!this.IsDisposed && this.IsHandleCreated)
            {
                this.Invoke(new Action(() => RefreshHeroesList()));
            }
        }

        public void RefreshHeroesList()
        {
            try
            {
                var previousCount = currentHeroes?.Count ?? 0;
                currentHeroes = logic.GetListHeros();

                // Обновляем ListBox только если данные изменились
                if (previousCount != currentHeroes.Count ||
                    listBoxHeroes.Items.Count != currentHeroes.Count)
                {
                    listBoxHeroes.Items.Clear();
                    foreach (var hero in currentHeroes)
                    {
                        listBoxHeroes.Items.Add($"{hero.Id}: {hero.Name} - {hero.Species} ({hero.Hp} HP)");
                    }
                }
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки при обновлении
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            RefreshHeroesList();
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            refreshTimer?.Stop();
            Program.StopApplication();
            base.OnFormClosed(e);
        }

        private void btnRefreshh_Click(object sender, EventArgs e)
        {
            RefreshHeroesList();
            txtOutput.Text = "Данные обновлены!";
        }
    }
}
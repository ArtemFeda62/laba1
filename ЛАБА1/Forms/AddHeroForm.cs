using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace ЛАБА1
{
    public class AddHeroForm : Form
    {
        public string HeroName { get; private set; }
        public int HeroSpeciesId { get; private set; }
        public string HeroGenre { get; private set; }
        public int HeroStrange { get; private set; }
        public string HeroDamageType { get; private set; }
        public double HeroHp { get; private set; }

        private TextBox txtName, txtGenre, txtDamageType;
        private ComboBox cmbSpecies;
        private NumericUpDown numStrange, numHp;
        private Button btnOk, btnCancel;

        public AddHeroForm()
        {
            InitializeComponent();
            LoadSpecies();
        }

        private void InitializeComponent()
        {
            this.Text = "Добавить нового героя";
            this.Size = new Size(350, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            var lblName = new Label { Text = "Имя:", Location = new Point(10, 10), Width = 100 };
            txtName = new TextBox { Location = new Point(120, 10), Width = 200 };

            var lblSpecies = new Label { Text = "Раса:", Location = new Point(10, 40), Width = 100 };
            cmbSpecies = new ComboBox { Location = new Point(120, 40), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblGenre = new Label { Text = "Гендер:", Location = new Point(10, 70), Width = 100 };
            txtGenre = new TextBox { Location = new Point(120, 70), Width = 200 };

            var lblStrange = new Label { Text = "Сила:", Location = new Point(10, 100), Width = 100 };
            numStrange = new NumericUpDown { Location = new Point(120, 100), Width = 200, Minimum = 0, Maximum = 1000000 };

            var lblDamageType = new Label { Text = "Тип урона:", Location = new Point(10, 130), Width = 100 };
            txtDamageType = new TextBox { Location = new Point(120, 130), Width = 200 };

            var lblHp = new Label { Text = "HP:", Location = new Point(10, 160), Width = 100 };
            numHp = new NumericUpDown { Location = new Point(120, 160), Width = 200, Minimum = 1, Maximum = 1000000, Value = 100 };

            btnOk = new Button { Text = "OK", Location = new Point(80, 200), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Отмена", Location = new Point(180, 200), Width = 80, DialogResult = DialogResult.Cancel };

            btnOk.Click += (s, e) =>
            {
                if (ValidateInput())
                {
                    HeroName = txtName.Text;
                    HeroSpeciesId = (int)cmbSpecies.SelectedValue;
                    HeroGenre = txtGenre.Text;
                    HeroStrange = (int)numStrange.Value;
                    HeroDamageType = txtDamageType.Text;
                    HeroHp = (double)numHp.Value;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };

            btnCancel.Click += (s, e) => Close();

            this.Controls.AddRange(new Control[] {
                lblName, txtName, lblSpecies, cmbSpecies, lblGenre, txtGenre,
                lblStrange, numStrange, lblDamageType, txtDamageType, lblHp, numHp,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void LoadSpecies()
        {
            try
            {
                using (var context = new DataAccessLayer.EntityFramework.HeroContext())
                {
                    var species = context.Species.OrderBy(s => s.Name).ToList();
                    cmbSpecies.DataSource = species;
                    cmbSpecies.DisplayMember = "Name";
                    cmbSpecies.ValueMember = "Id";

                    if (species.Any())
                        cmbSpecies.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рас: {ex.Message}");
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя героя");
                txtName.Focus();
                return false;
            }
            if (cmbSpecies.SelectedValue == null)
            {
                MessageBox.Show("Выберите расу героя");
                cmbSpecies.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                MessageBox.Show("Введите гендер героя");
                txtGenre.Focus();
                return false;
            }
            return true;
        }
    }
}
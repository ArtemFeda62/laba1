using System;
using System.Reflection.Emit;

public class AddHeroForm : Form
{
    public string HeroName { get; private set; }
    public string HeroSpecies { get; private set; }
    public string HeroGenre { get; private set; }
    public int HeroStrange { get; private set; }
    public string HeroDamageType { get; private set; }
    public double HeroHp { get; private set; }

    private TextBox txtName, txtSpecies, txtGenre, txtDamageType;
    private NumericUpDown numStrange, numHp;
    private Button btnOk, btnCancel;

    public AddHeroForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Добавить нового героя";
        this.Size = new Size(300, 250);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;

        // Создание и размещение элементов управления
        var lblName = new Label { Text = "Имя:", Location = new Point(10, 10), Width = 100 };
        txtName = new TextBox { Location = new Point(120, 10), Width = 150 };

        var lblSpecies = new Label { Text = "Раса:", Location = new Point(10, 40), Width = 100 };
        txtSpecies = new TextBox { Location = new Point(120, 40), Width = 150 };

        var lblGenre = new Label { Text = "Гендер:", Location = new Point(10, 70), Width = 100 };
        txtGenre = new TextBox { Location = new Point(120, 70), Width = 150 };

        var lblStrange = new Label { Text = "Сила:", Location = new Point(10, 100), Width = 100 };
        numStrange = new NumericUpDown { Location = new Point(120, 100), Width = 150, Minimum = 0, Maximum = 1000000 };

        var lblDamageType = new Label { Text = "Тип урона:", Location = new Point(10, 130), Width = 100 };
        txtDamageType = new TextBox { Location = new Point(120, 130), Width = 150 };

        var lblHp = new Label { Text = "HP:", Location = new Point(10, 160), Width = 100 };
        numHp = new NumericUpDown { Location = new Point(120, 160), Width = 150, Minimum = 1, Maximum = 1000000, Value = 100 };

        btnOk = new Button { Text = "OK", Location = new Point(50, 190), DialogResult = DialogResult.OK };
        btnCancel = new Button { Text = "Отмена", Location = new Point(150, 190), DialogResult = DialogResult.Cancel };

        btnOk.Click += (s, e) =>
        {
            if (ValidateInput())
            {
                HeroName = txtName.Text;
                HeroSpecies = txtSpecies.Text;
                HeroGenre = txtGenre.Text;
                HeroStrange = (int)numStrange.Value;
                HeroDamageType = txtDamageType.Text;
                HeroHp = (double)numHp.Value;
                DialogResult = DialogResult.OK;
                Close();
            }
        };

        this.Controls.AddRange(new Control[] {
                lblName, txtName, lblSpecies, txtSpecies, lblGenre, txtGenre,
                lblStrange, numStrange, lblDamageType, txtDamageType, lblHp, numHp,
                btnOk, btnCancel
            });
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Введите имя героя");
            return false;
        }
        if (string.IsNullOrWhiteSpace(txtSpecies.Text))
        {
            MessageBox.Show("Введите расу героя");
            return false;
        }
        return true;
    }
}


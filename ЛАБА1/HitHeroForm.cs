using System;
using System.Reflection.Emit;

public class HitHeroForm : Form
{
    public double DamageAmount { get; private set; }

    private NumericUpDown numDamage;
    private Button btnOk, btnCancel;
    private Hero targetHero;

    public HitHeroForm(Hero hero)
    {
        targetHero = hero;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = $"Нанести урон герою {targetHero.Name}";
        this.Size = new Size(300, 150);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;

        var lblInfo = new Label
        {
            Text = $"Текущее HP: {targetHero.Hp}\nВведите урон:",
            Location = new Point(10, 10),
            Width = 200,
            Height = 40
        };

        numDamage = new NumericUpDown
        {
            Location = new Point(10, 60),
            Width = 150,
            Minimum = 0,
            Maximum = (decimal)targetHero.Hp * 2,
            Value = (decimal)Math.Min(10, targetHero.Hp)
        };

        btnOk = new Button { Text = "OK", Location = new Point(50, 90), DialogResult = DialogResult.OK };
        btnCancel = new Button { Text = "Отмена", Location = new Point(150, 90), DialogResult = DialogResult.Cancel };

        btnOk.Click += (s, e) =>
        {
            DamageAmount = (double)numDamage.Value;
            DialogResult = DialogResult.OK;
            Close();
        };

        this.Controls.AddRange(new Control[] { lblInfo, numDamage, btnOk, btnCancel });
    }
}
}


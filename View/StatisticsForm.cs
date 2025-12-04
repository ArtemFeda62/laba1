using Shared.Interfaces;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class StatisticsForm : Form, IStatisticsView
    {
        public event EventHandler RefreshStatistics;
        public event EventHandler Close;

        private TextBox txtStatistics;
        private Button btnRefresh;
        private Button btnClose;

        public StatisticsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Статистика героев";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            txtStatistics = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9),
                BackColor = Color.White
            };

            var buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            btnRefresh = new Button { Text = "Обновить", Location = new Point(10, 7), Width = 80 };
            btnClose = new Button { Text = "Закрыть", Location = new Point(100, 7), Width = 80 };

            btnRefresh.Click += (s, e) => RefreshStatistics?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) =>
            {
                Close?.Invoke(this, EventArgs.Empty);
                this.Close();
            };

            buttonPanel.Controls.AddRange(new Control[] { btnRefresh, btnClose });
            this.Controls.AddRange(new Control[] { txtStatistics, buttonPanel });
        }

        public void DisplayStatistics(string statistics)
        {
            txtStatistics.Text = statistics;
        }

        public void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void CloseView()
        {
            this.Close();
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
        }
    }
}
using System.Reflection.Emit;

namespace ЛАБА1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridViewHeroes;
        private Button btnAddHero;
        private Button btnEditHero;
        private Button btnDeleteHero;
        private Button btnHitHero;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnGroupBySpecies;
        private Button btnGroupByDamage;
        private Button btnLowHp;
        private Button btnStrongest;
        private Button btnRefresh;
        private Label lblTotalCount;
        private Label label1;
        private Panel panel1;
        private Panel panel2;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewHeroes = new System.Windows.Forms.DataGridView();
            this.btnAddHero = new System.Windows.Forms.Button();
            this.btnEditHero = new System.Windows.Forms.Button();
            this.btnDeleteHero = new System.Windows.Forms.Button();
            this.btnHitHero = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnGroupBySpecies = new System.Windows.Forms.Button();
            this.btnGroupByDamage = new System.Windows.Forms.Button();
            this.btnLowHp = new System.Windows.Forms.Button();
            this.btnStrongest = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeroes)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewHeroes
            // 
            this.dataGridViewHeroes.AllowUserToAddRows = false;
            this.dataGridViewHeroes.AllowUserToDeleteRows = false;
            this.dataGridViewHeroes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHeroes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHeroes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHeroes.Location = new System.Drawing.Point(12, 120);
            this.dataGridViewHeroes.Name = "dataGridViewHeroes";
            this.dataGridViewHeroes.ReadOnly = true;
            this.dataGridViewHeroes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHeroes.Size = new System.Drawing.Size(760, 380);
            this.dataGridViewHeroes.TabIndex = 0;
            this.dataGridViewHeroes.SelectionChanged += new System.EventHandler(this.dataGridViewHeroes_SelectionChanged);
            // 
            // btnAddHero
            // 
            this.btnAddHero.Location = new System.Drawing.Point(3, 3);
            this.btnAddHero.Name = "btnAddHero";
            this.btnAddHero.Size = new System.Drawing.Size(100, 30);
            this.btnAddHero.TabIndex = 1;
            this.btnAddHero.Text = "Добавить";
            this.btnAddHero.UseVisualStyleBackColor = true;
            this.btnAddHero.Click += new System.EventHandler(this.btnAddHero_Click);
            // 
            // btnEditHero
            // 
            this.btnEditHero.Enabled = false;
            this.btnEditHero.Location = new System.Drawing.Point(109, 3);
            this.btnEditHero.Name = "btnEditHero";
            this.btnEditHero.Size = new System.Drawing.Size(100, 30);
            this.btnEditHero.TabIndex = 2;
            this.btnEditHero.Text = "Редактировать";
            this.btnEditHero.UseVisualStyleBackColor = true;
            this.btnEditHero.Click += new System.EventHandler(this.btnEditHero_Click);
            // 
            // btnDeleteHero
            // 
            this.btnDeleteHero.Enabled = false;
            this.btnDeleteHero.Location = new System.Drawing.Point(215, 3);
            this.btnDeleteHero.Name = "btnDeleteHero";
            this.btnDeleteHero.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteHero.TabIndex = 3;
            this.btnDeleteHero.Text = "Удалить";
            this.btnDeleteHero.UseVisualStyleBackColor = true;
            this.btnDeleteHero.Click += new System.EventHandler(this.btnDeleteHero_Click);
            // 
            // btnHitHero
            // 
            this.btnHitHero.Enabled = false;
            this.btnHitHero.Location = new System.Drawing.Point(321, 3);
            this.btnHitHero.Name = "btnHitHero";
            this.btnHitHero.Size = new System.Drawing.Size(100, 30);
            this.btnHitHero.TabIndex = 4;
            this.btnHitHero.Text = "Нанести урон";
            this.btnHitHero.UseVisualStyleBackColor = true;
            this.btnHitHero.Click += new System.EventHandler(this.btnHitHero_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(209, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Поиск";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnGroupBySpecies
            // 
            this.btnGroupBySpecies.Location = new System.Drawing.Point(427, 3);
            this.btnGroupBySpecies.Name = "btnGroupBySpecies";
            this.btnGroupBySpecies.Size = new System.Drawing.Size(120, 30);
            this.btnGroupBySpecies.TabIndex = 7;
            this.btnGroupBySpecies.Text = "Группировка по расе";
            this.btnGroupBySpecies.UseVisualStyleBackColor = true;
            this.btnGroupBySpecies.Click += new System.EventHandler(this.btnGroupBySpecies_Click);
            // 
            // btnGroupByDamage
            // 
            this.btnGroupByDamage.Location = new System.Drawing.Point(553, 3);
            this.btnGroupByDamage.Name = "btnGroupByDamage";
            this.btnGroupByDamage.Size = new System.Drawing.Size(150, 30);
            this.btnGroupByDamage.TabIndex = 8;
            this.btnGroupByDamage.Text = "Группировка по урону";
            this.btnGroupByDamage.UseVisualStyleBackColor = true;
            this.btnGroupByDamage.Click += new System.EventHandler(this.btnGroupByDamage_Click);
            // 
            // btnLowHp
            // 
            this.btnLowHp.Location = new System.Drawing.Point(290, 3);
            this.btnLowHp.Name = "btnLowHp";
            this.btnLowHp.Size = new System.Drawing.Size(120, 30);
            this.btnLowHp.TabIndex = 9;
            this.btnLowHp.Text = "Раненые герои";
            this.btnLowHp.UseVisualStyleBackColor = true;
            this.btnLowHp.Click += new System.EventHandler(this.btnLowHp_Click);
            // 
            // btnStrongest
            // 
            this.btnStrongest.Location = new System.Drawing.Point(416, 3);
            this.btnStrongest.Name = "btnStrongest";
            this.btnStrongest.Size = new System.Drawing.Size(120, 30);
            this.btnStrongest.TabIndex = 10;
            this.btnStrongest.Text = "Самые сильные";
            this.btnStrongest.UseVisualStyleBackColor = true;
            this.btnStrongest.Click += new System.EventHandler(this.btnStrongest_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(542, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(12, 104);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(76, 13);
            this.lblTotalCount.TabIndex = 12;
            this.lblTotalCount.Text = "Всего героев:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Поиск";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddHero);
            this.panel1.Controls.Add(this.btnEditHero);
            this.panel1.Controls.Add(this.btnDeleteHero);
            this.panel1.Controls.Add(this.btnHitHero);
            this.panel1.Controls.Add(this.btnGroupBySpecies);
            this.panel1.Controls.Add(this.btnGroupByDamage);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 40);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnLowHp);
            this.panel2.Controls.Add(this.btnStrongest);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Location = new System.Drawing.Point(12, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(760, 40);
            this.panel2.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTotalCount);
            this.Controls.Add(this.dataGridViewHeroes);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Система управления героями";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeroes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

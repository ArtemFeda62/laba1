using System.Reflection.Emit;
using System.Drawing;
using System.Windows.Forms;

namespace ЛАБА1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.listBoxHeroes = new System.Windows.Forms.ListBox();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.btnAddHero = new System.Windows.Forms.Button();
            this.btnFindByName = new System.Windows.Forms.Button();
            this.btnGroupBySpecies = new System.Windows.Forms.Button();
            this.btnGroupByDamage = new System.Windows.Forms.Button();
            this.btnWounded = new System.Windows.Forms.Button();
            this.btnStrongest = new System.Windows.Forms.Button();
            this.btnHitHero = new System.Windows.Forms.Button();
            this.btnDeleteHero = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBoxHeroes
            // 
            this.listBoxHeroes.FormattingEnabled = true;
            this.listBoxHeroes.Location = new System.Drawing.Point(12, 12);
            this.listBoxHeroes.Name = "listBoxHeroes";
            this.listBoxHeroes.Size = new System.Drawing.Size(300, 199);
            this.listBoxHeroes.TabIndex = 0;
            // 
            // btnShowAll
            // 
            this.btnShowAll.Location = new System.Drawing.Point(318, 12);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(150, 30);
            this.btnShowAll.TabIndex = 1;
            this.btnShowAll.Text = "Показать всех";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // btnAddHero
            // 
            this.btnAddHero.Location = new System.Drawing.Point(318, 48);
            this.btnAddHero.Name = "btnAddHero";
            this.btnAddHero.Size = new System.Drawing.Size(150, 30);
            this.btnAddHero.TabIndex = 2;
            this.btnAddHero.Text = "Добавить героя";
            this.btnAddHero.UseVisualStyleBackColor = true;
            this.btnAddHero.Click += new System.EventHandler(this.btnAddHero_Click);
            // 
            // btnFindByName
            // 
            this.btnFindByName.Location = new System.Drawing.Point(318, 84);
            this.btnFindByName.Name = "btnFindByName";
            this.btnFindByName.Size = new System.Drawing.Size(150, 30);
            this.btnFindByName.TabIndex = 3;
            this.btnFindByName.Text = "Найти по имени";
            this.btnFindByName.UseVisualStyleBackColor = true;
            this.btnFindByName.Click += new System.EventHandler(this.btnFindByName_Click);
            // 
            // btnGroupBySpecies
            // 
            this.btnGroupBySpecies.Location = new System.Drawing.Point(318, 120);
            this.btnGroupBySpecies.Name = "btnGroupBySpecies";
            this.btnGroupBySpecies.Size = new System.Drawing.Size(150, 30);
            this.btnGroupBySpecies.TabIndex = 4;
            this.btnGroupBySpecies.Text = "Группировка по расам";
            this.btnGroupBySpecies.UseVisualStyleBackColor = true;
            this.btnGroupBySpecies.Click += new System.EventHandler(this.btnGroupBySpecies_Click);
            // 
            // btnGroupByDamage
            // 
            this.btnGroupByDamage.Location = new System.Drawing.Point(318, 156);
            this.btnGroupByDamage.Name = "btnGroupByDamage";
            this.btnGroupByDamage.Size = new System.Drawing.Size(150, 30);
            this.btnGroupByDamage.TabIndex = 5;
            this.btnGroupByDamage.Text = "Группировка по урону";
            this.btnGroupByDamage.UseVisualStyleBackColor = true;
            this.btnGroupByDamage.Click += new System.EventHandler(this.btnGroupByDamage_Click);
            // 
            // btnWounded
            // 
            this.btnWounded.Location = new System.Drawing.Point(318, 192);
            this.btnWounded.Name = "btnWounded";
            this.btnWounded.Size = new System.Drawing.Size(150, 30);
            this.btnWounded.TabIndex = 6;
            this.btnWounded.Text = "Раненые герои";
            this.btnWounded.UseVisualStyleBackColor = true;
            this.btnWounded.Click += new System.EventHandler(this.btnWounded_Click);
            // 
            // btnStrongest
            // 
            this.btnStrongest.Location = new System.Drawing.Point(318, 228);
            this.btnStrongest.Name = "btnStrongest";
            this.btnStrongest.Size = new System.Drawing.Size(150, 30);
            this.btnStrongest.TabIndex = 7;
            this.btnStrongest.Text = "Самые сильные";
            this.btnStrongest.UseVisualStyleBackColor = true;
            this.btnStrongest.Click += new System.EventHandler(this.btnStrongest_Click);
            // 
            // btnHitHero
            // 
            this.btnHitHero.Location = new System.Drawing.Point(318, 264);
            this.btnHitHero.Name = "btnHitHero";
            this.btnHitHero.Size = new System.Drawing.Size(150, 30);
            this.btnHitHero.TabIndex = 8;
            this.btnHitHero.Text = "Нанести урон";
            this.btnHitHero.UseVisualStyleBackColor = true;
            this.btnHitHero.Click += new System.EventHandler(this.btnHitHero_Click);
            // 
            // btnDeleteHero
            // 
            this.btnDeleteHero.Location = new System.Drawing.Point(318, 300);
            this.btnDeleteHero.Name = "btnDeleteHero";
            this.btnDeleteHero.Size = new System.Drawing.Size(150, 30);
            this.btnDeleteHero.TabIndex = 9;
            this.btnDeleteHero.Text = "Удалить героя";
            this.btnDeleteHero.UseVisualStyleBackColor = true;
            this.btnDeleteHero.Click += new System.EventHandler(this.btnDeleteHero_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 230);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Поиск по имени:";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(12, 256);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(300, 150);
            this.txtOutput.TabIndex = 12;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(480, 420);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnDeleteHero);
            this.Controls.Add(this.btnHitHero);
            this.Controls.Add(this.btnStrongest);
            this.Controls.Add(this.btnWounded);
            this.Controls.Add(this.btnGroupByDamage);
            this.Controls.Add(this.btnGroupBySpecies);
            this.Controls.Add(this.btnFindByName);
            this.Controls.Add(this.btnAddHero);
            this.Controls.Add(this.btnShowAll);
            this.Controls.Add(this.listBoxHeroes);
            this.Name = "Form1";
            this.Text = "Система управления героями";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxHeroes;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.Button btnAddHero;
        private System.Windows.Forms.Button btnFindByName;
        private System.Windows.Forms.Button btnGroupBySpecies;
        private System.Windows.Forms.Button btnGroupByDamage;
        private System.Windows.Forms.Button btnWounded;
        private System.Windows.Forms.Button btnStrongest;
        private System.Windows.Forms.Button btnHitHero;
        private System.Windows.Forms.Button btnDeleteHero;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutput;
    }
}

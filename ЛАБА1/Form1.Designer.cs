namespace ЛАБА1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem героиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьГерояToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьГерояToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem нанестиУронToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьСписокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьВсегоГероевToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem найтиПоИмениToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem группировкаПоРасамToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem группировкаПоТипуУронаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem раненыеГероиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem топ3СильнейшихToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьВсеРасыToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.героиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьГерояToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьГерояToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.нанестиУронToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.показатьВсегоГероевToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.найтиПоИмениToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.группировкаПоРасамToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.группировкаПоТипуУронаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.раненыеГероиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.топ3СильнейшихToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьВсеРасыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(800, 404);
            this.dataGridView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.героиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // героиToolStripMenuItem
            // 
            this.героиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьГерояToolStripMenuItem,
            this.удалитьГерояToolStripMenuItem,
            this.нанестиУронToolStripMenuItem,
            this.обновитьСписокToolStripMenuItem,
            this.toolStripSeparator1,
            this.показатьВсегоГероевToolStripMenuItem,
            this.найтиПоИмениToolStripMenuItem,
            this.группировкаПоРасамToolStripMenuItem,
            this.группировкаПоТипуУронаToolStripMenuItem,
            this.раненыеГероиToolStripMenuItem,
            this.топ3СильнейшихToolStripMenuItem,
            this.показатьВсеРасыToolStripMenuItem});
            this.героиToolStripMenuItem.Name = "героиToolStripMenuItem";
            this.героиToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.героиToolStripMenuItem.Text = "Герои";
            // 
            // добавитьГерояToolStripMenuItem
            // 
            this.добавитьГерояToolStripMenuItem.Name = "добавитьГерояToolStripMenuItem";
            this.добавитьГерояToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.добавитьГерояToolStripMenuItem.Text = "Добавить героя";
            this.добавитьГерояToolStripMenuItem.Click += new System.EventHandler(this.добавитьГерояToolStripMenuItem_Click);
            // 
            // удалитьГерояToolStripMenuItem
            // 
            this.удалитьГерояToolStripMenuItem.Name = "удалитьГерояToolStripMenuItem";
            this.удалитьГерояToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.удалитьГерояToolStripMenuItem.Text = "Удалить героя";
            this.удалитьГерояToolStripMenuItem.Click += new System.EventHandler(this.удалитьГерояToolStripMenuItem_Click);
            // 
            // нанестиУронToolStripMenuItem
            // 
            this.нанестиУронToolStripMenuItem.Name = "нанестиУронToolStripMenuItem";
            this.нанестиУронToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.нанестиУронToolStripMenuItem.Text = "Нанести урон";
            this.нанестиУронToolStripMenuItem.Click += new System.EventHandler(this.нанестиУронToolStripMenuItem_Click);
            // 
            // обновитьСписокToolStripMenuItem
            // 
            this.обновитьСписокToolStripMenuItem.Name = "обновитьСписокToolStripMenuItem";
            this.обновитьСписокToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.обновитьСписокToolStripMenuItem.Text = "Обновить список";
            this.обновитьСписокToolStripMenuItem.Click += new System.EventHandler(this.обновитьСписокToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // показатьВсегоГероевToolStripMenuItem
            // 
            this.показатьВсегоГероевToolStripMenuItem.Name = "показатьВсегоГероевToolStripMenuItem";
            this.показатьВсегоГероевToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.показатьВсегоГероевToolStripMenuItem.Text = "Показать всех героев";
            this.показатьВсегоГероевToolStripMenuItem.Click += new System.EventHandler(this.показатьВсегоГероевToolStripMenuItem_Click);
            // 
            // найтиПоИмениToolStripMenuItem
            // 
            this.найтиПоИмениToolStripMenuItem.Name = "найтиПоИмениToolStripMenuItem";
            this.найтиПоИмениToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.найтиПоИмениToolStripMenuItem.Text = "Найти по имени";
            this.найтиПоИмениToolStripMenuItem.Click += new System.EventHandler(this.найтиПоИмениToolStripMenuItem_Click);
            // 
            // группировкаПоРасамToolStripMenuItem
            // 
            this.группировкаПоРасамToolStripMenuItem.Name = "группировкаПоРасамToolStripMenuItem";
            this.группировкаПоРасамToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.группировкаПоРасамToolStripMenuItem.Text = "Группировка по расам";
            this.группировкаПоРасамToolStripMenuItem.Click += new System.EventHandler(this.группировкаПоРасамToolStripMenuItem_Click);
            // 
            // группировкаПоТипуУронаToolStripMenuItem
            // 
            this.группировкаПоТипуУронаToolStripMenuItem.Name = "группировкаПоТипуУронаToolStripMenuItem";
            this.группировкаПоТипуУронаToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.группировкаПоТипуУронаToolStripMenuItem.Text = "Группировка по типу урона";
            this.группировкаПоТипуУронаToolStripMenuItem.Click += new System.EventHandler(this.группировкаПоТипуУронаToolStripMenuItem_Click);
            // 
            // раненыеГероиToolStripMenuItem
            // 
            this.раненыеГероиToolStripMenuItem.Name = "раненыеГероиToolStripMenuItem";
            this.раненыеГероиToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.раненыеГероиToolStripMenuItem.Text = "Раненые герои";
            this.раненыеГероиToolStripMenuItem.Click += new System.EventHandler(this.раненыеГероиToolStripMenuItem_Click);
            // 
            // топ3СильнейшихToolStripMenuItem
            // 
            this.топ3СильнейшихToolStripMenuItem.Name = "топ3СильнейшихToolStripMenuItem";
            this.топ3СильнейшихToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.топ3СильнейшихToolStripMenuItem.Text = "Топ-3 сильнейших";
            this.топ3СильнейшихToolStripMenuItem.Click += new System.EventHandler(this.топ3СильнейшихToolStripMenuItem_Click);
            // 
            // показатьВсеРасыToolStripMenuItem
            // 
            this.показатьВсеРасыToolStripMenuItem.Name = "показатьВсеРасыToolStripMenuItem";
            this.показатьВсеРасыToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.показатьВсеРасыToolStripMenuItem.Text = "Показать все расы";
            this.показатьВсеРасыToolStripMenuItem.Click += new System.EventHandler(this.показатьВсеРасыToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Управление героями";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
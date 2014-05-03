namespace VampireGame
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonHomes = new System.Windows.Forms.Button();
            this.buttonOffice = new System.Windows.Forms.Button();
            this.buttonHospital = new System.Windows.Forms.Button();
            this.buttonShop = new System.Windows.Forms.Button();
            this.buttonSchool = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelHome = new System.Windows.Forms.Label();
            this.labelSchool = new System.Windows.Forms.Label();
            this.labelShop = new System.Windows.Forms.Label();
            this.labelOffice = new System.Windows.Forms.Label();
            this.labelHospital = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonPause = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonHomes
            // 
            this.buttonHomes.Location = new System.Drawing.Point(49, 36);
            this.buttonHomes.Name = "buttonHomes";
            this.buttonHomes.Size = new System.Drawing.Size(75, 23);
            this.buttonHomes.TabIndex = 0;
            this.buttonHomes.Text = "Homes";
            this.buttonHomes.UseVisualStyleBackColor = true;
            // 
            // buttonOffice
            // 
            this.buttonOffice.Location = new System.Drawing.Point(30, 203);
            this.buttonOffice.Name = "buttonOffice";
            this.buttonOffice.Size = new System.Drawing.Size(75, 23);
            this.buttonOffice.TabIndex = 1;
            this.buttonOffice.Text = "Offices";
            this.buttonOffice.UseVisualStyleBackColor = true;
            // 
            // buttonHospital
            // 
            this.buttonHospital.Location = new System.Drawing.Point(202, 229);
            this.buttonHospital.Name = "buttonHospital";
            this.buttonHospital.Size = new System.Drawing.Size(75, 23);
            this.buttonHospital.TabIndex = 2;
            this.buttonHospital.Text = "Hospital";
            this.buttonHospital.UseVisualStyleBackColor = true;
            this.buttonHospital.Click += new System.EventHandler(this.buttonHospital_Click);
            // 
            // buttonShop
            // 
            this.buttonShop.Location = new System.Drawing.Point(154, 122);
            this.buttonShop.Name = "buttonShop";
            this.buttonShop.Size = new System.Drawing.Size(75, 23);
            this.buttonShop.TabIndex = 3;
            this.buttonShop.Text = "Shops";
            this.buttonShop.UseVisualStyleBackColor = true;
            // 
            // buttonSchool
            // 
            this.buttonSchool.Location = new System.Drawing.Point(202, 36);
            this.buttonSchool.Name = "buttonSchool";
            this.buttonSchool.Size = new System.Drawing.Size(75, 23);
            this.buttonSchool.TabIndex = 4;
            this.buttonSchool.Text = "School";
            this.buttonSchool.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelHome
            // 
            this.labelHome.AutoSize = true;
            this.labelHome.Location = new System.Drawing.Point(69, 62);
            this.labelHome.Name = "labelHome";
            this.labelHome.Size = new System.Drawing.Size(35, 13);
            this.labelHome.TabIndex = 5;
            this.labelHome.Text = "label1";
            // 
            // labelSchool
            // 
            this.labelSchool.AutoSize = true;
            this.labelSchool.Location = new System.Drawing.Point(222, 62);
            this.labelSchool.Name = "labelSchool";
            this.labelSchool.Size = new System.Drawing.Size(35, 13);
            this.labelSchool.TabIndex = 6;
            this.labelSchool.Text = "label1";
            // 
            // labelShop
            // 
            this.labelShop.AutoSize = true;
            this.labelShop.Location = new System.Drawing.Point(174, 148);
            this.labelShop.Name = "labelShop";
            this.labelShop.Size = new System.Drawing.Size(35, 13);
            this.labelShop.TabIndex = 7;
            this.labelShop.Text = "label1";
            // 
            // labelOffice
            // 
            this.labelOffice.AutoSize = true;
            this.labelOffice.Location = new System.Drawing.Point(50, 229);
            this.labelOffice.Name = "labelOffice";
            this.labelOffice.Size = new System.Drawing.Size(35, 13);
            this.labelOffice.TabIndex = 8;
            this.labelOffice.Text = "label1";
            // 
            // labelHospital
            // 
            this.labelHospital.AutoSize = true;
            this.labelHospital.Location = new System.Drawing.Point(222, 255);
            this.labelHospital.Name = "labelHospital";
            this.labelHospital.Size = new System.Drawing.Size(35, 13);
            this.labelHospital.TabIndex = 9;
            this.labelHospital.Text = "label1";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(13, 301);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(72, 20);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(91, 301);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 11;
            this.buttonPause.Text = "Pause/Play";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listView1.Location = new System.Drawing.Point(339, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(259, 349);
            this.listView1.TabIndex = 12;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Surname";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Forename";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Opinion";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Morning";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Evening";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 349);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelHospital);
            this.Controls.Add(this.labelOffice);
            this.Controls.Add(this.labelShop);
            this.Controls.Add(this.labelSchool);
            this.Controls.Add(this.labelHome);
            this.Controls.Add(this.buttonSchool);
            this.Controls.Add(this.buttonShop);
            this.Controls.Add(this.buttonHospital);
            this.Controls.Add(this.buttonOffice);
            this.Controls.Add(this.buttonHomes);
            this.Name = "GameForm";
            this.Text = "GameForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonHomes;
        private System.Windows.Forms.Button buttonOffice;
        private System.Windows.Forms.Button buttonHospital;
        private System.Windows.Forms.Button buttonShop;
        private System.Windows.Forms.Button buttonSchool;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelHome;
        private System.Windows.Forms.Label labelSchool;
        private System.Windows.Forms.Label labelShop;
        private System.Windows.Forms.Label labelOffice;
        private System.Windows.Forms.Label labelHospital;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}
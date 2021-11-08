
namespace ReversiUI
{
    partial class SettingsForm
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
            this.levelUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.heuristicsComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.levelUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // levelUpDown
            // 
            this.levelUpDown.Location = new System.Drawing.Point(290, 42);
            this.levelUpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.levelUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.levelUpDown.Name = "levelUpDown";
            this.levelUpDown.Size = new System.Drawing.Size(60, 26);
            this.levelUpDown.TabIndex = 0;
            this.levelUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.levelUpDown.ValueChanged += new System.EventHandler(this.levelUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Уровень";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Чёрные",
            "Белые"});
            this.comboBox1.Location = new System.Drawing.Point(206, 127);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 28);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Кем играть ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Эвристика";
            // 
            // heuristicsComboBox
            // 
            this.heuristicsComboBox.FormattingEnabled = true;
            this.heuristicsComboBox.Items.AddRange(new object[] {
            "Мобильность",
            "Стабильность",
            "Углы",
            "Статические веса",
            "Количество фишек",
            "Мобильность и стабильность"});
            this.heuristicsComboBox.Location = new System.Drawing.Point(125, 208);
            this.heuristicsComboBox.Name = "heuristicsComboBox";
            this.heuristicsComboBox.Size = new System.Drawing.Size(270, 28);
            this.heuristicsComboBox.TabIndex = 5;
            this.heuristicsComboBox.SelectedIndexChanged += new System.EventHandler(this.heuristicsComboBox_SelectedIndexChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 450);
            this.Controls.Add(this.heuristicsComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelUpDown);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.levelUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown levelUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox heuristicsComboBox;
    }
}
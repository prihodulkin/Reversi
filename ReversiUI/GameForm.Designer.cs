
namespace ReversiUI
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
            this.reversiField = new System.Windows.Forms.PictureBox();
            this.movingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.reversiField)).BeginInit();
            this.SuspendLayout();
            // 
            // reversiField
            // 
            this.reversiField.Location = new System.Drawing.Point(12, 3);
            this.reversiField.Name = "reversiField";
            this.reversiField.Size = new System.Drawing.Size(605, 605);
            this.reversiField.TabIndex = 0;
            this.reversiField.TabStop = false;
            this.reversiField.Click += new System.EventHandler(this.reversiField_Click);
            this.reversiField.Paint += new System.Windows.Forms.PaintEventHandler(this.reversiField_Paint);
            this.reversiField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.reversiField_MouseClick);
            // 
            // movingLabel
            // 
            this.movingLabel.AutoSize = true;
            this.movingLabel.Location = new System.Drawing.Point(640, 20);
            this.movingLabel.Name = "movingLabel";
            this.movingLabel.Size = new System.Drawing.Size(118, 20);
            this.movingLabel.TabIndex = 1;
            this.movingLabel.Text = "Ходят чёрные";
            this.movingLabel.Click += new System.EventHandler(this.movingLabel_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 639);
            this.Controls.Add(this.movingLabel);
            this.Controls.Add(this.reversiField);
            this.Name = "GameForm";
            this.Text = "GameForm";
            ((System.ComponentModel.ISupportInitialize)(this.reversiField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox reversiField;
        private System.Windows.Forms.Label movingLabel;
    }
}
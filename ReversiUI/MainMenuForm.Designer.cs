
namespace ReversiUI
{
    partial class MainMenuForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.twoPlayersButton = new System.Windows.Forms.Button();
            this.vsBotButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // twoPlayersButton
            // 
            this.twoPlayersButton.Location = new System.Drawing.Point(12, 17);
            this.twoPlayersButton.Name = "twoPlayersButton";
            this.twoPlayersButton.Size = new System.Drawing.Size(327, 69);
            this.twoPlayersButton.TabIndex = 0;
            this.twoPlayersButton.Text = "Два игрока";
            this.twoPlayersButton.UseVisualStyleBackColor = true;
            this.twoPlayersButton.Click += new System.EventHandler(this.twoPlayersButton_Click);
            // 
            // vsBotButton
            // 
            this.vsBotButton.Location = new System.Drawing.Point(12, 109);
            this.vsBotButton.Name = "vsBotButton";
            this.vsBotButton.Size = new System.Drawing.Size(327, 69);
            this.vsBotButton.TabIndex = 1;
            this.vsBotButton.Text = "Против бота";
            this.vsBotButton.UseVisualStyleBackColor = true;
            this.vsBotButton.Click += new System.EventHandler(this.vsBotButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(12, 205);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(327, 69);
            this.settingsButton.TabIndex = 2;
            this.settingsButton.Text = "Настройки";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 450);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.vsBotButton);
            this.Controls.Add(this.twoPlayersButton);
            this.Name = "MainMenuForm";
            this.Text = "Меню";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button twoPlayersButton;
        private System.Windows.Forms.Button vsBotButton;
        private System.Windows.Forms.Button settingsButton;
    }
}


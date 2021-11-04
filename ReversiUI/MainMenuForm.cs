using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReversiUI
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void twoPlayersButton_Click(object sender, EventArgs e)
        {
            new GameForm(GameMode.TwoPlayers).Show();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }

        private void vsBotButton_Click(object sender, EventArgs e)
        {
            new GameForm(GameMode.PlayerVsBot).Show();
        }
    }
}

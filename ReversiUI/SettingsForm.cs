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
    public partial class SettingsForm : Form
    {
        Settings settings;
        public SettingsForm()
        {
            settings = Settings.GetInstance();
            InitializeComponent();
            levelUpDown.Value = settings.Level;
            comboBox1.SelectedIndex = settings.User == Reversi.Player.Black ? 0 : 1;
        }

        private void levelUpDown_ValueChanged(object sender, EventArgs e)
        {
           var level =(int)((NumericUpDown)sender).Value;
           settings.Level = level;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           switch( ((ComboBox)sender).SelectedItem)
            {
                case "Чёрные":
                    settings.User = Reversi.Player.Black;
                    break;
                case "Белые":
                    settings.User = Reversi.Player.White;
                    break;
            }
        }

        private void heuristicsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).SelectedItem)
            {
                case "Углы":
                    settings.Heuristics = Reversi.HeuristicsEnum.CornersCount;
                    break;
                case "Мобильность":
                    settings.Heuristics = Reversi.HeuristicsEnum.Mobility;
                    break;
                case "Стабильность":
                    settings.Heuristics = Reversi.HeuristicsEnum.Stability;
                    break;
                case "Статические веса":
                    settings.Heuristics = Reversi.HeuristicsEnum.StaticWeights;
                    break;
                case "Количество фишек":
                    settings.Heuristics = Reversi.HeuristicsEnum.CoinPartly;
                    break;
                case "Мобильность и стабильность":
                    settings.Heuristics = Reversi.HeuristicsEnum.MobilityAndStability;
                    break;
            }
        }
    }
}

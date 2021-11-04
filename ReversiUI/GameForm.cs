using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reversi;

namespace ReversiUI
{
    public enum GameMode
    {
        TwoPlayers,
        PlayerVsBot
    }
    public partial class GameForm : Form
    {
        GameMode mode;
        Stack<GamePosition> gamePositions;
        public GameForm(GameMode mode)
        {
            this.mode = mode;
            gamePositions = new Stack<GamePosition>();
            gamePositions.Push(new GamePosition());
            InitializeComponent();
        }

        private void reversiField_Click(object sender, EventArgs e)
        {
            
        }

        private void reversiField_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush);
            Graphics g = e.Graphics;
            var width = ((sender as PictureBox).Width-5)/8*8;
            var height = ((sender as PictureBox).Height-5)/8*8;
            var wStep = width / 8;
            var hStep = height / 8;
            var x = 0;
            var y = 0;  
            for(int i = 0; i < 9; i++)
            {
                g.DrawLine(pen, new Point(x,0), new Point(x, height));
                g.DrawLine(pen, new Point(0, y), new Point(width, y));
                x += wStep;
                y += hStep;
            }
            var currentPosition = gamePositions.Peek();
            foreach(var piece in currentPosition.BlackPieces)
            {
                var rect = new Rectangle(piece.X * wStep + wStep / 10,
                                  piece.Y * hStep + hStep / 10,
                                  wStep / 5 * 4,
                                  hStep / 5 * 4);
                g.FillEllipse(brush, rect);
            }

            foreach (var piece in currentPosition.WhitePieces)
            {
                var rect = new Rectangle(piece.X * wStep + wStep / 10,
                                  piece.Y * hStep + hStep / 10,
                                  wStep / 5 * 4,
                                  hStep / 5 * 4);
                g.DrawEllipse(pen, rect);       
            }
            foreach(var s in currentPosition.PossibleStepsSquares)
            {
                SolidBrush pBrush = new SolidBrush(Color.Aqua);
                g.FillRectangle(pBrush, s.X*wStep+1, s.Y*hStep+1, wStep-1, hStep-1);
            }
            movingLabel.Text = currentPosition.Player == Player.Black ? "Ходят чёрные" : "Ходят белые";
        }

        private void reversiField_MouseClick(object sender, MouseEventArgs e)
        {
            var width = ((sender as PictureBox).Width - 5) / 8 * 8;
            var height = ((sender as PictureBox).Height - 5) / 8 * 8;
            var wStep = width / 8;
            var hStep = height / 8;
            var currentPosition = gamePositions.Peek();
            Square square = new Square(e.X/wStep, e.Y/hStep);
            try
            {
                gamePositions.Push(currentPosition.MakeStep(square));
                Refresh();
            } catch(ArgumentException) 
            {
                MessageBox.Show("Выбрана неправильная клетка для хода");
            }
        }

        private void movingLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

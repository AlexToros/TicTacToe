using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;
using System.Windows.Forms;

namespace TicTacToeClient
{
    public partial class TicTacToeField : UserControl
    {
        private Game Game;
        private FieldCell[,] cells;
        public TicTacToeField()
        {
            InitializeComponent();
            cells = new FieldCell[3, 3];
            PictureBox[] pictureBoxes = this.tableLayoutPanel1.Controls.OfType<PictureBox>().ToArray();
            for (int i = 0; i < pictureBoxes.Length; i++)
            {
                FieldCell cell = new FieldCell(pictureBoxes[i])
                {
                    Row = i / 3,
                    Col = i % 3
                };
                cells[i / 3, i % 3] = cell;
                cell.OnClick += Cell_OnClick;
            }
        }
        
        public void Build(Game game)
        {
            Game = game;
            Game.OnNewTurn += ProcessTurn;
            Game.OnGameOver += Game_OnGameOver;
        }

        private void Game_OnGameOver(bool IamWiner)
        {
            MessageBox.Show(IamWiner ? "Вы победили!" : "Вы проиграли!");
        }

        private void Cell_OnClick(int row, int col)
        {
            Game?.MyTurn(row, col);
        }
        
        private void ProcessTurn(Symbol symbol, int row, int col)
        {
            cells[row, col].Box.Image = symbol == Symbol.Circle ? Properties.Resources.Circle : Properties.Resources.Cross;
        }

    }
    public class FieldCell
    {
        public FieldCell(PictureBox pictureBox)
        {
            Box = pictureBox;
            pictureBox.Click += Box_Click;
        }

        private void Box_Click(object sender, EventArgs e)
        {
            OnClick?.Invoke(Row, Col);
        }

        public PictureBox Box { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public delegate void Click(int row, int col);
        public event Click OnClick;
    }
}

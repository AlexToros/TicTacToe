using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;

namespace TicTacToeClient
{
    public class Game
    {
        public Symbol[,] Field { get; private set; }

        public uint ID { get; private set; }
        public Symbol MySymbol { get; set; }
        public Player Opponent { get; set; }

        private Symbol OppenentSymbol { get => Opponent.Symbol; }

        public delegate void NewTurn(Symbol Field, int row, int col);
        public event NewTurn OnNewTurn;

        public delegate void GameOver(bool IamWiner);
        public event GameOver OnGameOver;

        public Game(uint GameID)
        {
            Field = new Symbol[3, 3];
            ID = GameID;
        }

        internal void OpponentTurn(int row, int col)
        {
            Field[row, col] = OppenentSymbol;
            OnNewTurn?.Invoke(Field[row, col], row, col);
        }

        internal void MyTurn(int row, int col)
        {
            Field[row, col] = MySymbol;
            OnNewTurn?.Invoke(Field[row, col], row, col);
        }

        internal void GameIsOver(bool IamWinner) => OnGameOver?.Invoke(IamWinner);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;

namespace TicTacToeServer
{
    class Game
    {
        static object locker = new object();
        static uint ID_Counter = 1;

        public Player CrossPlayer { get; set; }
        public Player CirclePlayer { get; set; }
        public uint ID { get; private set; }
        public bool IsOver { get; private set; }

        private Symbol[,] Field = new Symbol[3, 3];
        private Player TurnOwner;

        public Game(Player crossPlayer, Player circlePlayer)
        {
            lock (locker)
            {
                ID = ID_Counter++;
            }

            CrossPlayer = crossPlayer;
            CirclePlayer = circlePlayer;

            crossPlayer.Game = this;
            circlePlayer.Game = this;

            crossPlayer.Symbol = Symbol.Cross;
            circlePlayer.Symbol = Symbol.Circle;

            TurnOwner = crossPlayer;
        }

        internal Player Turn(int rowINDX, int colINDX)
        {
            Field[rowINDX, colINDX] = TurnOwner.Symbol;
            CheckField(TurnOwner, rowINDX, colINDX);
            if (TurnOwner.Symbol == Symbol.Cross)
                TurnOwner = CirclePlayer;
            else
                TurnOwner = CrossPlayer;

            return TurnOwner;
        }

        private void CheckField(Player currentPlayer, int rowINDX, int colINDX)
        {
            bool IsEnd = false;
            for (int i = 0; i < Field.GetLength(0); i++)
            {
                for (int j = 0; j < Field.GetLength(1); j++)
                {
                    if (Field[i, j] != currentPlayer.Symbol)
                    {
                        IsEnd = false;
                        break;
                    }
                    else
                        IsEnd = true;
                }
                if (IsEnd) break;
            }
            if (!IsEnd)
            {
                for (int i = 0; i < Field.GetLength(0); i++)
                {
                    for (int j = 0; j < Field.GetLength(1); j++)
                    {
                        if (Field[j, i] != currentPlayer.Symbol)
                        {
                            IsEnd = false;
                            break;
                        }
                        else
                            IsEnd = true;
                    }
                    if (IsEnd) break;
                }
            }
            if (!IsEnd)
            {
                for (int i = 0; i < Math.Min(Field.GetLength(0), Field.GetLength(1)); i++)
                {
                    if (Field[i, i] != currentPlayer.Symbol)
                    {
                        IsEnd = false;
                        break;
                    }
                    else
                        IsEnd = true;
                }
            }
            if (!IsEnd)
            {
                for (int i = 0; i < Math.Min(Field.GetLength(0), Field.GetLength(1)); i++)
                {
                    if (Field[i, Field.GetLength(1) - i - 1] != currentPlayer.Symbol)
                    {
                        IsEnd = false;
                        break;
                    }
                    else
                        IsEnd = true;
                }
            }
            if (IsEnd)
            {
                CrossPlayer.Winner = currentPlayer.Symbol == Symbol.Cross;
                CirclePlayer.Winner = currentPlayer.Symbol == Symbol.Circle;
                IsOver = IsEnd;
            }
        }
        
    }
}

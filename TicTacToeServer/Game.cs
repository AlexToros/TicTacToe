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
            if (TurnOwner.Symbol == Symbol.Cross)
                TurnOwner = CirclePlayer;
            else
                TurnOwner = CrossPlayer;

            return TurnOwner;
        }
    }
}

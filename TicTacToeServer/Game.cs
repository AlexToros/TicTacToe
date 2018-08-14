using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}

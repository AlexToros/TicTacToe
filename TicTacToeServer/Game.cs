using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeServer
{
    class Game
    {
        public Player CrossPlayer { get; set; }
        public Player CirclePlayer { get; set; }

        private Symbol[,] Field = new Symbol[3, 3]; 

        public Game(Player crossPlayer, Player circlePlayer)
        {
            CrossPlayer = crossPlayer;
            CirclePlayer = circlePlayer;

            crossPlayer.Game = this;
            circlePlayer.Game = this;

            crossPlayer.Symbol = Symbol.Cross;
            circlePlayer.Symbol = Symbol.Circle;
        }
    }
}

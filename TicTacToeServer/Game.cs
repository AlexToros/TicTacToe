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

        public Game(Player crossPlayer, Player circlePlayer)
        {
            CrossPlayer = crossPlayer;
            CirclePlayer = circlePlayer;

            crossPlayer.Game = this;
            circlePlayer.Game = this;
        }
    }
}

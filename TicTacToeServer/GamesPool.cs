using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeServer
{
    class GamesPool : List<Game>
    {
        public GamesPool() : base()
        {

        }

        public Game GetGame(UInt32 ID)
        {
            return this.Single(g => g.ID == ID);
        }
    }
}

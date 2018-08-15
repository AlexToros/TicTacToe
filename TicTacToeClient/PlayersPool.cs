using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeClient
{
    class PlayersPool : List<Player>
    {
        public PlayersPool() : base() { }

        public void NewPlayer(uint ID, string Name)
        {
            this.Add(new Player(ID, Name));
        }

        public Player GetPlayer(uint ID)
        {
            return this.Single(pl => pl.ID == ID);
        }
    }
}

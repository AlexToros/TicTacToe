using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeServer
{
    class PlayersPool
    {
        private List<Player> players;
        public delegate void NotifyPlayers(List<Player> players);
        public event NotifyPlayers OnPlayersChanged;

        public PlayersPool()
        {
            players = new List<Player>();
        }

        public void New(Player player)
        {
            OnPlayersChanged += player.SendNewPlayerList;
            players.Add(player);
            OnPlayersChanged.Invoke(players);
        }
    }
}

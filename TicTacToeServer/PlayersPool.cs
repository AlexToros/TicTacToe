using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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
            Thread thread = new Thread(DisconnectHandler);
            thread.Start();
        }

        public void New(Player player)
        {
            OnPlayersChanged += player.SendNewPlayerList;
            players.Add(player);
            OnPlayersChanged.Invoke(players);
        }

        public Player GetPlayer(uint PlayerId)
        {
            return players.Single(p => p.ID == PlayerId);
        }

        private void DisconnectHandler()
        {
            while (true)
            {
                Thread.Sleep(2000);
                for (int i = 0; i < players.Count; i++)
                {
                    if (!players[i].client.Connected)
                    {
                        players.RemoveAt(i--);
                        OnPlayersChanged?.Invoke(players);
                    }
                }
            }
        }

    }
}

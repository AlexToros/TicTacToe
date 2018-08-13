using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TicTacToeLibrary;

namespace TicTacToeServer
{
    class Player
    {
        static object locker = new object();
        static uint ID_Counter = 1;

        private TcpClient client;

        public uint ID { get; private set; }
        public string Name { get; set; }
        
        public Player(TcpClient Client)
        {
            lock (locker)
            {
                ID = ID_Counter++;
            }
            client = Client;
        }

        public void Process()
        {
            lock (locker)
            {
                NetworkStream stream = client.GetStream();
            }
        }

        public void SendNewPlayerList(List<Player> players)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].ID != this.ID)
                {
                    sb.Append(players[i].ID.ToString() + ";" + players[i].Name);
                    if (i != players.Count - 1) sb.Append("|");
                }
            }
            string message = sb.ToString();
            byte[] send = Encoding.UTF8.GetBytes(message);
            lock (locker)
            {
                NetworkStream stream = client.GetStream();

                stream.WriteByte((byte)Commands.NEW_PLAYER_LIST);
                stream.Write(send, 0, send.Length);
            }
        }
    }
}

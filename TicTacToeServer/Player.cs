using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TicTacToeLibrary;
using System.IO;

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
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(players.Count - 1);
            
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].ID != this.ID)
                {
                    writer.Write(players[i].ID);
                    writer.Write(players[i].Name);
                }
            }
        }
    }
}

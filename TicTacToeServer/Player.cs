using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TicTacToeServer
{
    class Player
    {
        private TcpClient client;
        public Player(TcpClient Client)
        {
            client = Client;
        }

        public void Process()
        {
            NetworkStream stream = client.GetStream();
        }
    }
}

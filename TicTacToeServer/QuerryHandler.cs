using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using TicTacToeLibrary;
using System.IO;

namespace TicTacToeServer
{
    class QuerryHandler
    {
        private PlayersPool ServerPlayers;

        public QuerryHandler(PlayersPool playersPool)
        {
            ServerPlayers = playersPool;
        }

        public void ClientQuerryHandler(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            BinaryReader reader = new BinaryReader(tcpClient.GetStream());
            try
            {
                bool exit = false;
                while (!exit)
                {
                    Commands command = (Commands)reader.ReadByte();
                    switch (command)
                    {
                        case Commands.INVITE:
                            break;
                        case Commands.NEW_PLAYER_LIST:
                            ProcessNewPlayer(tcpClient);
                            break;
                        case Commands.DISCONNECT:
                            exit = DisconnectPlayer(tcpClient);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString()); }

        }

        private void ProcessNewPlayer(TcpClient client)
        {
            BinaryReader reader = new BinaryReader(client.GetStream());
            string Name = reader.ReadString();
            Console.WriteLine(Name + " Подключен");
            Player player = new Player(client);
            player.Name = Name;
            ServerPlayers.New(player);
        }
        private bool DisconnectPlayer(TcpClient client)
        {
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
            return true;
        }


    }
}

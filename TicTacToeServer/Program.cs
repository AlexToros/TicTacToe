using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using TicTacToeLibrary;

namespace TicTacToeServer
{
    class Program
    {
        static TcpListener listener;
        static PlayersPool ServerPlayers;
        static void Main(string[] args)
        {
            try
            {
                ServerPlayers = new PlayersPool();
                listener = new TcpListener(IPAddress.Parse(Options.Addres), Options.Port);
                listener.Start();
                Console.WriteLine("Сервер стартовал. Ожидание игроков.");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Player player = new Player(client);
                    ServerPlayers.OnPlayersChanged += player.SendNewPlayerList;
                    ServerPlayers.New(player);

                    Thread clientThread = new Thread(player.Process);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                listener?.Stop();
            }
        }
    }
}

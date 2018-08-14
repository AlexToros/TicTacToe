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
    class Program
    {
        static TcpListener listener;
        static PlayersPool ServerPlayers;
        static GamesPool OpenGames;
        static QuerryHandler handler;
        static void Main(string[] args)
        {
            Game g = new Game(null, null);
            
            try
            {
                ServerPlayers = new PlayersPool();
                OpenGames = new GamesPool();
                handler = new QuerryHandler(ServerPlayers, OpenGames);
                listener = new TcpListener(IPAddress.Parse(Options.Addres), Options.Port);
                listener.Start();
                Console.WriteLine("Сервер стартовал. Ожидание игроков.");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Было подключение");
                    Thread clientThread = new Thread(new ParameterizedThreadStart(handler.ClientQuerryHandler));
                    clientThread.Start(client);
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

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
                    Console.WriteLine("Было подключение");
                    Thread clientThread = new Thread(new ParameterizedThreadStart(ProcessConnection));
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
        static void ProcessConnection(object client)
        {
            NetworkStream stream = ((TcpClient)client).GetStream();

            byte firstByte = (byte)stream.ReadByte();
            Commands InputCommand = (Commands)firstByte;
        }
    }
}

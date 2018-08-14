﻿using System;
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
        private GamesPool OpenGames;

        public QuerryHandler(PlayersPool playersPool, GamesPool gamesPool)
        {
            ServerPlayers = playersPool;
            OpenGames = gamesPool;
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
                            ProcessPlayerInvite(tcpClient);
                            break;
                        case Commands.ACCEPT_INVITE:
                            ProcessAccepting(tcpClient);
                            break;
                        case Commands.DENIED_INVITE:
                            ProcessDenied(tcpClient);
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

        private void ProcessDenied(TcpClient tcpClient)
        {
            BinaryReader reader = new BinaryReader(tcpClient.GetStream());
            uint InviterID = reader.ReadUInt32();
            Player Inviter = ServerPlayers.GetPlayer(InviterID);
            BinaryWriter writer = new BinaryWriter(Inviter.client.GetStream());
            writer.Write((byte)Commands.DENIED_INVITE);
        }

        private void ProcessAccepting(TcpClient tcpClient)
        {
            BinaryReader reader = new BinaryReader(tcpClient.GetStream());
            uint InviterID = reader.ReadUInt32();
            uint AcceptorID = reader.ReadUInt32();
            Player first = ServerPlayers.GetPlayer(InviterID);
            Player second = ServerPlayers.GetPlayer(AcceptorID);
            Game newGame = new Game(first, second);
            OpenGames.Add(newGame);
            BinaryWriter writer = new BinaryWriter(tcpClient.GetStream());
            writer.Write((byte)Commands.ACCEPT_INVITE);
            writer = new BinaryWriter(first.client.GetStream());
            writer.Write((byte)Commands.ACCEPT_INVITE);
        }

        private void ProcessPlayerInvite(TcpClient client)
        {
            BinaryReader reader = new BinaryReader(client.GetStream());
            uint InvitorPlayerId = reader.ReadUInt32();
            uint targetPlayerId = reader.ReadUInt32();
            Player invitorPl = ServerPlayers.GetPlayer(InvitorPlayerId);
            TcpClient targetClient = ServerPlayers.GetPlayer(targetPlayerId).client;
            BinaryWriter writer = new BinaryWriter(targetClient.GetStream());
            writer.Write((byte)Commands.INVITE);
            writer.Write(InvitorPlayerId);
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

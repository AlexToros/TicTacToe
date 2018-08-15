using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using TicTacToeLibrary;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TicTacToeClient
{
    class ServerConnector : INotifyPropertyChanged
    {
        private bool _isMyTurn;
        private bool _isDisconnect;

        public bool IsMyTurn
        {
            get => _isMyTurn;
            private set
            {
                _isMyTurn = value;
                OnChangeTurn?.Invoke(_isMyTurn);
            }
        }
        public bool IsDisconnect
        {
            get => _isDisconnect;

            set
            {
                _isDisconnect = value;
                NotifyPropertyChanged();
            }
        }
        public string MyName { get; private set; }
        public uint MyID { get; private set; }
        public Game CurrentGame { get; private set; }
        public PlayersPool PlayersOnline { get; set; }

        public delegate void NewPlayers(PlayersPool NewList);
        public event NewPlayers OnNewPlayers;

        public delegate void StartNewGame(Game NewGame);
        public event StartNewGame NewGameStarted;

        public delegate void ChangeTurn(bool val);
        public event ChangeTurn OnChangeTurn;

        private TcpClient Client { get; set; }

        public ServerConnector(string userName)
        {
            MyName = userName;
            IsMyTurn = false;
            PlayersOnline = new PlayersPool();

            Connect();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void Disconnect()
        {
            NetworkStream stream = Client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.DISCONNECT);
        }

        public void InvitePlayer(uint PlayerId)
        {
            NetworkStream stream = Client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.INVITE);
            writer.Write(MyID);
            writer.Write(PlayerId);
        }

        public void MyTurn(int row, int col)
        {
            CurrentGame.MyTurn(row, col);
            NetworkStream stream = Client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.TURN);
            writer.Write(CurrentGame.ID);
            writer.Write(row);
            writer.Write(col);
            IsMyTurn = false;
        }

        private void Connect()
        {
            Client = new TcpClient(Options.Addres, Options.Port);
            NetworkStream stream = Client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.NEW_PLAYER_LIST);
            writer.Write(MyName);
            Thread thread = new Thread(ServerHandling);
            thread.Start();
        }

        private void ServerHandling()
        {
            BinaryReader reader = new BinaryReader(Client.GetStream());
            while (true)
            {
                Commands command = (Commands)reader.ReadByte();
                switch (command)
                {
                    case Commands.INVITE:
                        ProcessPlayerInvite();
                        break;
                    case Commands.NEW_PLAYER_LIST:
                        GetPlayersList();
                        break;
                    case Commands.DISCONNECT:
                        break;
                    case Commands.ACCEPT_INVITE:
                        ProcessAccept();
                        break;
                    case Commands.DENIED_INVITE:
                        ProcessDenied();
                        break;
                    case Commands.TURN:
                        ProcessTurn();
                        break;
                    case Commands.GAME_OVER:
                        ProcessGameOver();
                        break;
                    case Commands.PLAYER_ID:
                        GetMyId();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ProcessGameOver()
        {
            NetworkStream stream = Client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            bool IamWinner = reader.ReadBoolean();
            CurrentGame.GameIsOver(IamWinner);
        }

        private void ProcessTurn()
        {
            NetworkStream stream = Client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            int row = reader.ReadInt32();
            int col = reader.ReadInt32();
            CurrentGame.OpponentTurn(row, col);
            IsMyTurn = true;
        }

        private void ProcessAccept()
        {
            BinaryReader reader = new BinaryReader(Client.GetStream());
            uint GameID = reader.ReadUInt32();
            uint OpponentID = reader.ReadUInt32();
            IsMyTurn = reader.ReadBoolean();
            CurrentGame = new Game(GameID)
            {
                MySymbol = IsMyTurn ? Symbol.Cross : Symbol.Circle,
                Opponent = PlayersOnline.GetPlayer(OpponentID)
            };
            CurrentGame.Opponent.Symbol = IsMyTurn ? Symbol.Circle : Symbol.Cross;
            NewGameStarted?.Invoke(CurrentGame);
            MessageBox.Show("Игра готова!");
        }

        private void ProcessDenied()
        {
            MessageBox.Show("Игрок отклонил приглашение!");
        }

        private void ProcessPlayerInvite()
        {
            NetworkStream stream = Client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            uint InviterID = reader.ReadUInt32();
            Player candidate = PlayersOnline.GetPlayer(InviterID);
            DialogResult res = MessageBox.Show("Вас пригласил на игру " + candidate.Name, "Внимание!", MessageBoxButtons.OKCancel);
            BinaryWriter writer = new BinaryWriter(stream);
            if (res == DialogResult.OK)
            {
                writer.Write((byte)Commands.ACCEPT_INVITE);
                writer.Write(InviterID);
                writer.Write(MyID);
            }
            else
            {
                writer.Write((byte)Commands.DENIED_INVITE);
                writer.Write(InviterID);
            }
        }

        private void GetPlayersList()
        {
            BinaryReader reader = new BinaryReader(Client.GetStream());
            int playerscount = reader.ReadInt32();
            PlayersOnline.Clear();
            for (int i = 0; i < playerscount; i++)
                PlayersOnline.NewPlayer(reader.ReadUInt32(), reader.ReadString());
            OnNewPlayers?.Invoke(PlayersOnline);
        }

        private void GetMyId()
        {
            BinaryReader reader = new BinaryReader(Client.GetStream());
            MyID = reader.ReadUInt32();
        }
    }
}

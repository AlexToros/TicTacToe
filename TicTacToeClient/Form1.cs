using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using TicTacToeLibrary;
using System.IO;
using System.Runtime.CompilerServices;

namespace TicTacToeClient
{
    public partial class Form1 : Form, INotifyPropertyChanged
    {
        Random rnd = new Random();
        TcpClient client;
        private string _playerList;
        UInt32 GameID;
        Symbol symbol;
        private bool _isMyTurn;

        public bool IsMyTurn
        {
            get => _isMyTurn;
            set
            {
                _isMyTurn = value;
                NotifyPropChanged();
            }
        }
        public string PlayerList
        {
            get => _playerList; set
            {
                _playerList = value;
                NotifyPropChanged();
            }
        }
        public Form1()
        {
            InitializeComponent();
            symbol = Symbol.Cross;
            IsMyTurn = true;
            label1.DataBindings.Add("Text", this, "PlayerList", false, DataSourceUpdateMode.OnPropertyChanged);
            textBox1.DataBindings.Add("Enabled", this, "IsMyTurn", false, DataSourceUpdateMode.OnPropertyChanged);
            textBox2.DataBindings.Add("Enabled", this, "IsMyTurn", false, DataSourceUpdateMode.OnPropertyChanged);
            button2.DataBindings.Add("Enabled", this, "IsMyTurn", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new TcpClient(Options.Addres, Options.Port);
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.NEW_PLAYER_LIST);
            writer.Write("USER" + rnd.Next(100));
            Thread ServerListner = new Thread(new ThreadStart(ServerHandler));
            ServerListner.Start();
            this.Text = rnd.Next(0, 10000).ToString();
        }
        private void ServerHandler()
        {
            while (true)
            {
                BinaryReader reader = new BinaryReader(client.GetStream());
                Commands command = (Commands)reader.ReadByte();
                switch (command)
                {
                    case Commands.GAME_OVER:
                        ProcessGameOver();
                        break;
                    case Commands.TURN:
                        ProcessTurn();
                        break;
                    case Commands.INVITE:
                        ProcessPlayerInvite();
                        break;
                    case Commands.NEW_PLAYER_LIST:
                        int playerscount = reader.ReadInt32();
                        List<string> list = new List<string>();
                        for (int i = 0; i < playerscount; i++)
                        {
                            list.Add(reader.ReadInt32().ToString() + ";" + reader.ReadString());
                        }
                        PlayerList = String.Join("|", list);
                        break;
                    case Commands.ACCEPT_INVITE:
                        GameID = reader.ReadUInt32();
                        MessageBox.Show("Игра готова! ID игры - " + GameID.ToString());
                        break;
                    case Commands.DENIED_INVITE:
                        MessageBox.Show(Text + ", Игрок отклонил приглашение!");
                        break;
                    default:
                        break;
                }

            }
        }

        private void ProcessGameOver()
        {
            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            bool IamWinner = reader.ReadBoolean();
            string message = IamWinner ? "победили!" : "проиграли!";
            MessageBox.Show(Text + ", Вы " + message);
        }

        private void ProcessTurn()
        {
            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            string message = symbol == Symbol.Cross ? "Нолик" : "Крестик";
            int row = reader.ReadInt32();
            int col = reader.ReadInt32();
            MessageBox.Show("Оппонент поставил " + message + " на координаты " + (row + 1).ToString() + "," + (col + 1).ToString());
            IsMyTurn = true;
        }

        private void ProcessPlayerInvite()
        {
            NetworkStream stream = client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            uint InviterID = reader.ReadUInt32();
            DialogResult res = MessageBox.Show(this.Text + " Вас пригласил на игру игрок с ID " + InviterID.ToString(), "Внимание!", MessageBoxButtons.OKCancel);
            BinaryWriter writer = new BinaryWriter(stream);
            if (res == DialogResult.OK)
            {
                writer.Write((byte)Commands.ACCEPT_INVITE);
                writer.Write(InviterID);
                writer.Write((UInt32)2);
                symbol = Symbol.Circle;
                IsMyTurn = false;
            }
            else
            {
                writer.Write((byte)Commands.DENIED_INVITE);
                writer.Write(InviterID);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.DISCONNECT);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.INVITE);
            writer.Write((UInt32)1);
            writer.Write((UInt32)2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((byte)Commands.TURN);
            writer.Write(GameID);
            writer.Write(Convert.ToUInt32(textBox1.Text));
            writer.Write(Convert.ToUInt32(textBox2.Text));

            IsMyTurn = false;
        }
    }
}

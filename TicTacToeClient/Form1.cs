﻿using System;
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
            label1.DataBindings.Add("Text", this, "PlayerList", false, DataSourceUpdateMode.OnPropertyChanged);
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
            writer.Write("USER" + rnd.Next(100));
            Thread ServerListner = new Thread(new ThreadStart(ServerHandler));
            ServerListner.Start();
        }
        private void ServerHandler()
        {
            while (true)
            {
                BinaryReader reader = new BinaryReader(client.GetStream());
                int playerscount = reader.ReadInt32();
                List<string> list = new List<string>();
                for (int i = 0; i < playerscount; i++)
                {
                    list.Add(reader.ReadInt32().ToString() + ";" + reader.ReadString());
                }
                PlayerList = String.Join("|", list);
            }
        }
    }
}

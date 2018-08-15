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
    public partial class MainForm : Form
    {
        ServerConnector connector;
        Action<bool> helpAction;
        Action<PlayersPool> UpdateList;
        Action<Game> SetNewGame;
        public MainForm()
        {
            InitializeComponent();
            ticTacToeField1.Enabled = false;
            helpAction = new Action<bool>((x) => ticTacToeField1.Enabled = x);
            UpdateList = new Action<PlayersPool>((list) => {
                listBox1.DataSource = null;
                listBox1.DataSource = list;
            });
            SetNewGame = new Action<Game>((game) => ticTacToeField1.Build(game));
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                connector = new ServerConnector(textBox1.Text);
                connector.OnNewPlayers += Connector_OnNewPlayers;
                connector.NewGameStarted += Connector_NewGameStarted;
                connector.OnChangeTurn += Connector_OnChangeTurn;
            }
            else
            {
                MessageBox.Show("Введите имя игрока");
                return;
            }

            ConnectButton.DataBindings.Add("Enabled", connector, "IsDisconnect");
            textBox1.DataBindings.Add("Enabled", connector, "IsDisconnect");
        }
        
        private void Connector_OnChangeTurn(bool val)
        {
            helpAction.Invoke(val);
        }

        private void Connector_NewGameStarted(Game NewGame)
        {
            SetNewGame.Invoke(NewGame);
        }

        private void Connector_OnNewPlayers(PlayersPool NewList)
        {
            UpdateList.Invoke(NewList);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connector?.Disconnect();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Player selectedPlayer = (Player)listBox1.SelectedItem;
            connector.InvitePlayer(selectedPlayer.ID);
        }
    }
}

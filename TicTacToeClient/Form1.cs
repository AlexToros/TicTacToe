using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using TicTacToeLibrary;

namespace TicTacToeClient
{
    public partial class Form1 : Form
    {
        
        public string PlayerList { get; set; }
        public Form1()
        {
            InitializeComponent();
            label1.DataBindings.Add("Text", this, "PlayerList", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}

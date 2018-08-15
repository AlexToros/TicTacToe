using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;

namespace TicTacToeClient
{
    public class Player
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }
        public Symbol Symbol { get; set; }

        public Player(uint PlayerId, string PlayerName)
        {
            ID = PlayerId;
            Name = PlayerName;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

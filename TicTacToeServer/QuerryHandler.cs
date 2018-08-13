using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLibrary;

namespace TicTacToeServer
{
    class QuerryHandler
    {
        public delegate void Handler(string command, object obj);
        public Handler handler;

        public QuerryHandler(Commands command, object obj)
        {
            switch (command)
            {
                case Commands.CONNECT:
                    break;
                case Commands.INVITE:
                    break;
                case Commands.NEW_PLAYER_LIST:

                    break;
                default:
                    break;
            }
        }
        private void ProcessNewPlayer(string name, PlayersPool pool)
        {

        }
    }
}

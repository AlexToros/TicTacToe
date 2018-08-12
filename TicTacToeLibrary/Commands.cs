using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLibrary
{
    /// <summary>
    /// Перечисление комманд, доступных для передачи серверу
    /// </summary>
    public enum Commands
    {
        /// <summary>
        /// Приглашение другого игрока
        /// </summary>
        INVITE = 0x001,
    }
}

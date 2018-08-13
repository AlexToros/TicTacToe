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
    public enum Commands : byte
    {
        /// <summary>
        /// Подключение клиента
        /// </summary>
        CONNECT = 0x00,
        /// <summary>
        /// Приглашение другого игрока
        /// </summary>
        INVITE = 0x01,
        /// <summary>
        /// Уведомление о новом списке игроков
        /// </summary>
        NEW_PLAYER_LIST = 0x02,
    }
}

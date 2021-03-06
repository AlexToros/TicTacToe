﻿using System;
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
        /// Приглашение другого игрока
        /// </summary>
        INVITE = 0x01,
        /// <summary>
        /// Уведомление о новом списке игроков
        /// </summary>
        NEW_PLAYER_LIST = 0x02,
        /// <summary>
        /// Уведомление об отключении клиента
        /// </summary>
        DISCONNECT = 0x03,
        /// <summary>
        /// Принять запрос
        /// </summary>
        ACCEPT_INVITE = 0x04,
        /// <summary>
        /// Отклонить запрос
        /// </summary>
        DENIED_INVITE = 0x05,
        /// <summary>
        /// Команда для обработки хода игрока
        /// </summary>
        TURN = 0x06,
        /// <summary>
        /// Код конца игры
        /// </summary>
        GAME_OVER = 0x07,
        /// <summary>
        /// Код сообщения с ID игрока
        /// </summary>
        PLAYER_ID = 0x08,
    }
}

using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.Domain.Abstract
{
    public interface IGameRepository
    {
        IEnumerable<Game> Games { get; }
        /// <summary>
        /// Вносит и сохраняет изменения в БД
        /// </summary>
        /// <param name="game"></param>
        void SaveGame(Game game);
        Game DeleteGame(int gameId);
    }
}

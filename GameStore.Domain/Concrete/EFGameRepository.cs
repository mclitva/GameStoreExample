using System.Collections.Generic;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using System;

namespace GameStore.Domain.Concrete
{
    public class EFGameRepository : IGameRepository
    {
        EFDbContext context = new EFDbContext();
        /// <summary>
        /// Представляет выборку всех данных из таблицы Games БД GameStore
        /// </summary>
        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }
        
        /// <summary>
        /// Вносит и сохраняет изменения в БД
        /// </summary>
        /// <param name="game"></param>
        public void SaveGame(Game game)
        {
            if(game.GameId == 0)
            {
                context.Games.Add(game);
            }
            else
            {
                Game dbEntry = context.Games.Find(game.GameId);
                if(dbEntry != null)
                {
                    dbEntry.Name = game.Name;
                    dbEntry.Price = game.Price;
                    dbEntry.Description = game.Description;
                    dbEntry.Category = game.Category;
                    dbEntry.ImageData = game.ImageData;
                    dbEntry.ImageMimeType = game.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Game DeleteGame(int gameId)
        {
            Game dbEntry = context.Games.Find(gameId);
            if(dbEntry != null)
            {
                context.Games.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}

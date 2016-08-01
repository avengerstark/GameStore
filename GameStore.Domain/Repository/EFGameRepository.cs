using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using GameStore.Domain.EF;

namespace GameStore.Domain.Repository
{
    public class EFGameRepository : IGameRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }

        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
            {
                context.Games.Add(game);
            }
            else
            {
                Game dbEntity = context.Games.Find(game.GameId);
                if (dbEntity != null)
                {
                    dbEntity.Name = game.Name;
                    dbEntity.Description = game.Description;
                    dbEntity.Price = game.Price;
                    dbEntity.Category = game.Category;
                }
            }
            context.SaveChanges();
        }


        public Game DeleteGame(int gameId)
        {
            Game game = context.Games.Find(gameId);
            if (game != null)
            {
                context.Games.Remove(game);
                context.SaveChanges();
            }
            return game;
        }
    }
}

using GameStore.Domain.Entities;
using System.Data.Entity;

namespace GameStore.Domain.Concrete
{
    class EFDbContext: DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Base
{
    public class DatabaseContext : DbContext
    {
        internal DbSet<Client> Client { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("IdentityMap");
        }
    }
}

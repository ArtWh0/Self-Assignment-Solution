using TechRent.Domain.Entities;
using System.Data.Entity;

namespace TechRent.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Tech> Teches { get; set; }
    }
}

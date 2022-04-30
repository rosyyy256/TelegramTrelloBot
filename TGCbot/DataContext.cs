using Microsoft.EntityFrameworkCore;
using TGCbot.Entities;

namespace TGCbot
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<AppUser> Users { get; set; }
    }
}
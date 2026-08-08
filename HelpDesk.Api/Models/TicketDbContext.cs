using Microsoft.EntityFrameworkCore;
namespace HelpDesk.Api.Models
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
            
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}

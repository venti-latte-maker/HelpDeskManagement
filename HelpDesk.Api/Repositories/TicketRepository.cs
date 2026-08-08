using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        TicketDbContext context;

        public TicketRepository(TicketDbContext context)
        {
            this.context = context;
        }
        public async Task<int> CreateTicketAsync(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            return ticket.Id;
        }

        public async Task DeleteTicketAsync(int id)
        {
            var t = await context.Tickets.FindAsync(id);
            if (t == null) return;

            context.Tickets.Remove(t);
            await context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await context.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await context.Tickets.FindAsync(id);
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            var ticketList = await context.Tickets
                .Where(t => t.Status.ToLower() == status.ToLower())
                .ToListAsync();

            return ticketList;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            context.Tickets.Update(ticket);
            await context.SaveChangesAsync();
        }
    }
}

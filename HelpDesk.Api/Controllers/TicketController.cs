using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private 
        ITicketRepository repo;
        public TicketController(ITicketRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTickets()
        {
            var ticketList = await repo.GetAllTicketsAsync();
            if (ticketList == null) return BadRequest("No tickets found.");
            return Ok(ticketList);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetTicketById(int id)
        {
            if (id == 0) return BadRequest();
            var t = await repo.GetTicketByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("No ticket found.");
            }

            if (string.IsNullOrWhiteSpace(ticket.Title))
                return BadRequest("Title is required.");

            if (string.IsNullOrWhiteSpace(ticket.Description))
                return BadRequest("Description is required.");

            if (string.IsNullOrWhiteSpace(ticket.RaisedBy))
                return BadRequest("RaisedBy is required.");

            var allowedStatus = new[] { "Open", "In Progress", "Closed" };
            if (!allowedStatus.Any(a => a.Equals(ticket.Status, StringComparison.OrdinalIgnoreCase)))        //checks if status is valid
                return BadRequest("Invalid status.");

            var allowedPriority = new[] { "Low", "Medium", "High" };
            if (!allowedPriority.Any(a => a.Equals(ticket.Priority, StringComparison.OrdinalIgnoreCase)))        //checks if priority is valid
                return BadRequest("Invalid priority.");

            ticket.CreatedDate = DateTime.UtcNow;

            int id = await repo.CreateTicketAsync(ticket);

            return Ok("Ticket created with Id = " + id);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody]Ticket ticket)
        {
            var currentTicket = await repo.GetTicketByIdAsync(id);
            if (currentTicket == null) return BadRequest("ID not found.");

            var allowedStatus = new[] { "Open", "In Progress", "Closed" };
            if (!allowedStatus.Any(a => a.Equals(ticket.Status, StringComparison.OrdinalIgnoreCase)))        //checks if status is valid
                return BadRequest("Invalid status.");

            var allowedPriority = new[] { "Low", "Medium", "High" };
            if (!allowedPriority.Any(a => a.Equals(ticket.Priority, StringComparison.OrdinalIgnoreCase)))        //checks if priority is valid
                return BadRequest("Invalid priority.");

            currentTicket.Title = ticket.Title;
            currentTicket.Description = ticket.Description;
            currentTicket.Priority = ticket.Priority;
            currentTicket.Status = ticket.Status;

            await repo.UpdateTicketAsync(currentTicket);

            return Ok(currentTicket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (id == 0) return BadRequest("Invalid ID given.");
            var ticket = await repo.GetTicketByIdAsync(id);
            if (ticket == null) return BadRequest("No ID found");

            await repo.DeleteTicketAsync(ticket.Id);

            return Ok("Ticket with id " + ticket.Id + " has been deleted.");

        }

        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetTicketByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status is required.");

            var allowed = new[] { "Open", "In Progress", "Closed" };
            if (!allowed.Any(a => a.Equals(status, StringComparison.OrdinalIgnoreCase)))        //checks if status is valid
                return BadRequest("Invalid status.");

            var ticketList = await repo.GetTicketsByStatusAsync(status);
            return Ok(ticketList);
        }
    }
}

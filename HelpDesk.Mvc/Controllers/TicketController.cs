using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Services;
using HelpDesk.Mvc.Models;
namespace HelpDesk.Mvc.Controllers
{
    public class TicketController : Controller
    {
        TicketService service;

        public TicketController(TicketService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var ticketList = await service.GetAllTicketsAsync();
            return View(ticketList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await service.GetTicketByIdAsync(id);
            return View(ticket);
        }

        public async Task<IActionResult> Status(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return RedirectToAction("Index");

            var ticketList = await service.GetTicketByStatusAsync(status);
            return View(ticketList);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateEditViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                await service.CreateTicketAsync(ticket);

                return RedirectToAction("Index");
            }
            return View(ticket);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var ticket = await service.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TicketCreateEditViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                await service.UpdateTicketAsync(id, ticket);
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await service.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.DeleteTicketAsync(id);
            return RedirectToAction("Index");
        }

    }
}

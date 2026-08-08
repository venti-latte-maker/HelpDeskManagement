using HelpDesk.Mvc.Models;
using System.Net;
using System.Net.Http.Json;
namespace HelpDesk.Mvc.Services
{
    public class TicketService
    {
        HttpClient client;

        public TicketService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<TicketCreateEditViewModel?> GetTicketByIdAsync(int id)
        {
            var response = await client.GetAsync($"{id}");

            // GetFromJsonAsync throws on non-success status codes (e.g. the API's 404
            // for an unknown id) instead of returning null, so read the response
            // manually and only deserialize on success.
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TicketCreateEditViewModel>();
        }

        public async Task<List<TicketCreateEditViewModel>?> GetAllTicketsAsync()
        {
            return await client.GetFromJsonAsync<List<TicketCreateEditViewModel>>("All");
        }

        public async Task CreateTicketAsync(TicketCreateEditViewModel ticket)
        {
            // API Create endpoint is a POST to the controller root
            var response = await client.PostAsJsonAsync("", ticket);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTicketAsync(int id, TicketCreateEditViewModel ticket)
        {
            // API Update endpoint expects PUT {id}
            var response = await client.PutAsJsonAsync($"{id}", ticket);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<TicketCreateEditViewModel>?> GetTicketByStatusAsync(string status)
        {
            return await client.GetFromJsonAsync<List<TicketCreateEditViewModel>>($"Status/{status}");
        }

        public async Task DeleteTicketAsync(int id)
        {
            var response = await client.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

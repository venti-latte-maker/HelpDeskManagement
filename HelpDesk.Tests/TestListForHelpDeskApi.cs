using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using HelpDesk.Api.Controllers;

namespace HelpDesk.Tests
{
    public class TestListForHelpDeskApi
    {
        List<Ticket> validTicket = new List<Ticket>
        {
            new Ticket { Id = 1, Title = "Ticket 1", Description = "Description 1", Priority = "Low", Status = "Open", RaisedBy = "User1", CreatedDate = DateTime.Parse("8/8/2026 10:53:31 AM") },
            new Ticket { Id = 2, Title = "Ticket 2", Description = "Description 2", Priority = "Medium", Status = "Open", RaisedBy = "User2", CreatedDate = DateTime.Parse("8/8/2026 10:53:31 AM") }
        };

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(x => x.GetAllTicketsAsync()).ReturnsAsync(validTicket);
            var controller = new TicketController(mockRepo.Object);

            var result = await controller.GetAllTickets();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, model.Count);
            Assert.Equal("Ticket 2", model[1].Title);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            int id = 1;
            var expected = validTicket[0];
            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(x => x.GetTicketByIdAsync(id)).ReturnsAsync(expected);

            var controller = new TicketController(mockRepo.Object);

            var result = await controller.GetTicketById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(expected.Id, model.Id);
            Assert.Equal(expected.Title, model.Title);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            int id = 99;
            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(x => x.GetTicketByIdAsync(id)).ReturnsAsync((Ticket?)null);

            var controller = new TicketController(mockRepo.Object);

            var result = await controller.GetTicketById(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            var newTicket = new Ticket
            {
                Title = "New",
                Description = "New description",
                Priority = "Low",
                Status = "Open",
                RaisedBy = "User"
            };

            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(x => x.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(3);

            var controller = new TicketController(mockRepo.Object);

            var result = await controller.CreateTicket(newTicket);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var msg = Assert.IsType<string>(okResult.Value);
            Assert.Contains("Ticket created with Id = 3", msg);
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            var mockRepo = new Mock<ITicketRepository>();
            var controller = new TicketController(mockRepo.Object);

            var result = await controller.CreateTicket(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            string status = "Open";
            var matches = new List<Ticket> { validTicket[0], validTicket[1] };

            var mockRepo = new Mock<ITicketRepository>();
            mockRepo.Setup(x => x.GetTicketsByStatusAsync(status)).ReturnsAsync(matches);

            var controller = new TicketController(mockRepo.Object);

            var result = await controller.GetTicketByStatus(status);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }
    }
}

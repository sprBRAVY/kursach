// PrintingOrderManager.Web.Controllers/ClientsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ContactsSortParm"] = sortOrder == "Contacts" ? "contacts_desc" : "Contacts";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var clients = _clientService.GetClientsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.ClientName.Contains(searchString) ||
                                            (!string.IsNullOrEmpty(s.Contacts) && s.Contacts.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.ClientName);
                    break;
                case "Contacts":
                    clients = clients.OrderBy(s => s.Contacts ?? "");
                    break;
                case "contacts_desc":
                    clients = clients.OrderByDescending(s => s.Contacts ?? "");
                    break;
                default:
                    clients = clients.OrderBy(s => s.ClientName);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<ClientDto>.CreateAsync(clients, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null) return NotFound();
            return View(client);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateClientDto clientDto)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddClientAsync(clientDto);
                return RedirectToAction(nameof(Index));
            }
            return View(clientDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null) return NotFound();
            var updateDto = new UpdateClientDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Contacts = client.Contacts
            };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateClientDto clientDto)
        {
            if (id != clientDto.ClientId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _clientService.UpdateClientAsync(id, clientDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clientDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
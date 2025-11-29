// PrintingOrderManager.Web/Controllers/ClientsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
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

            var clients = (await _clientService.GetAllClientsAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.ClientName.Contains(searchString) || (s.Contacts != null && s.Contacts.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.ClientName);
                    break;
                case "Contacts":
                    clients = clients.OrderBy(s => s.Contacts == null ? "" : s.Contacts);
                    break;
                case "contacts_desc":
                    clients = clients.OrderByDescending(s => s.Contacts == null ? "" : s.Contacts);
                    break;
                default:
                    clients = clients.OrderBy(s => s.ClientName);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<ClientDto>.CreateAsync(clients.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null) return NotFound();

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientName,Contacts")] CreateClientDto clientDto)
        {
            if (ModelState.IsValid)
            {
                await _clientService.AddClientAsync(clientDto);
                return RedirectToAction(nameof(Index));
            }
            return View(clientDto);
        }

        // GET: Clients/Edit/5
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

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,ClientName,Contacts")] UpdateClientDto clientDto)
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

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var client = await _clientService.GetClientByIdAsync(id.Value);
            if (client == null) return NotFound();

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
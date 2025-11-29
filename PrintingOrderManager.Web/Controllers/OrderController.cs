// PrintingOrderManager.Web/Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService; // Для получения списка клиентов

        public OrdersController(IOrderService orderService, IClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? clientIdFilter, string statusFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["ClientSortParm"] = sortOrder == "Client" ? "client_desc" : "Client";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var orders = (await _orderService.GetAllOrdersAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.ClientName.Contains(searchString));
            }

            if (clientIdFilter.HasValue && clientIdFilter.Value > 0)
            {
                orders = orders.Where(o => o.ClientId == clientIdFilter.Value);
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                orders = orders.Where(o => o.Status == statusFilter);
            }

            switch (sortOrder)
            {
                case "date_desc":
                    orders = orders.OrderByDescending(o => o.PlacementDate);
                    break;
                case "Client":
                    orders = orders.OrderBy(o => o.ClientName);
                    break;
                case "client_desc":
                    orders = orders.OrderByDescending(o => o.ClientName);
                    break;
                case "Status":
                    orders = orders.OrderBy(o => o.Status == null ? "" : o.Status);
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Status == null ? "" : o.Status);
                    break;
                default:
                    orders = orders.OrderBy(o => o.PlacementDate);
                    break;
            }

            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            ViewBag.SelectedClientId = clientIdFilter;
            ViewBag.SelectedStatus = statusFilter;

            int pageSize = 10;
            return View(await PaginatedList<OrderDto>.CreateAsync(orders.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,PlacementDate,CompletionDate,Status")] CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            return View(orderDto);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            ViewBag.Clients = await _clientService.GetAllClientsAsync();

            var updateDto = new UpdateOrderDto
            {
                OrderId = order.OrderId,
                ClientId = order.ClientId,
                PlacementDate = order.PlacementDate,
                CompletionDate = order.CompletionDate,
                Status = order.Status
            };

            return View(updateDto);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ClientId,PlacementDate,CompletionDate,Status")] UpdateOrderDto orderDto)
        {
            if (id != orderDto.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateOrderAsync(id, orderDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            return View(orderDto);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
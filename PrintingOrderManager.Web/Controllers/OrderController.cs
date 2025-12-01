// PrintingOrderManager.Web.Controllers/OrdersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;

        public OrdersController(IOrderService orderService, IClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? clientIdFilter,
            string statusFilter,
            int? pageNumber)
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

            var orders = _orderService.GetOrdersQueryable();

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
                    orders = orders.OrderBy(o => o.Status ?? "");
                    break;
                case "status_desc":
                    orders = orders.OrderByDescending(o => o.Status ?? "");
                    break;
                default:
                    orders = orders.OrderBy(o => o.PlacementDate);
                    break;
            }

            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            ViewBag.SelectedClientId = clientIdFilter;
            ViewBag.SelectedStatus = statusFilter;

            int pageSize = 10;
            return View(await PaginatedList<OrderDto>.CreateAsync(orders, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateOrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clients = await _clientService.GetAllClientsAsync();
            return View(orderDto);
        }

        [Authorize(Roles = "Admin")]
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
               // CompletionDate = order.CompletionDate,
                //Status = order.Status
            };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateOrderDto orderDto)
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _orderService.GetOrderByIdAsync(id.Value);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
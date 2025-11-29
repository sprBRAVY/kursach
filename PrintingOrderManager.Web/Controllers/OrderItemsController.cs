// PrintingOrderManager.Web/Controllers/OrderItemsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderService _orderService;
        private readonly IServiceService _serviceService;
        private readonly IWorkerService _workerService;
        private readonly IEquipmentService _equipmentService;

        public OrderItemsController(IOrderItemService orderItemService, IOrderService orderService, IServiceService serviceService, IWorkerService workerService, IEquipmentService equipmentService)
        {
            _orderItemService = orderItemService;
            _orderService = orderService;
            _serviceService = serviceService;
            _workerService = workerService;
            _equipmentService = equipmentService;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? orderIdFilter, int? serviceIdFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["OrderSortParm"] = string.IsNullOrEmpty(sortOrder) ? "order_desc" : "";
            ViewData["ServiceSortParm"] = sortOrder == "Service" ? "service_desc" : "Service";
            ViewData["CostSortParm"] = sortOrder == "Cost" ? "cost_desc" : "Cost";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var orderItems = (await _orderItemService.GetAllOrderItemsAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                orderItems = orderItems.Where(oi => oi.ServiceName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (orderIdFilter.HasValue && orderIdFilter.Value > 0)
            {
                orderItems = orderItems.Where(oi => oi.OrderId == orderIdFilter.Value);
            }

            if (serviceIdFilter.HasValue && serviceIdFilter.Value > 0)
            {
                orderItems = orderItems.Where(oi => oi.ServiceId == serviceIdFilter.Value);
            }

            switch (sortOrder)
            {
                case "order_desc":
                    orderItems = orderItems.OrderByDescending(oi => oi.OrderId);
                    break;
                case "Service":
                    orderItems = orderItems.OrderBy(oi => oi.ServiceName);
                    break;
                case "service_desc":
                    orderItems = orderItems.OrderByDescending(oi => oi.ServiceName);
                    break;
                case "Cost":
                    orderItems = orderItems.OrderBy(oi => oi.Cost);
                    break;
                case "cost_desc":
                    orderItems = orderItems.OrderByDescending(oi => oi.Cost);
                    break;
                default:
                    orderItems = orderItems.OrderBy(oi => oi.ItemId);
                    break;
            }

            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.Services = await _serviceService.GetAllServicesAsync();
            ViewBag.SelectedOrderId = orderIdFilter;
            ViewBag.SelectedServiceId = serviceIdFilter;

            int pageSize = 10;
            return View(await PaginatedList<OrderItemDto>.CreateAsync(orderItems.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id.Value);
            if (orderItem == null) return NotFound();

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.Services = await _serviceService.GetAllServicesAsync();
            ViewBag.Workers = await _workerService.GetAllWorkersAsync();
            ViewBag.Equipment = await _equipmentService.GetAllEquipmentAsync();
            return View();
        }

        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ServiceId,WorkerId,EquipmentId,Paper,Color,Quantity,Cost")] CreateOrderItemDto orderItemDto)
        {
            if (ModelState.IsValid)
            {
                await _orderItemService.AddOrderItemAsync(orderItemDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.Services = await _serviceService.GetAllServicesAsync();
            ViewBag.Workers = await _workerService.GetAllWorkersAsync();
            ViewBag.Equipment = await _equipmentService.GetAllEquipmentAsync();
            return View(orderItemDto);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id.Value);
            if (orderItem == null) return NotFound();

            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.Services = await _serviceService.GetAllServicesAsync();
            ViewBag.Workers = await _workerService.GetAllWorkersAsync();
            ViewBag.Equipment = await _equipmentService.GetAllEquipmentAsync();

            var updateDto = new UpdateOrderItemDto
            {
                ItemId = orderItem.ItemId,
                OrderId = orderItem.OrderId,
                ServiceId = orderItem.ServiceId,
                WorkerId = orderItem.WorkerId,
                EquipmentId = orderItem.EquipmentId,
                Paper = orderItem.Paper,
                Color = orderItem.Color,
                Quantity = orderItem.Quantity,
                Cost = orderItem.Cost
            };

            return View(updateDto);
        }

        // POST: OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,OrderId,ServiceId,WorkerId,EquipmentId,Paper,Color,Quantity,Cost")] UpdateOrderItemDto orderItemDto)
        {
            if (id != orderItemDto.ItemId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderItemService.UpdateOrderItemAsync(id, orderItemDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.Services = await _serviceService.GetAllServicesAsync();
            ViewBag.Workers = await _workerService.GetAllWorkersAsync();
            ViewBag.Equipment = await _equipmentService.GetAllEquipmentAsync();
            return View(orderItemDto);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id.Value);
            if (orderItem == null) return NotFound();

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderItemService.DeleteOrderItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
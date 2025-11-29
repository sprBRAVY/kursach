// PrintingOrderManager.Web/Controllers/ServicesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // GET: Services
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, decimal? minPriceFilter, decimal? maxPriceFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var services = (await _serviceService.GetAllServicesAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.ServiceName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (minPriceFilter.HasValue)
            {
                services = services.Where(s => s.UnitPrice >= minPriceFilter.Value);
            }

            if (maxPriceFilter.HasValue)
            {
                services = services.Where(s => s.UnitPrice <= maxPriceFilter.Value);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    services = services.OrderByDescending(s => s.ServiceName);
                    break;
                case "Price":
                    services = services.OrderBy(s => s.UnitPrice);
                    break;
                case "price_desc":
                    services = services.OrderByDescending(s => s.UnitPrice);
                    break;
                default:
                    services = services.OrderBy(s => s.ServiceName);
                    break;
            }

            ViewBag.MinPriceFilter = minPriceFilter;
            ViewBag.MaxPriceFilter = maxPriceFilter;

            int pageSize = 10;
            return View(await PaginatedList<ServiceDto>.CreateAsync(services.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetServiceByIdAsync(id.Value);
            if (service == null) return NotFound();

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceName,Description,UnitPrice")] CreateServiceDto serviceDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceService.AddServiceAsync(serviceDto);
                return RedirectToAction(nameof(Index));
            }
            return View(serviceDto);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetServiceByIdAsync(id.Value);
            if (service == null) return NotFound();

            var updateDto = new UpdateServiceDto
            {
                ServiceId = service.ServiceId,
                ServiceName = service.ServiceName,
                Description = service.Description,
                UnitPrice = service.UnitPrice
            };

            return View(updateDto);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,UnitPrice")] UpdateServiceDto serviceDto)
        {
            if (id != serviceDto.ServiceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceService.UpdateServiceAsync(id, serviceDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceDto);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetServiceByIdAsync(id.Value);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceService.DeleteServiceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
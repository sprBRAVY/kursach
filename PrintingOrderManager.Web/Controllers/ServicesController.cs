// PrintingOrderManager.Web/Controllers/ServicesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            decimal? minPriceFilter,
            decimal? maxPriceFilter,
            int? pageNumber)
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

            var services = _serviceService.GetServicesQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.ServiceName.Contains(searchString));
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
            return View(await PaginatedList<ServiceDto>.CreateAsync(services, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var service = await _serviceService.GetServiceByIdAsync(id.Value);
            if (service == null) return NotFound();
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateServiceDto serviceDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceService.AddServiceAsync(serviceDto);
                return RedirectToAction(nameof(Index));
            }
            return View(serviceDto);
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateServiceDto serviceDto)
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var service = await _serviceService.GetServiceByIdAsync(id.Value);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceService.DeleteServiceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
// PrintingOrderManager.Web.Controllers/EquipmentController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            string modelFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ModelSortParm"] = sortOrder == "Model" ? "model_desc" : "Model";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var equipment = _equipmentService.GetEquipmentQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                equipment = equipment.Where(e => e.EquipmentName.Contains(searchString) ||
                                                (!string.IsNullOrEmpty(e.Specifications) && e.Specifications.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(modelFilter))
            {
                equipment = equipment.Where(e => e.Model == modelFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    equipment = equipment.OrderByDescending(e => e.EquipmentName);
                    break;
                case "Model":
                    equipment = equipment.OrderBy(e => e.Model ?? "");
                    break;
                case "model_desc":
                    equipment = equipment.OrderByDescending(e => e.Model ?? "");
                    break;
                default:
                    equipment = equipment.OrderBy(e => e.EquipmentName);
                    break;
            }

            ViewBag.SelectedModel = modelFilter;
            int pageSize = 10;
            return View(await PaginatedList<EquipmentDto>.CreateAsync(equipment, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);
            if (equipment == null) return NotFound();
            return View(equipment);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateEquipmentDto equipmentDto)
        {
            if (ModelState.IsValid)
            {
                await _equipmentService.AddEquipmentAsync(equipmentDto);
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);
            if (equipment == null) return NotFound();
            var updateDto = new UpdateEquipmentDto
            {
                EquipmentId = equipment.EquipmentId,
                EquipmentName = equipment.EquipmentName,
                Model = equipment.Model,
                Specifications = equipment.Specifications
            };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateEquipmentDto equipmentDto)
        {
            if (id != equipmentDto.EquipmentId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _equipmentService.UpdateEquipmentAsync(id, equipmentDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id.Value);
            if (equipment == null) return NotFound();
            return View(equipment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _equipmentService.DeleteEquipmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsWithItems(int id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
            if (equipment == null) return NotFound();

            var orderItems = await _equipmentService.GetOrderItemsByEquipmentIdAsync(id);

            ViewBag.EquipmentName = equipment.EquipmentName;
            ViewBag.TotalItems = orderItems.Sum(x => x.Quantity);
            ViewBag.TotalCost = orderItems.Sum(x => x.Cost);

            return View(orderItems);
        }
    }
}
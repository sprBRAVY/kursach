// PrintingOrderManager.Web/Controllers/WorkersController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class WorkersController : Controller
    {
        private readonly IWorkerService _workerService;

        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            string positionFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PositionSortParm"] = sortOrder == "Position" ? "position_desc" : "Position";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var workers = _workerService.GetWorkersQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(w => w.WorkerFullName.Contains(searchString) ||
                                            (!string.IsNullOrEmpty(w.Contacts) && w.Contacts.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(positionFilter))
            {
                workers = workers.Where(w => w.Position == positionFilter);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    workers = workers.OrderByDescending(w => w.WorkerFullName);
                    break;
                case "Position":
                    workers = workers.OrderBy(w => w.Position ?? "");
                    break;
                case "position_desc":
                    workers = workers.OrderByDescending(w => w.Position ?? "");
                    break;
                default:
                    workers = workers.OrderBy(w => w.WorkerFullName);
                    break;
            }

            ViewBag.SelectedPosition = positionFilter;
            int pageSize = 10;
            return View(await PaginatedList<WorkerDto>.CreateAsync(workers, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _workerService.GetWorkerByIdAsync(id.Value);
            if (worker == null) return NotFound();
            return View(worker);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateWorkerDto workerDto)
        {
            if (ModelState.IsValid)
            {
                await _workerService.AddWorkerAsync(workerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(workerDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _workerService.GetWorkerByIdAsync(id.Value);
            if (worker == null) return NotFound();
            var updateDto = new UpdateWorkerDto
            {
                WorkerId = worker.WorkerId,
                WorkerFullName = worker.WorkerFullName,
                Position = worker.Position,
                Contacts = worker.Contacts
            };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdateWorkerDto workerDto)
        {
            if (id != workerDto.WorkerId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _workerService.UpdateWorkerAsync(id, workerDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workerDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _workerService.GetWorkerByIdAsync(id.Value);
            if (worker == null) return NotFound();
            return View(worker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _workerService.DeleteWorkerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsWithItems(int id)
        {
            var worker = await _workerService.GetWorkerByIdAsync(id);
            if (worker == null) return NotFound();

            var orderItems = await _workerService.GetOrderItemsByWorkerIdAsync(id);

            ViewBag.WorkerName = worker.WorkerFullName;
            ViewBag.TotalItems = orderItems.Sum(x => x.Quantity);
            ViewBag.TotalCost = orderItems.Sum(x => x.Cost);

            return View(orderItems);
        }

    }
}
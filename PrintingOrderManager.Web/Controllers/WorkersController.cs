// PrintingOrderManager.Web/Controllers/WorkersController.cs
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

        // GET: Workers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, string positionFilter, int? pageNumber)
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

            // ✅ ИСПРАВЛЕНО: используем IQueryable напрямую
            var workers = _workerService.GetWorkersQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(w => w.WorkerFullName.Contains(searchString) ||
                                            (w.Contacts != null && w.Contacts.Contains(searchString)));
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

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _workerService.GetWorkerByIdAsync(id.Value);
            if (worker == null) return NotFound();
            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkerFullName,Position,Contacts")] CreateWorkerDto workerDto)
        {
            if (ModelState.IsValid)
            {
                await _workerService.AddWorkerAsync(workerDto);
                return RedirectToAction(nameof(Index));
            }
            return View(workerDto);
        }

        // GET: Workers/Edit/5
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

        // POST: Workers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkerId,WorkerFullName,Position,Contacts")] UpdateWorkerDto workerDto)
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

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var worker = await _workerService.GetWorkerByIdAsync(id.Value);
            if (worker == null) return NotFound();
            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _workerService.DeleteWorkerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
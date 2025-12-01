using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;

public class ReportsController : Controller
{
    private readonly IReportingService _reportingService;

    public ReportsController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    public async Task<IActionResult> EquipmentUtilization()
    {
        var report = await _reportingService.GetEquipmentUtilizationAsync();
        return View(report);
    }

    public async Task<IActionResult> WorkerUtilization()
    {
        var report = await _reportingService.GetWorkerUtilizationAsync();
        return View(report);
    }

    public async Task<IActionResult> EquipmentDetails(int id)
    {
        var details = await _reportingService.GetEquipmentDetailsAsync(id);
        ViewBag.Title = "Детали по оборудованию";
        return View("UtilizationDetails", details);
    }

    public async Task<IActionResult> WorkerDetails(int id)
    {
        var details = await _reportingService.GetWorkerDetailsAsync(id);
        ViewBag.Title = "Детали по сотруднику";
        return View("UtilizationDetails", details);
    }
}
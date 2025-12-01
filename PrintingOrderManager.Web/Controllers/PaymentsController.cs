// PrintingOrderManager.Web.Controllers/PaymentsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using PrintingOrderManager.Web.Models;

namespace PrintingOrderManager.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? orderIdFilter,
            string statusFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["AmountSortParm"] = sortOrder == "Amount" ? "amount_desc" : "Amount";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var payments = _paymentService.GetPaymentsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                payments = payments.Where(p => p.Amount.ToString().Contains(searchString));
            }

            if (orderIdFilter.HasValue && orderIdFilter.Value > 0)
            {
                payments = payments.Where(p => p.OrderId == orderIdFilter.Value);
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                payments = payments.Where(p => p.PaymentStatus == statusFilter);
            }

            switch (sortOrder)
            {
                case "date_desc":
                    payments = payments.OrderByDescending(p => p.PaymentDate);
                    break;
                case "Amount":
                    payments = payments.OrderBy(p => p.Amount);
                    break;
                case "amount_desc":
                    payments = payments.OrderByDescending(p => p.Amount);
                    break;
                default:
                    payments = payments.OrderBy(p => p.PaymentDate);
                    break;
            }

            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            ViewBag.SelectedOrderId = orderIdFilter;
            ViewBag.SelectedStatus = statusFilter;

            int pageSize = 10;
            return View(await PaginatedList<PaymentDto>.CreateAsync(payments, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null) return NotFound();
            return View(payment);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreatePaymentDto paymentDto)
        {
            if (ModelState.IsValid)
            {
                await _paymentService.AddPaymentAsync(paymentDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            return View(paymentDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null) return NotFound();
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            var updateDto = new UpdatePaymentDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus
            };
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UpdatePaymentDto paymentDto)
        {
            if (id != paymentDto.PaymentId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await _paymentService.UpdatePaymentAsync(id, paymentDto);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            return View(paymentDto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null) return NotFound();
            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
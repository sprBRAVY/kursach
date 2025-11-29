// PrintingOrderManager.Web/Controllers/PaymentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: Payments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? orderIdFilter, string statusFilter, int? pageNumber)
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

            var payments = (await _paymentService.GetAllPaymentsAsync()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Поиск по сумме (как строке)
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
            return View(await PaginatedList<PaymentDto>.CreateAsync(payments.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Amount,PaymentDate,PaymentStatus")] CreatePaymentDto paymentDto)
        {
            if (ModelState.IsValid)
            {
                await _paymentService.AddPaymentAsync(paymentDto);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Orders = await _orderService.GetAllOrdersAsync();
            return View(paymentDto);
        }

        // GET: Payments/Edit/5
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

        // POST: Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,OrderId,Amount,PaymentDate,PaymentStatus")] UpdatePaymentDto paymentDto)
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

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _paymentService.GetPaymentByIdAsync(id.Value);
            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
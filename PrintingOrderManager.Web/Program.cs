using Microsoft.EntityFrameworkCore;
using PrintingOrderManager.Infrastructure.Data;
using PrintingOrderManager.Core.Interfaces;
using PrintingOrderManager.Infrastructure.Repositories;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Application.Mappings;  // ← MappingProfile здесь

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// === Репозитории ===
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();

// === Сервисы ===
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();

// === AutoMapper 15.1.0 — РАБОЧАЯ РЕГИСТРАЦИЯ ===
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "";  // Для разработки; для продакшена — купи лицензию

    cfg.AddProfile<MappingProfile>();  // Твой профиль
    // Или: cfg.AddMaps(typeof(MappingProfile).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
using InventoryWebService.Data;
using InventoryWebService.Repository;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conn") ?? throw new InvalidOperationException("Connection string 'conn' not found.")));*/

builder.Services.AddDbContext<InventoryContext>(o => o.UseInMemoryDatabase("InventoryDb"));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddSingleton<ISeedData, SeedData>();

var app = builder.Build();


/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<ISeedData>();
    dbInitializer.Initialize(scope.ServiceProvider);
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inventory}/{action=Index}/{id?}");

app.Run();

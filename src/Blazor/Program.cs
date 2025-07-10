using Application;
using Contract;
using Infrastructure;
using InventoryManagement;
using InventoryManagement.Components;
using Microsoft.EntityFrameworkCore;
using Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Inventory Management Api";
    config.Version = "v1";
    config.Description = "API for Inventory Management";
    config.DocumentName = "v1";
});
builder.Services.AddControllers();


builder.Services.AddTransient<ISeedService, SeedService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IInventoryReader, InventoryReader>();
builder.Services.AddTransient<IInventoryWriter, InventoryWriter>();
builder.Services.AddDbContextFactory<MainDbContext>(options =>
    options.UseInMemoryDatabase("PortfolioDemoDb"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseOpenApi();
app.UseSwaggerUi();
app.Run();

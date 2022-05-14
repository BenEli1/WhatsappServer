using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static whatsappProject.Controllers.FeedBackService;
using static whatsappProject.Controllers.IFeedBackService;
using whatsappProject.Data;
using whatsappProject.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<whatsappProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("whatsappProjectContext") ?? throw new InvalidOperationException("Connection string 'whatsappProjectContext' not found.")));

builder.Services.AddScoped<IFeedBackService, FeedBackService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

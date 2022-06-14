using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static whatsappProject.Controllers.FeedBackService;
using static whatsappProject.Controllers.IFeedBackService;
using whatsappProject.Controllers;
using whatsappProject.Hubs;
using Microsoft.AspNetCore.Builder;
using whatsappProject.Data;
using CorePush.Apple;
using CorePush.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using net_core_api_push_notification_demo.Models;
using net_core_api_push_notification_demo.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddSingleton<IUserService>(new DataBase());
builder.Services.AddSingleton<ChatHub>(new ChatHub());

builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddHttpClient<FcmSender>();
builder.Services.AddHttpClient<ApnSender>();

// Configure strongly typed settings objects
var appSettingsSection = builder.Configuration.GetSection("FcmNotification");
builder.Services.Configure<FcmNotificationSetting>(appSettingsSection);
builder.Services.AddRazorPages();
builder.Services.AddDbContext<whatsappProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("whatsappProjectContext") ?? throw new InvalidOperationException("Connection string 'whatsappProjectContext' not found.")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin();
                              //.AllowCredentials();
                      });
});

/*builder.Services.AddDbContext<whatsappProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("whatsappProjectContext") ?? throw new InvalidOperationException("Connection string 'whatsappProjectContext' not found.")));
*/
builder.Services.AddScoped<IFeedBackService, FeedBackService>();
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



//app.UseHttpRedirection();

app.UseStaticFiles();
app.MapHub<ChatHub>("/Hubs/chatHub");
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSession();
app.UseCookiePolicy();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=/api/contacts}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});
app.Run();




using AIS;
using AIS.Services;
using Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoreContext>(options => options.UseNpgsql(connection));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<CoreContext>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IPartnerService, PartnerService>();
builder.Services.AddTransient<IMyTaskService, MyTaskService>();
builder.Services.AddTransient<IMyUsersService, MyUsersService>();
builder.Services.AddTransient<IContractsService, ContractsService>();
builder.Services.AddTransient<ILetterService, LetterService>();
builder.Services.AddTransient<IEnclosureService, EnclosureService>();
builder.Services.AddTransient<IConditionsService, ConditionsService>();
builder.Services.AddTransient<IDocumentGenerator, DocumentGenerator>();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//Переопределяем разделитель для русскоязычной локализации (иначе не будут работать поля number)
var cultureInfo = new CultureInfo("ru-RU");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



app.UseRouting();
app.UseAuthentication();    // подключение аутентификации
app.UseAuthorization();


app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<AisHub>("/push");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
    app.Run();
}



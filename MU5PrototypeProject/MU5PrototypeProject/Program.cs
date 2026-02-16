using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MU5PrototypeProject.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MUContext")
        ?? throw new InvalidOperationException("Connection string 'MUContext' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDbContext<MUContext>(options =>
    options.UseSqlite(connectionString));

//To give acess to IHttpContextAccessor for Audit Data with IAudtable
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Prepare DB and seed data (Medical Office style: just before app.Run)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Dev: migrations + sample data
    // Non-dev (staging/prod): migrations, but no sample data
    var isDev = app.Environment.IsDevelopment();

    MUInitializer.Initialize(
        serviceProvider: services,
        deleteDatabase: false,
        useMigrations: true,
        seedSampleData: isDev);
}

app.Run();

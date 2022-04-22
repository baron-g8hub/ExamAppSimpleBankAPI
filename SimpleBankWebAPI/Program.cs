using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Contracts;
using DataAccessLayer.Repository;
using DataAccessLayer.DataContextEFCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DBCoreContext' not found.")));


var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("DefaultConnection");


//var provider = builder.Services.BuildServiceProvider();
//var configuration = provider.GetRequiredService<IConfiguration>();
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<BankingContext>(options => options.UseSqlServer("name=ConnectionStrings:BankingContext"));



// NOTE: Should add here the environment variable to be used in CI/CD
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.Dev.json", optional: true);
builder.Configuration.AddEnvironmentVariables();



// Add services to the container.

builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();


//builder.Services.AddTransient<IAccountsServiceRepository, AccountsServiceRepository>();
//builder.Services.AddTransient<IPostedTransactionsRepository, PostedTransactionsRepository>();




// NOTE: Registration of customized Swagger UI and to be turned-of when production to free up resources.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Simple Bank .NetCore6 API by: BaronLugtu",
        Description = "Web app, and Api with Swagger UI.",
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Example Contact",
        //    Url = new Uri("https://example.com/contact")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });
});


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

app.UseRouting();

app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("../swagger/v1/swagger.json", "SimpleBankingApi");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Index}/{id?}");

app.Run();

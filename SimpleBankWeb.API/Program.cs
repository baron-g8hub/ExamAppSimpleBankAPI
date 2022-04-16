using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// NOTE: Should add here the environment variable to be used in CI/CD
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.Dev.json", optional: true);
builder.Configuration.AddEnvironmentVariables();



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// NOTE: Registration of customized Swagger UI and to be turned-of when production to free up resources.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Simple Bank API",
        Description = ".NetCore test app by: Baron",
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
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("../swagger/v1/swagger.json", "SimpleBankingApi");
});

app.MapControllers();

app.Run();

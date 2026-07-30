using Sadkah.API.Extensions;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.AddIdentityServices()
    .AddJwtAuthentication(builder.Configuration)
    .AddCustomValidation()
    .AddApplicationServices()
    .AddRateLimiting()
    .AddCloudinaryConfiguration(builder.Configuration);

var app = builder.Build();

// Runtime image doesn't ship dotnet-ef, so apply pending migrations here instead.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDBContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseCustomStatusCodes();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapCustomEndpoints();

app.Run();

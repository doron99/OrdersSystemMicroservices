using BusinessLogicLayer;
using BusinessLogicLayer.DomainEvents;
using DataAccessLayer;
using DataAccessLayer.Data;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Order.API.FilterAttributes;
using Order.API.Hubs;
using Order.API.Middleware;
using Order.API.OperationFilters;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // Set minimum log level to Debug for trace logging
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // Override for Microsoft logs
            //.Enrich.FromLogContext() // Enrich logs with contextual information
            //.Enrich.WithCorrelationId() // Add correlation ID to logs
            .WriteTo.Console() // Log to console
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Log to file
            .CreateLogger();


// Use Serilog for logging
builder.Host.UseSerilog();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>(); // Register the custom filter

}).AddNewtonsoftJson();

//add inner url

string orderscLIENTServiceUrl = $"http://{builder.Configuration["ClientMicroserviceExternalName"]}:{builder.Configuration["ClientMicroserviceExternalPort"]}";
//Environment.GetEnvironmentVariable("ORDER_CLIENT_MICROSERVICE_BASE_URL") ?? "http://localhost:5002";
//string[] strings = orderscLIENTServiceUrl.Split(",").ToArray();

await builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
builder.Services.AddHttpClient<IDomainEventPublisher, ApiEventPublisher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
            .WithOrigins(orderscLIENTServiceUrl)//"http://localhost:5002", orderscLIENTServiceUrl)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});
builder.Services.AddSignalR();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // הוסף כותרות ל-WAGGER
    c.OperationFilter<AddCorrelationIdHeader>();
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseMiddleware<CorrelationIdMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();
app.MapHub<OrderHub>("/orderHub");
app.MapHealthChecks("/health");

app.Run();


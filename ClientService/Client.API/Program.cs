using Client.API.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddHttpClient<OrdersMicroserviceClient>(client =>
{
    //string ordersServiceUrl = Environment.GetEnvironmentVariable("ORDER_API_MICROSERVICE_HTTP_CLIENT_BASE_URL");

    //client.BaseAddress = new Uri(ordersServiceUrl);// $"http://localhost:5001");

    client.BaseAddress = new Uri($"http://{builder.Configuration["OrdersMicroserviceName"]}:{builder.Configuration["OrdersMicroservicePort"]}");
});
builder.Services.AddHttpClient<ProductsMicroserviceClient>(client =>
{
    //string ordersServiceUrl = Environment.GetEnvironmentVariable("PRODUCT_API_MICROSERVICE_HTTP_CLIENT_BASE_URL");

    //client.BaseAddress = new Uri(ordersServiceUrl);// $"http://localhost:5001");

    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

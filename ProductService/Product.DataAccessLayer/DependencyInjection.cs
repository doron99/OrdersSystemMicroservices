using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoriesContracts;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static async Task<IServiceCollection> AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionStringTemplate = configuration.GetConnectionString("MSSQLDB")!;
            string connectionString = connectionStringTemplate
                .Replace("$MSSQL_HOST", Environment.GetEnvironmentVariable("MSSQL_HOST"))
                .Replace("$MSSQL_PORT", Environment.GetEnvironmentVariable("MSSQL_PORT"))
                .Replace("$MSSQL_DATABASE", Environment.GetEnvironmentVariable("MSSQL_DATABASE"))
                .Replace("$MSSQL_USER", Environment.GetEnvironmentVariable("MSSQL_USER"))
                .Replace("$MSSQL_PASSWORD", Environment.GetEnvironmentVariable("MSSQL_PASSWORD"));


            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IProductsRepository, ProductsRepository>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var productsRepo = scope.ServiceProvider.GetRequiredService<IProductsRepository>();
                //ensure create db  
                context.Database.EnsureCreated();
                //seed first data
                if (!await context.Products.AnyAsync())
                {
                    var product1 = Product.Create("Cement Bag", "CEM001", 50.00m, 100);
                    var product2 = Product.Create("Steel Rod", "STEEL002", 25.50m, 150);
                    var product3 = Product.Create("Wooden Plank", "WOOD003", 10.75m, 75);
                    var product4 = Product.Create("Brick", "BRICK004", 0.50m, 500);
                    var product5 = Product.Create("Concrete Mix", "CONCRETE005", 60.00m, 80);
                    var product6 = Product.Create("Insulation Foam", "INSUL006", 15.30m, 120);
                    var product7 = Product.Create("Paint Can", "PAINT007", 20.00m, 200);
                    var product8 = Product.Create("Nail Box", "NAIL008", 3.99m, 300);
                    var product9 = Product.Create("Pipe Fittings", "PIPE009", 8.25m, 60);
                    var product10 = Product.Create("Safety Helmet", "HELMET010", 12.00m, 90);
                    var product11 = Product.Create("Lumber", "LUMBER011", 45.00m, 70);
                    var product12 = Product.Create("Drywall Sheet", "DRYWALL012", 12.50m, 150);
                    var product13 = Product.Create("Roof Shingles", "SHINGLE013", 2.00m, 300);
                    var product14 = Product.Create("Concrete Block", "CONBLOCK014", 3.00m, 200);
                    var product15 = Product.Create("Electrical Wiring", "WIRING015", 0.75m, 500);
                    var product16 = Product.Create("Plumbing Pipes", "PLUMB016", 5.00m, 60);
                    var product17 = Product.Create("Tile", "TILE017", 1.50m, 400);
                    var product18 = Product.Create("Construction Adhesive", "ADHESIVE018", 8.00m, 100);
                    var product19 = Product.Create("Safety Gloves", "GLOVES019", 4.50m, 150);
                    var product20 = Product.Create("Masonry Tools Set", "TOOL020", 75.00m, 25);
                    var product21 = Product.Create("Safety Goggles", "GOGGLES021", 10.00m, 120);
                    var product22 = Product.Create("Measuring Tape", "TAPE022", 7.50m, 200);
                    var product23 = Product.Create("Concrete Mixer", "MIXER023", 250.00m, 10);
                    var product24 = Product.Create("Excavator Rental", "EXCAVATOR024", 300.00m, 5);
                    var product25 = Product.Create("Sandbags", "SANDBAG025", 1.50m, 300);
                    var product26 = Product.Create("Tarpaulin", "TARP026", 25.00m, 50);
                    var product27 = Product.Create("Plywood", "PLYWOOD027", 40.00m, 75);
                    var product28 = Product.Create("Scaffolding", "SCAFFOLD028", 150.00m, 15);
                    var product29 = Product.Create("Tool Belt", "BELT029", 15.00m, 90);
                    var product30 = Product.Create("Climbing Harness", "HARNESS030", 30.00m, 40);
                    //add all products to db using addrange
                    await context.Products.AddRangeAsync(new List<Product> {
                        product1, product2, product3, product4, product5,
                        product6, product7, product8, product9, product10,
                        product11, product12, product13, product14, product15,
                        product16, product17, product18, product19, product20,
                        product21, product22, product23, product24, product25,
                        product26, product27, product28, product29, product30
                    });
                    await context.SaveChangesAsync();

                    //Cement Bag: שק מיוחד להכנת בטון.
                    //Steel Rod: מוט ברזל בנייה.
                    //Wooden Plank: קורת עץ לשימושים שונים.
                    //Brick: לבנה לבנייה.
                    //Concrete Mix: תערובת בטון מוכנה.
                    //Insulation Foam: קצף בידוד.
                    //Paint Can: פח צבע.
                    //Nail Box: קופסה עם מסמרים.
                    //Pipe Fittings: חיבורי צינורות.
                    //Safety Helmet: קסדה לביטחון בעבודה.
                    //Lumber: עץ לחיתוך לבניית מבנים.
                    //Drywall Sheet: לוח גבס לציפוי קירות.
                    //Roof Shingles: רעפי גג להגנה על בית.
                    //Concrete Block: בלוק בטון לבנייה.
                    //Electrical Wiring: חוטים חשמליים להתקנה.
                    //Plumbing Pipes: צינורות אינסטלציה.
                    //Tile: אריחים לריצוף.
                    //Construction Adhesive: דבק בנייה.
                    //Safety Gloves: כפפות בטיחות לעבודה.
                    //Masonry Tools Set: ערכת כלים לבניין.
                    //Safety Goggles: משקפי מגן להגנה על העיניים.
                    //Measuring Tape: סרט מדידה.
                    //Concrete Mixer: מכונת ערבוב בטון.
                    //Excavator Rental: השכרת אקזקווטור.
                    //Sandbags: שקי חול לשימושים שונים, כולל חיזוק.
                    //Tarpaulin: יריעת כיסוי עמידה למים.
                    //Plywood: לוח לא מעובד מעץ לשימושים שונים.
                    //Scaffolding: סנפלינג לבניין.
                    //Tool Belt: חגורה לאחסון כלים בזמן העבודה.
                    //Climbing Harness: רתמת טיפוס להגנה בעבודה בגובה.

                }

            }

            return services;
        }
    }
}

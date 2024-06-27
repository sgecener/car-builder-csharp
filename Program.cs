using CarBuilderAPI.models;

var paintColors = new List<PaintColor>
        {
            new PaintColor { Id = 1, Color = "Silver", Price = 0 },
            new PaintColor { Id = 2, Color = "Midnight Blue", Price = 295 },
            new PaintColor { Id = 3, Color = "Firebrick Red", Price = 395 },
            new PaintColor { Id = 4, Color = "Spring Green", Price = 495 }
        };

var interiors = new List<Interior>
{
    new Interior { Id = 1, Material = "Beige Fabric", Price = 0 },
    new Interior { Id = 2, Material = "Charcoal Fabric", Price = 195 },
    new Interior { Id = 3, Material = "White Leather", Price = 1495 },
    new Interior { Id = 4, Material = "Black Leather", Price = 1495 }
};

var techPackages = new List<Technology>
{
    new Technology { Id = 1, Package = "Basic Package", Price = 0 },
    new Technology { Id = 2, Package = "Navigation Package", Price = 795 },
    new Technology { Id = 3, Package = "Visibility Package", Price = 1095 },
    new Technology { Id = 4, Package = "Ultra Package", Price = 1595 }
};

var wheels = new List<Wheels>
{
    new Wheels { Id = 1, Style = "17-inch Pair Radial", Price = 0 },
    new Wheels { Id = 2, Style = "17-inch Pair Radial Black", Price = 195 },
    new Wheels { Id = 3, Style = "18-inch Pair Spoke Silver", Price = 495 },
    new Wheels { Id = 4, Style = "18-inch Pair Spoke Black", Price = 695 }
};

var orders = new List<Order>
{
     new Order
    {
        Id = 1,
        Timestamp = new DateTime(2024, 6, 1, 8, 30, 0),
        WheelId = 1,
        TechnologyId = 1,
        PaintId = 3,
        InteriorId = 4
    },
    new Order
    {
        Id = 2,
        Timestamp = new DateTime(2024, 6, 2, 9, 45, 0),
        WheelId = 2,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 4
    },
    new Order
    {
        Id = 3,
        Timestamp = new DateTime(2024, 6, 3, 10, 15, 0),
        WheelId = 3,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 3
    },
    new Order
    {
        Id = 4,
        Timestamp = new DateTime(2024, 6, 4, 11, 30, 0),
        WheelId = 1,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 4
    }
};




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/paintColor", () =>
{
    return paintColors.Select(e => new PaintColorDto
    {
        Id = e.Id,
        Color = e.Color,
        Price = e.Price

    });
});

app.MapGet("/interiors", () =>
{
    return interiors.Select(e => new InteriorDto
    {
        Id = e.Id,
        Material = e.Material,
        Price = e.Price

    });
});

app.MapGet("/techPackages", () =>
{
    return techPackages.Select(e => new TechnologyDto
    {
        Id = e.Id,
        Package = e.Package,
        Price = e.Price

    });
});

app.MapGet("/wheels", () =>
{
    return wheels.Select(e => new WheelsDto
    {
        Id = e.Id,
        Style = e.Style,
        Price = e.Price

    });
});


app.MapGet("/orders", () =>
{
    return orders.Select(o => new OrderDto
    {
        Id = o.Id,
        WheelId = o.WheelId,
        TechnologyId = o.TechnologyId,
        PaintColorId = o.PaintId,
        InteriorId = o.InteriorId,
        Timestamp = o.Timestamp
    });
});

app.MapPost("/orders", (Order order) =>
{

    // Get the customer data to check that the customerid for the service ticket is valid
    // Order order = orders.FirstOrDefault(o => o.Id == order.CustomerId);

    // if the client did not provide a valid customer id, this is a bad request
    // if (customer == null)
    // {
    //     return Results.BadRequest();
    // }

    // creates a new id (SQL will do this for us like JSON Server did!)
    order.Id = orders.Max(st => st.Id) + 1;
    orders.Add(order);

    // Created returns a 201 status code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/orders/{order.Id}", new OrderDto
    {
        Id = order.Id,
        TechnologyId = order.TechnologyId,
         
       
    });

});





app.Run();


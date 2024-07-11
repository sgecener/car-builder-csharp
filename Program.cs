using CarBuilderAPI.models;

var paintColors = new List<PaintColor>
        {
            new PaintColor { Id = 1, Color = "Silver", Price = 0 },
            new PaintColor { Id = 2, Color = "Midnight Blue", Price = 295 },
            new PaintColor { Id = 3, Color = "Firebrick Red", Price = 395 },
            new PaintColor { Id = 4, Color = "Spring Forest Green", Price = 495 }
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
        InteriorId = 4,
        Fulfilled = false
    },
    new Order
    {
        Id = 2,
        Timestamp = new DateTime(2024, 6, 2, 9, 45, 0),
        WheelId = 2,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 4,
        Fulfilled = false

    },
    new Order
    {
        Id = 3,
        Timestamp = new DateTime(2024, 6, 3, 10, 15, 0),
        WheelId = 3,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 3,
        Fulfilled = false

    },
    new Order
    {
        Id = 4,
        Timestamp = new DateTime(2024, 6, 4, 11, 30, 0),
        WheelId = 1,
        TechnologyId = 2,
        PaintId = 3,
        InteriorId = 4,
        Fulfilled = false

    }
};



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}
app.UseHttpsRedirection();

app.MapGet("/paintColors", () =>
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

app.MapGet("/technologies", () =>
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


app.MapGet("/orders", (int? paintColorId) =>
{

    if (paintColorId != null & paintColors.FirstOrDefault(paintColor => paintColor.Id == paintColorId) == null)
    {
        return Results.BadRequest();
    }

    List<OrderDto> ordersDTO = new List<OrderDto>();

    foreach (Order order in orders)

    {
        if (!order.Fulfilled) 
        {
            Wheels? wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
            Technology? technology = techPackages.FirstOrDefault(t => t.Id == order.TechnologyId);
            PaintColor? paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);
            Interior? interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);

         ordersDTO.Add(new OrderDto()
        {
            Id = order.Id,
            InteriorId = order.InteriorId,
            PaintColorId = order.PaintId,
            TechnologyId = order.TechnologyId,
            WheelId = order.WheelId,
            Wheel = wheel == null ? null : new WheelsDto
            {
                Id = wheel.Id,
                Price = wheel.Price,
                Style = wheel.Style
            },
            Interior = interior == null ? null : new InteriorDto
            {
                Id = interior.Id,
                Price = interior.Price,
                Material = interior.Material
            },
            Technology = technology == null ? null : new TechnologyDto
            {
                Id = technology.Id,
                Price = technology.Price,
                Package = technology.Package
            },
            Paint = paintColor == null ? null : new PaintColorDto
            {
                Id = paintColor.Id,
                Price = paintColor.Price,
                Color = paintColor.Color
            },
        });

    }

}

List<OrderDto> filteredOrders = ordersDTO;

if (paintColorId != null)
    {
        filteredOrders = filteredOrders.Where(order => order.PaintColorId == paintColorId).ToList();
    }

    return Results.Ok(filteredOrders);

});

app.MapPost("/orders", (Order order) =>
{
    // Set the timestamp to the current time
    order.Timestamp = DateTime.Now;

    // Create a new id for the order
    order.Id = orders.Max(o => o.Id) + 1;
    orders.Add(order);

    // Return a 201 status code with a link to the new resource
    return Results.Created($"/orders/{order.Id}", new OrderDto
    {
        Id = order.Id,
        WheelId = order.WheelId,
        TechnologyId = order.TechnologyId,
        PaintColorId = order.PaintId,
        InteriorId = order.InteriorId,
        Timestamp = order.Timestamp
    });
});

app.MapPost("/orders/{id}/fulfill", (int id) => 
{
    Order order = orders.FirstOrDefault(order => order.Id == id);

    if (order == null | order.Fulfilled == true)
    {
        return Results.BadRequest();
    }

    order.Fulfilled = true;

    return Results.Ok();
});

app.Run();





app.Run();


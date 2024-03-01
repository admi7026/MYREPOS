using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ordering.API.Data;
using Ordering.API.Models;
using Ordering.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddOptions<AudienceSettings>().BindConfiguration("AudienceSettings");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.DescribeAllEnumsAsStrings();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HTTP API",
        Version = "v1",
        Description = "The Service HTTP API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' following by space and JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "AuthKey"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

var audienceConfig = new AudienceSettings();
builder.Configuration.GetSection("AudienceSettings").Bind(audienceConfig);
var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig.Secret));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = audienceConfig.Iss,
    ValidateAudience = true,
    ValidAudience = audienceConfig.Aud,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
};

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = "AuthKey";
    })
    .AddJwtBearer("AuthKey", x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/orders", async (IOrderService orderService, CreateOrderRequest request) =>
{
    var orderId = await orderService.CreateOrderAsync(request);
    return new { orderId };
})
.WithName("create-order")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/orders/{orderId}", async (IOrderService orderService, int orderId) =>
{
    var order = await orderService.GetOrderAsync(orderId);
    if(order is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
})
.WithName("get-order-by-id")
.WithOpenApi()
.RequireAuthorization();

app.MapGet("/orders", async (IOrderService orderService) =>
{
    var orders = await orderService.GetOrdersAsync();
    
    return orders;
})
.WithName("list-all-orders")
.WithOpenApi()
.RequireAuthorization();

app.Run();

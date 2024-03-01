using Account.API.Data;
using Account.API.Models;
using Account.API.Services;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddOptions<AudienceSettings>().BindConfiguration("AudienceSettings");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddHttpContextAccessor();
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

app.MapPost("/register", async (IAccountService service, UserModel model) =>
{
    var result = await service.RegisterUser(model.UserName!, model.Password!);

    if (result)
    {
        return Results.NoContent();
    }

    return Results.BadRequest();
})
.WithName("register")
.WithOpenApi();

app.MapPost("/login", async (IAccountService service, UserModel model) =>
{
    var result = await service.Login(model.UserName!, model.Password!);

    if (result != null)
    {
        return Results.Ok(result);
    }

    return Results.BadRequest();
})
.WithName("login")
.WithOpenApi();

app.MapGet("/get-user", async (IHttpContextAccessor service) =>
{
    var user = service.HttpContext.User;

    return Results.Ok();
})
.WithName("get-user")
.WithOpenApi()
.RequireAuthorization();

app.Run();

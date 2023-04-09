using Alumni_Back;
using Alumni_Back.Helpers;
using Alumni_Back.Repository;
using Alumni_Back.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization Header /{token}",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "token"
        },
        In = ParameterLocation.Header
    };
    var requirements = new OpenApiSecurityRequirement
    {
        {scheme,new List<string>() }
    };
    options.OperationFilter<AuthorizationOperationFilter>();
    //options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", build =>
    {
        build.WithOrigins("http://localhost:4200");
        build.AllowAnyHeader();
        build.AllowAnyMethod();
    });
});

var connectionstring = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(
        connectionstring,
        ServerVersion.AutoDetect(connectionstring)
     );
});

//All the services
builder.Services.AddScoped<IJwtRepository, JwtService>();
builder.Services.AddScoped<IMediaRepository, MediaHelper>();
builder.Services.AddScoped<Authorization>();
builder.Services.AddScoped<IUserRepository,UserService>();
builder.Services.AddScoped<IUniversityRepository,UniversityService>();
builder.Services.AddScoped<IPointRepository, PointService>();
builder.Services.AddScoped<IInterractorRepository, InterractionService>();
builder.Services.AddScoped<IPostRepository, PostService>();
builder.Services.AddScoped<IFaker,FakerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

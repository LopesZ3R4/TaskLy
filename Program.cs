// File Path: c:\TaskLy\Program.cs

using Services;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.EnableAnnotations();
});

var connectionString = string.Empty;

if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else
{
    connectionString = builder.Configuration.GetConnectionString("FallbackConnection");
}

try
{
    using var connection = new SqlConnection(connectionString);
    connection.Open();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while trying to open the connection: {ex.Message}");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
    
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<HolidayService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<TagsRepository>();
builder.Services.AddScoped<TaskTagsRepository>();
builder.Services.AddScoped<HolidayRepository>();
builder.Services.AddScoped<CountyRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run("http://*:80");
//app.Run();
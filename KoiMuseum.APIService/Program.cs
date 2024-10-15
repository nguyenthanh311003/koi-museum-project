using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using Microsoft.EntityFrameworkCore;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegisterDetailService, RegisterDetailService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IRankService, RankService>();
//builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddDbContext<Fa24Se172594Prn231G1KfsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Koi_museumDB")));

// Initialize PayOS
PayOS payOS = new PayOS(
    builder.Configuration["PayOS:ClientId"] ?? Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID"),
    builder.Configuration["PayOS:ApiKey"] ?? Environment.GetEnvironmentVariable("PAYOS_API_KEY"),
    builder.Configuration["PayOS:ChecksumKey"] ?? Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY")
);


// Register PayOS as a singleton service
builder.Services.AddSingleton(payOS);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

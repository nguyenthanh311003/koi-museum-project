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
builder.Services.AddScoped<IJudgeService, JudgeService>();
//builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<JwtService, JwtService>();
builder.Services.AddDbContext<Fa24Se172594Prn231G1KfsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Koi_museumDB")));

// Initialize PayOS
PayOS payOS = new PayOS(
    builder.Configuration["PayOS:ClientId"] ?? Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID"),
    builder.Configuration["PayOS:ApiKey"] ?? Environment.GetEnvironmentVariable("PAYOS_API_KEY"),
    builder.Configuration["PayOS:ChecksumKey"] ?? Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY")
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7232", "https://localhost:7028")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          );
});

// Register PayOS as a singleton service
builder.Services.AddSingleton(payOS);
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

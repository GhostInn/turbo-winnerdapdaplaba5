using Matuning.Domain.Interfaces;
using Matuning.Infrastructure;
using Matuning.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog (раскомментируйте и настройте при необходимости)
//Log.Logger = new LoggerConfiguration()
  //  // .ReadFrom.Configuration(builder.Configuration)
    //.Enrich.FromLogContext()
  //  .WriteTo.Console()
    //.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    //.CreateLogger();

// builder.Host.UseSerilog();

// Добавление DbContext в контейнер DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев с временем жизни Scoped
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>(); // Если используете репозитории

builder.Services.AddScoped<ICarsRepository, CarsRepository>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();

// Добавление контроллеров и других сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Остальные сервисы и настройки
// Например, настройки CORS, аутентификации и т.д.

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
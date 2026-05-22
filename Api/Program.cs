using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Clients.AppDbContext>(options =>
    options.UseSqlite("Data Source=finance.db"));
// Подключаем контроллеры (чтобы работал FinanceController)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Этот конвертер заставит API принимать и возвращать строки вместо 0 и 1
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Подключаем генерацию Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Принудительно заставляем Swagger отображать enum как строки в схемах
    c.UseInlineDefinitionsForEnums();
});

// Регистрация слоев DI
builder.Services.AddScoped<Models.IAccountRepository, Clients.AccountRepository>();
builder.Services.AddScoped<Models.ITransactionRepository, Clients.TransactionRepository>();
builder.Services.AddScoped<Models.ILimitRepository, Clients.LimitRepository>();

builder.Services.AddScoped<Controllers.FinanceService>();

var app = builder.Build();

// Включаем Swagger UI в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Это создаст страницу по адресу /swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Маппим эндпоинты наших контроллеров
app.MapControllers();

app.Run();
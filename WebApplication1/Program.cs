
using BusinessLogic.Services;
using DataAccess.Wrapper;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JLNest Social Network API",
        Description = "API ��� ���������� ���� JLNest � ������������, ������ � ��������",
        Contact = new OpenApiContact
        {
            Name = "JLNest Support",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://example.com/license")
        }
    });
    //using system.reflection
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<JlnestContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("DataAccess")
    ));

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserInventoryService, UserInventoryService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IStoreItemService, StoreItemService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IMediaService, MediaService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<JlnestContext>();
    
    logger.LogInformation("Ожидание подключения к MySQL...");
    
    int retries = 10;
    for (int i = 0; i < retries; i++)
    {
        try
        {
            logger.LogInformation($"Попытка подключения к MySQL ({i + 1}/{retries})...");
            
            // ПРОСТАЯ ПРОВЕРКА ПОДКЛЮЧЕНИЯ
            if (context.Database.CanConnect())
            {
                logger.LogInformation("Подключение к MySQL успешно!");
                
                // АВТОМАТИЧЕСКОЕ ПРИМЕНЕНИЕ МИГРАЦИЙ
                logger.LogInformation("Применение миграций...");
                context.Database.Migrate();
                logger.LogInformation("Миграции применены успешно!");
                break;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Не удалось подключиться к MySQL. Попытка {i + 1}/{retries}. Ошибка: {ex.Message}");
            if (i == retries - 1)
            {
                logger.LogError("Все попытки подключения исчерпаны!");
                throw;
            }
            Thread.Sleep(5000);
        }
    }
}

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
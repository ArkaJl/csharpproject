using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Interfaces;
using DataAccess.Wrapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<JlnestContext>(
    options => options.UseMySQL("Server=localhost;Port=3306;Database=jlnest;User=root;Password=1234;"));

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
    var context = scope.ServiceProvider.GetRequiredService<JlnestContext>();

    try
    {
        if (context.Database.CanConnect())
        {
            Console.WriteLine("Подключение к БД успешно");

            // Проверим есть ли данные
            var userCount = context.Users.Count();
            Console.WriteLine($"Количество пользователей в БД: {userCount}");
        }
        else
        {
            Console.WriteLine(" Не удалось подключиться к БД");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения: {ex.Message}");
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
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Interfaces;
using DataAccess.Wrapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<JlnestContext>(
    options => options.UseMySQL("Server=localhost;Database=jlnest;User=root;Password=12345;"));


builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<JlnestContext>();

    try
    {
        if (context.Database.CanConnect())
        {
            Console.WriteLine("����������� � �� �������");

            // �������� ���� �� ������
            var userCount = context.Users.Count();
            Console.WriteLine($"���������� ������������� � ��: {userCount}");
        }
        else
        {
            Console.WriteLine(" �� ������� ������������ � ��");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"������ �����������: {ex.Message}");
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



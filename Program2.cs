using Boren.MiniAPI.WebApplication;
using Boren.MiniAPI.WebApplication.Data;
using Boren.MiniAPI.WebApplication.Data.Service;
using Microsoft.EntityFrameworkCore;

// 擴充類別、方法，將Endpoints 註冊集中
public static class Endpoints
{
    public static void MapClassEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/class");
        group.MapGet("/", () => "Get classes list");
    }

    public static void MapStudentEndpoints(this WebApplication app)
    {
        // 1. 可以定義完整routing
        // API 設定 https://{domain}/students/{id} 以HTTPGET 進行呼叫，回傳資料
        app.MapGet("/students/{Id}", (int id) => { return Results.Ok(new Student { Name = "Amy" }); });
        // 2. 也可以分群以便於管理
        // 業務邏輯也可以採用依賴注入
        var group = app.MapGroup("/students");
        group.MapGet("/", (IStudentService service) => service.ReadAll());
        group.MapPost("/", (Student student, IStudentService service) => service.Create(student));
    }
}

public static class Program
{
    static void Main(string[] args)
    {
        // builder 可以註冊DBContext，想當然也可以註冊類別
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
        builder.Services.AddScoped<IStudentService, StudentService>();
        var app = builder.Build();

        // 利用擴充方法綁定Endpoints
        app.MapClassEndpoints();
        app.MapStudentEndpoints();

        app.Run();
    }
}
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
        app.MapGet("/students/{Id}", (int id) =>
        {
            return Results.Ok(new Student { Name = "Amy" });
            //return TypedResults.Ok(new Student { Name = "Amy" });

            // Results 與TypedResults 差異
            // Results 回傳IResult，TypedResults 回傳IResult 的實作
            // 意即IResult 要做型別轉換才能知道實體類別
            // 所以推薦使用TypedResults 以便於程式碼閱讀及測試
        });

        // return Results 需要呼叫Produces 以供OpenApi 產生文件
        // return TypedResults 則不需要
        // app.MapGet("/hello", () => Results.Ok(new Todo() { Name = "Hello World!" }))
        //    .Produces<Todo>();
        // app.MapGet("/hello2", () => TypedResults.Ok(new Todo() { Name = "Hello World!" }));

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
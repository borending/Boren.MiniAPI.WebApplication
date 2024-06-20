using Boren.MiniAPI.WebApplication;
using Microsoft.EntityFrameworkCore;

// builder 可以註冊DBContext，想當然也可以註冊Class
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

// 一、定義完整routing
// API 設定domain/todoitems1 以HTTPGET 進行呼叫，回傳Todos 資料
app.MapGet("/todoitems1", async (TodoDb db) =>
    await db.Todos.ToListAsync());

// 二、使用MapGroup，將routing 分類
var todoItems = app.MapGroup("/todoitems2");
// 等於domain/todoitems2/{id} 走HTTPGET 
todoItems.MapGet("/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound());
// 等於domain/ 走HTTPPOST
todoItems.MapPost("/", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/{todo.Id}", todo);
});

// 三、從lambda 改成獨立方法
static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    // Results 與TypedResults 差異
    // Results 回傳IResult，TypedResults 回傳IResult 的實作
    // 意即IResult 要做型別轉換才能知道實體類別
    // 所以推薦使用TypedResults 以便於程式碼閱讀及測試
    return TypedResults.NotFound();
}
todoItems.MapDelete("/{id}", DeleteTodo);

// return Results 需要呼叫Produces 以供OpenApi 產生文件
// return TypedResults 則不需要
app.MapGet("/hello", () => Results.Ok(new Todo() { Name = "Hello World!" }))
    .Produces<Todo>();
app.MapGet("/hello2", () => TypedResults.Ok(new Todo() { Name = "Hello World!" }));

app.Run();
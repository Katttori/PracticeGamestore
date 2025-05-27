using PracticeGamestore.Business.Dependencies;
using PracticeGamestore.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBusinessServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
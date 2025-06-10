using FluentValidation;
using FluentValidation.AspNetCore;
using PracticeGamestore.Business.Dependencies;
using PracticeGamestore.Middlewares;
using Serilog;
using PracticeGamestore.Filters;

var builder = WebApplication.CreateBuilder(args);

var logFile = builder.Configuration["Logging:File:LogFilePath"] ?? "logs/log.txt";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logFile, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<RequestModelValidationFilter>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


builder.Services.AddControllers();
builder.Services.AddBusinessServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
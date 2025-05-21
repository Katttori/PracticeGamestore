using PracticeGamestore.Business.Dependencies;
using PracticeGamestore.Business.Services.Genre;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddBusinessServices(builder.Configuration);

builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
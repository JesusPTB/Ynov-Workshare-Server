using Microsoft.EntityFrameworkCore;
using Ynov_WorkShare_Server.Context;
using Ynov_WorkShare_Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<WorkShareDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("LocalConnection")
    )
);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using var scope = app.Services.CreateScope();
//var provider = scope.ServiceProvider;
//var context = provider.GetRequiredService<WorkShareDbContext>();
//context.Database.Migrate();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

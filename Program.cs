var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); //swagger
builder.Services.AddSwaggerGen(); // swagger
builder.Services.AddControllers(); //added code 

// Register configuration-based services
builder.Services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();

var app = builder.Build();

// 🔧 Initialize the database at startup
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

    using var connection = factory.CreateConnection();
    connection.Open();

    DbInitializer.Initialize(connection);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); //added code

app.Run();
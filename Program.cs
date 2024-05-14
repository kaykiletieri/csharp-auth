using CSharpAuth.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = null;

if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration["DefaultConnection"];
}

if (connectionString != null)
{
    builder.Services.AddEntityFramework(connectionString);
}
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

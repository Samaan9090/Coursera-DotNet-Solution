using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();  // Error handling middleware first
app.UseMiddleware<AuthenticationMiddleware>(); // Authentication middleware next
app.UseMiddleware<LoggingMiddleware>();       // Logging middleware last

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
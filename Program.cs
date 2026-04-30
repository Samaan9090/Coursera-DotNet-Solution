using UserManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();  // Error handling middleware first
app.UseMiddleware<LoggingMiddleware>();       // Logging middleware before Auth to capture 401s and execution time
app.UseMiddleware<AuthenticationMiddleware>(); // Authentication middleware next

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
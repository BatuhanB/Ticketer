using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketer.Application.Booking.Commands.ReserveSeats;
using Ticketer.Application.Common.Behaviours;
using Ticketer.Application.Common.Interfaces;
using Ticketer.Domain.Booking;
using Ticketer.Infrastructure.Persistence;
using Ticketer.Infrastructure.Repositories;
using Ticketer.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- 1. PRESENTATION LAYER SETUP ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. APPLICATION LAYER SETUP ---
// Register MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(ReserveSeatsCommand).Assembly);
});

// Register FluentValidation Validators
builder.Services.AddValidatorsFromAssembly(typeof(ReserveSeatsCommandValidator).Assembly);

// Register MediatR Pipeline Behavior for Validation
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// --- 3. INFRASTRUCTURE LAYER SETUP ---
// Register DbContext
builder.Services.AddDbContext<TicketerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register Repositories and Unit of Work
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TicketerDbContext>());
builder.Services.AddScoped<IShowInventoryRepository, ShowInventoryRepository>();

var app = builder.Build();

// --- 4. STARTUP TASKS (DOCKER / MIGRATIONS) ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketerDbContext>();
    try
    {
        // Delete the existing database (optional, good for clean testing)
        // dbContext.Database.EnsureDeleted(); 

        // This will forcefully create the tables based on your C# models 
        // without needing ANY migration files!
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Could not create tables: {ex.Message}");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add our custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
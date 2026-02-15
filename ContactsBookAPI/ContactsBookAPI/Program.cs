using ContactsBookAPI.Application.Commands;
using ContactsBookAPI.Infrastructure.Data;
using ContactsBookAPI.Infrastructure.Repositories.ContactRepository;
using ContactsBookAPI.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ContactsBookAPI", Version = "v1" });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateContactCommandHandler).Assembly));

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors();

builder.Services.AddScoped<IContactRepository, ContactRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors(cors => { cors.AllowAnyOrigin(); cors.AllowAnyMethod(); cors.AllowAnyHeader(); });

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();

app.Run();

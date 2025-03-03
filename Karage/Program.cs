#region Using Labs
using Karage.APIs.Extensions;
using Karage.APIs.Middlewares; 
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Added Swagger with JWT Support
builder.Services.AddSwaggerWithJwt(); 

// Add CORS policy
builder.Services.AddAngularCors();
 
var app = builder.Build();

//Add Golobal Error Handling
app.UseMiddleware<ErrorHandlingMiddleware>();

// Apply migrations and seed data using the extension method
app.ApplyMigrationsAndSeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for the Angular app
app.UseCors("AllowAngularApp"); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

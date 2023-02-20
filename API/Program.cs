
using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityService(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
/* builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); */


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder=> builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));


app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
/* if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} */

//app.UseHttpsRedirection(); 
//used for redirecting http request to https

//app.UseAuthorization();
//used for authorized user

app.MapControllers(); //it act as middleware

app.Run();


//this file is entry point the project it act as middleware

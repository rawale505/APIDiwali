using API_Diwali.Repository;
using API_Diwali.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient, MongoClient>(Sp => new MongoClient(builder.Configuration.GetConnectionString("DefaultConection")));
builder.Services.AddSingleton<IMongorepo, MongoRepo>();
builder.Services.AddSingleton<IRedisrepo, Redisrepo>();
builder.Services.AddSingleton<IProductServices, ProductServices>();
builder.Services.AddSingleton<ILoginServices, LoginServices>();
builder.Services.AddSingleton<IAdminServices, AdminServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

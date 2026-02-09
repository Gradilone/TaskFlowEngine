using MongoDB.Driver;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories;
using TaskManager.Data;
using TaskManager.Worker;
using MongoDB.Bson.Serialization.Conventions;

var builder = Host.CreateApplicationBuilder(args);

var mongoConnection = builder.Configuration["MongoDbSettings:ConnectionString"];
var mongoDatabase = builder.Configuration["MongoDbSettings:DatabaseName"];

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnection));
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabase);
});

var pack = new ConventionPack { new CamelCaseElementNameConvention() };
ConventionRegistry.Register("camelCase", pack, t => true);

builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

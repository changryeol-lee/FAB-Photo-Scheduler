using DataMart.SqlMapper;
using SimulationEngine.SimulationEntity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowVueApp",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:9000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

//string���� �������� �� enum���� �Ľ� 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//Data mart connection string���� 
//Data Mart �ν��Ͻ� �ʱ�ȭ (�ѹ��� ����)
var config = builder.Configuration;
var connStr = config.GetConnectionString("DefaultConnection");
InputMart.Initialize(connStr);
OutputMart.Initialize(connStr);
// DI ���
builder.Services.AddSingleton<InputMart>(provider => InputMart.Instance);
builder.Services.AddSingleton<OutputMart>(provider => OutputMart.Instance);
builder.Services.AddSingleton<SimFactory>(provider => SimFactory.Instance);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("AllowVueApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:7042");

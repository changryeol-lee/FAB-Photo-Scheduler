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
                          policy.WithOrigins(
                                     "http://localhost:9000",
                                     "http://localhost:9001",
                                     "http://localhost:9002", 
                                     "http://localhost:88",
                                     "http://52.79.235.205:88"   // EC2 퍼블릭 IP 
                                 )
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

//string으로 보내줬을 때 enum으로 파싱 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//Data mart connection string전달 
//Data Mart 인스턴스 초기화 (한번만 실행)
var config = builder.Configuration;
var connStr = config.GetConnectionString("DefaultConnection");
InputMart.Initialize(connStr);
OutputMart.Initialize(connStr);
// DI 등록
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

app.Run();

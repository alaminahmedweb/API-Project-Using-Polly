using API_Project_With_Polly;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var timeout=Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IWeatherServie, WeatherService>(c =>
{
    c.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
}).AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
.AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)))
.AddPolicyHandler(request =>
{
    if(request.Method == HttpMethod.Get)
    {
        return timeout;
    }
    return Policy.NoOpAsync<HttpResponseMessage>();
});

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

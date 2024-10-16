using Hangfire;
using HangFireAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHangfire(config =>
    config.UseInMemoryStorage()); // For production, use a persistent store like SQL Server or Redis
builder.Services.AddHangfireServer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// Enable Hangfire Dashboard
app.UseHangfireDashboard();

// Map Hangfire dashboard route

app.UseHangfireDashboard("/hangfire");
app.MapControllers();
app.MapHangfireDashboard();
var jobService = new SampleJobService();
//once when run
//BackgroundJob.Enqueue(() => jobService.ExecuteJob());

RecurringJob.AddOrUpdate("run every minute job",
    () => jobService.ExecuteJob()
    , Cron.Minutely);

app.Run();

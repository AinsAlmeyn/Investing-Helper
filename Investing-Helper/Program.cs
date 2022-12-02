using Hangfire;
using Hangfire.MemoryStorage;
using Investing_Helper.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Hangfire Settings and  Dependency Injections

    builder.Services.AddScoped<IInvestingJobs, InvestingJobs>();
    builder.Services.AddTransient<IMailService, MailService>();
    builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());
    builder.Services.AddHangfireServer();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "Hisse Senedi Backround Jobs",
    AppPath = "/",
});

//# field #   meaning        allowed values
//# -------   ------------   --------------
//#    1      minute         0-59
//#    2      hour           0-23
//#    3      day of month   1-31
//#    4      month          1-12 (or names, see below)
//#    5      day of week    0-7 (0 or 7 is Sun, or use names)

var cron = Cron.Daily(15, 10);
RecurringJob.AddOrUpdate<IInvestingJobs>("PostStockPrices",
  sp => sp.GetStockPrice(), cron);

app.UseAuthorization();
app.MapControllers();
app.Run();

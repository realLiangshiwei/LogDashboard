using Serilog.Events;
using Serilog;
using LogDashboard;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File($"{AppContext.BaseDirectory}Log/.log", rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:HH:mm:ss} || {Level} || {SourceContext:l} || {Message} ||{NewLine} {Exception} ||end {NewLine}")
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddLogDashboard();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseLogDashboard();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

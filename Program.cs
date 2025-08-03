using ImageCompressionAPI.Services;
using ImageCompressionAPI.Models;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add ML.NET context
builder.Services.AddSingleton<MLContext>();

// Add custom services
builder.Services.AddScoped<IImageCompressionService, ImageCompressionService>();
builder.Services.AddScoped<IMLModelService, MLModelService>();

// Register HttpClient for ML service communication
builder.Services.AddHttpClient<ImageCompressionService>();

// Configure file upload limits
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 50 * 1024 * 1024; // 50MB
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseRouting();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();

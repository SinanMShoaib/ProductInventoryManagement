var builder = WebApplication.CreateBuilder(args);

// 1. Add CORS support so your frontend HTML can make API calls
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// 2. Tell the engine to map your ProductsController class
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the pipeline for testing via Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Enable the CORS policy middleware
app.UseCors();

// 4. Cleanly bind your API routes
app.MapControllers();

app.Run();
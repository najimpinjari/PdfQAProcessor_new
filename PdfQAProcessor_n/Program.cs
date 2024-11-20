using PdfQAProcessor_n.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<PdfService>();  // Register PdfService for PDF text extraction
builder.Services.AddSingleton<HuggingFaceService>();  // Register HuggingFaceService for answering questions
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation
builder.Services.AddControllers();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Adds Swagger middleware
    app.UseSwaggerUI(); // Adds Swagger UI middleware
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

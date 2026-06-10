using Memiz.Api.Factories;
using Memiz.Api.Interfaces;
using Memiz.Api.Prompts;
using Memiz.Api.Providers;
using Memiz.Api.Services;
using Memiz.Api.Templates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Add new services
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IDictionaryProvider, MockDictionaryProvider>();
//AI Provider
builder.Services.AddHttpClient<OpenAiProvider>();
builder.Services.AddScoped<IAiProvider, MockAiProvider>();
builder.Services.AddScoped<IAiProvider, OpenAiProvider>();
//AI Prompt
builder.Services.AddScoped<AiPromptFactory>();


builder.Services.AddScoped<DictionaryProviderFactory>();
builder.Services.AddScoped<AiProviderFactory>();
builder.Services.AddScoped<ICardTemplate, BasicVocabularyTemplate>();
builder.Services.AddScoped<TemplateFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cross-Origin Resource Sharing (CORS) configuration for frontend development
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();




// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors("FrontendDev");
}

app.UseAuthorization();

app.MapControllers();

app.Run();

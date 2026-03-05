using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MottuFlowApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MottuFlowApi.Services;
using MottuFlowApi.Swagger;
using DotNetEnv;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Banco de Dados
// ----------------------
if (builder.Environment.EnvironmentName != "Testing")
{
    Env.Load();
}

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__OracleConnection");

if (builder.Environment.EnvironmentName != "Testing" && !string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString));
}
else if (builder.Environment.EnvironmentName != "Testing")
{
    throw new InvalidOperationException("Connection string Oracle não encontrada!");
}

// ----------------------
// Machine Learning
// ----------------------
builder.Services.AddSingleton(new MLContext());
builder.Services.AddSingleton<MotoMlService>();

// ----------------------
// Versionamento da API
// ----------------------
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = false;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ----------------------
// Autenticação JWT
// ----------------------
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


// ----------------------
// Swagger
// ----------------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MottuFlow API",
        Version = "v1",
        Description = "API de gerenciamento de motos"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Coloque o seu token no formato: Bearer {token}"
    });

    // Filtros personalizados
    options.OperationFilter<SwaggerSecurityRequirementsFilter>();
    options.OperationFilter<SwaggerAllowAnonymousFilter>();
    options.DocumentFilter<Documentacao>();
    options.DocumentFilter<OrdenarTagsDocumentFilter>();
    options.EnableAnnotations();
});

// Health Check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("BancoOracle");

// ----------------------
// Controllers / Endpoints
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

var app = builder.Build();

// ----------------------
// Middleware e Swagger
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MottuFlow API v1");
        c.RoutePrefix = "swagger";
    });
}

// Redireciona a raiz para o Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Endpoints de health check
app.MapGet("/api/health/ping", () => Results.Ok(new { status = "API rodando" }));
app.MapHealthChecks("/api/health");

app.MapControllers();
app.Run();

public partial class Program { }
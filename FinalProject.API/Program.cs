using System.Text;
using DataAccess.Repositorio;
using FinalProject.Aplicacao;
using FinalProject360.Aplicacao;
using FinalProject360.Aplicacao.Interfaces;
using FinalProject360.Aplicacao.Servicos;
using FinalProject360.Repositorio.Contexto;
using FinalProject360.Repositorio.Interfaces;
using FinalProject360.Repositorio.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados SQL Server
builder.Services.AddDbContext<BarbeariaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registro das camadas de Aplicação
builder.Services.AddScoped<IServicoAplicacao, ServicoAplicacao>();
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IAgendamentoAplicacao, AgendamentoAplicacao>();

// Registro das camadas de Repositório
builder.Services.AddScoped<IServicoRepositorio, ServicoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAgendamentoRepositorio, AgendamentoRepositorio>();

// Injeção do JwtService
builder.Services.AddSingleton(x =>
{
    var config = builder.Configuration.GetSection("Jwt");
    return new JwtService(
        config["Key"],
        config["Issuer"]
    );
});

// Configuração da autenticação JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero // Sem tolerância para expiração
    };
});

// Configuração do CORS para permitir acesso do front-end local
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Front-end React.js
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adiciona os serviços de controllers
builder.Services.AddControllers();

// Adiciona Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurações do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(); // Habilita CORS no ambiente de desenvolvimento
}

app.UseHttpsRedirection();

// 🟢 ATIVA AUTENTICAÇÃO E AUTORIZAÇÃO
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

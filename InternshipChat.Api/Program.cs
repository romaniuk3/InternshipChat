using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using InternshipChat.Api.Extensions;
using InternshipChat.Api.Hubs;
using InternshipChat.BLL.Mapping;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = string.Empty;

if (builder.Environment.IsProduction())
{
    var keyVaultUrl = new Uri(builder.Configuration.GetSection("KeyVaultURL").Value!);
    var azureCredential = new DefaultAzureCredential();
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, azureCredential);
    connectionString = builder.Configuration.GetSection("azuresqlconnectionstring").Value!;
    var signalRConnectionString = builder.Configuration.GetSection("azuresignalrconnectionstring").Value;
    builder.Services.AddSignalR().AddAzureSignalR(signalRConnectionString);
} else if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("Default")!;
    builder.Services.AddSignalR();
}

builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserChatsRepository, UserChatsRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7226", "https://internshipchat-test-client.azurewebsites.net", "https://localhost")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddRoles<ApplicationRole>()
  .AddEntityFrameworkStores<ChatContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.ConfigureValidators();

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

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub").RequireAuthorization();

app.Run();

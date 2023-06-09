using Blazored.LocalStorage;
using InternshipChat.WEB;
using InternshipChat.WEB.Services;
using InternshipChat.WEB.Services.Auth;
using InternshipChat.WEB.Services.Contracts;
using InternshipChat.WEB.StateContainers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string appBase = builder.Configuration["AppBase"]!;

builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<CallStateContainer>();

builder.Services.AddScoped(serviceProvider => new HttpClient
{
    BaseAddress = new Uri(appBase)
});

builder.Services.AddMudServices();

await builder.Build().RunAsync();
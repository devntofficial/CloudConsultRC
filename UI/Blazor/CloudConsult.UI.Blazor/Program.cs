using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Interfaces.Consultation;
using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.States.Authentication;
using CloudConsult.UI.Services.Consultation;
using CloudConsult.UI.Services.Doctor;
using CloudConsult.UI.Services.Identity;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IKycService, KycService>();
builder.Services.AddScoped<ITimeSlotService, TimeSlotService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://cloudconsult.apigateway") });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddScoped<Radzen.DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(LoginState).Assembly).UseReduxDevTools());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

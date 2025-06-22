using AutoriaBot;
using AutoriaBot.Services;
using AutoriaBot.Telegram;
using AutoriaBot.UserData;
using AutoriaBotNew;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserDataStore>();
builder.Services.AddSingleton<AutoRiaService>(_ => new AutoRiaService(apiKey: Constants.AutoRiaApiKey));
builder.Services.AddSingleton<GoogleMapsService>(_ => new GoogleMapsService(Constants.GoogleMapsApiKey));
builder.Services.AddSingleton<AutoriaBot.Services.VinDecoderService>();
builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

// ⚡️ ВАЖЛИВО: створюємо екземпляр бота — інакше він не стартує!
var tgBot = app.Services.GetRequiredService<TelegramBotService>();

app.Run();

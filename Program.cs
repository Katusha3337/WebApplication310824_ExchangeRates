using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Список поддерживаемых валют и их курсы обмена к гривне (UAH)
var exchangeRatesToUah = new Dictionary<string, decimal>
{
    { "USD", 41.10m }, // Пример курса обмена USD к UAH
    { "EUR", 43.50m }, // Пример курса обмена EUR к UAH
    { "GBP", 50.00m }  // Пример курса обмена GBP к UAH
};

// Эндпоинт для получения списка поддерживаемых валют
app.MapGet("/supportedCurrencies", () =>
{
    var currencies = exchangeRatesToUah.Keys;
    return Results.Json(currencies);
});

// Эндпоинт для получения текущих курсов обмена к гривне (UAH)
app.MapGet("/exchangeRate/{fromCurrency}", (string fromCurrency) =>
{
    if (exchangeRatesToUah.ContainsKey(fromCurrency))
    {
        var rate = exchangeRatesToUah[fromCurrency];
        return Results.Json(new { fromCurrency, toCurrency = "UAH", rate });
    }
    else
    {
        return Results.Json(new { error = "Unsupported currency or exchange rate not found." }, statusCode: 400);
    }
});

// Эндпоинт для конвертации суммы из одной валюты в гривну (UAH)
app.MapGet("/convertCurrency/{fromCurrency}/{amount}", (string fromCurrency, decimal amount) =>
{
    if (exchangeRatesToUah.ContainsKey(fromCurrency))
    {
        var rate = exchangeRatesToUah[fromCurrency];
        var convertedAmount = amount * rate;
        return Results.Json(new { fromCurrency, toCurrency = "UAH", amount, convertedAmount });
    }
    else
    {
        return Results.Json(new { error = "Unsupported currency or exchange rate not found." }, statusCode: 400);
    }
});

// Запуск приложения
app.Run();

//https://localhost:7275/supportedCurrencies
//https://localhost:7275/exchangeRate/USD
//https://localhost:7275/convertCurrency/USD/100

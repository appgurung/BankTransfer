using BankTransfer.API.Extensions;
using BankTransfer.API.Providers;
using BankTransfer.API.Services;
using BankTransfer.Core.Helpers;
using BankTransfer.Core.Interface;

var builder = WebApplication.CreateBuilder(args);

//IOC container for services
builder.Services.AddCBAServices();

var app = builder.Build();

// Configure the HTTP request middleware pipeline.
app.UseCBAMiddleware(builder.Environment);

app.Run();

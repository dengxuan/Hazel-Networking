// See https://aka.ms/new-console-template for more information
using Hazel.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddHostedService<Server>();
var app = builder.Build();
await app.RunAsync();
// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;

var configurationRoot = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
 //   .AddJsonFile("secrets.json")
    .Build();

var test = configurationRoot["Test"];
Console.ReadLine();
    
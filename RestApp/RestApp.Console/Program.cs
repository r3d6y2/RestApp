using System;
using Microsoft.Extensions.DependencyInjection;
using RestApp;
using RestApp.Logging;

var services = new ServiceCollection();

services.AddSingleton<ILogger, ConsoleLogger>();

services.AddSingleton<IRestClient, RestAppClient>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger>();
    var innerRestClient = new RestClient();
    return new RestAppClient(innerRestClient, logger, 3, 1000);
});



Console.WriteLine("Hello, World!");
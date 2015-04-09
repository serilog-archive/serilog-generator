using System;
using System.Collections.Concurrent;
using System.Linq;
using Serilog.Generator.Actors;
using Serilog.Generator.Configuration;
using Serilog.Generator.Enrichers;
using Serilog.Generator.Model;

namespace Serilog.Generator
{
    static class Program
    {
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.CommandLine()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "e-Commerce")
                .Enrich.With<SerialNumberEnricher>()
                .MinimumLevel.Debug()
                .CreateLogger();

            const int initialCustomers = 1;
            Log.Information("Simulation starting with {InitialCustomers} initial customers...", initialCustomers);

            var catalog = new Catalog();

            var customers = new ConcurrentBag<Customer>(Enumerable.Range(0, initialCustomers)
                .Select(_ => new Customer(catalog)));

            var traffic = new TrafficReferral(customers, catalog);
            var admin = new Administrator(catalog);

            foreach (var c in customers)
                c.Start();

            admin.Start();
            traffic.Start();

            Log.Information("Simulation running, press any key to exit.");
            var k = Console.ReadKey(true);
            Log.Information("Simulation stopped ({@KeyInfo})", k);
        }
    }
}

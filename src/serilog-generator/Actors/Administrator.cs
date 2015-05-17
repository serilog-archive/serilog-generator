using System;
using Serilog.Generator.Model;

namespace Serilog.Generator.Actors
{
    public class Administrator
    {
        readonly Catalog _catalog;
        readonly ActiveAgent _ai;
        readonly string _name;
        readonly ILogger _log = Log.ForContext<Administrator>();

        public Administrator(Catalog catalog)
        {
            _catalog = catalog;
            _ai = new ActiveAgent(100000, ViewCatalog, ChangePrice);
            _name = Fake.String("Administrator");
        }

        public void Start()
        {
            RequestLog("POST", "/users/authenticate").Information("Administrator {AdministratorName} logging on", _name);
            _ai.Start();
        }

        ILogger RequestLog(string method, string rawUrl)
        {
            var rq = Guid.NewGuid();
            var m = Machine.Choose();
            var log = _log.ForContext("RequestId", rq).ForContext("MachineName", m);
            log.Debug("Processing {HttpMethod} request for {RawUrl}", method, rawUrl);
            return log;
        }

        void ViewCatalog()
        {
            var log = RequestLog("GET", "/catalog?region=NAS");
            var item = _catalog.GetProduct(log);
            log.Information("{AdministratorName} requested current price data for {ItemName}", _name, item.Name);
        }

        void ChangePrice()
        {
            var log = RequestLog("PUT", "/catalog/67899876");
            var item = _catalog.GetProduct(log);
            var rnd = new Random();
            var newPrice = (item.ItemCost * (decimal)rnd.NextDouble()) + (item.ItemCost * (decimal)rnd.NextDouble());
            log.Information("Administrator {AdministratorName} submitting price change for {ProductName} to ${NewPrice:0.00}", _name, item.Name, newPrice);
            if (newPrice < 1m)
            {
                log.Warning("New price rejected; discount rules would not be maintained");
                return;
            }
            item.ItemCost = newPrice;
            log.Information("Price changed, effective {ChangedAtDate}", DateTime.UtcNow.Date.AddDays(2));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Generator.Model;

namespace Serilog.Generator.Actors
{
    class Customer : IDisposable
    {
        readonly Catalog _catalog;
        readonly ActiveAgent _ai;
        readonly string _name;
        readonly IList<CartItem> _cart = new List<CartItem>();
        readonly ILogger _log = Log.ForContext<Customer>();

        decimal _totalPurchases;

        public Customer(Catalog catalog)
        {
            _catalog = catalog;
            _ai = CreateProfile();
            _name = Fake.Name();
        }

        ActiveAgent CreateProfile()
        {
            var profile = new Random().Next() % 10;
            switch (profile)
            {
                case 0:
                    return new ActiveAgent(10000, ViewCatalog, AddItem, RemoveItem, Clear, CheckOut);
                case 1:
                case 2:
                case 3:
                case 4:
                    return new ActiveAgent(30000, ViewCatalog, ViewCatalog, ViewCatalog, AddItem, Clear);
                case 9:
                    return new ActiveAgent(60000, CrashViewingCatalog);
                default:
                    return new ActiveAgent(6000, ViewCatalog, ViewCatalog, AddItem, RemoveItem, CheckOut);
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public void Start()
        {
            RequestLog("POST", "/users/authenticate").Information("New customer {CustomerName} logging on", _name);
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

        void CrashViewingCatalog()
        {
            ViewCatalog();
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("optimisticConcurrencyToken");
        }

        void ViewCatalog()
        {
            var log = RequestLog("GET", "/catalog?region=NAS");
            var item = _catalog.GetProduct(log);
            log.Information("{CustomerName} provided time-limited discount on {ItemName}", _name, item.Name);
        }

        void AddItem()
        {
            var log = RequestLog("POST", "/cart");
            var item = _catalog.GetProduct(log);
            log.Information("{CustomerName} adding {ItemName} to cart", _name, item.Name);
            var tax = item.TaxRate * item.ItemCost;
            log.Information("Pre-discount tax total calculated at ${TaxAmount:0.00}", tax);
            var ci = new CartItem { Description = item.Name, Total = tax + item.ItemCost };
            _cart.Add(ci);
            log.Information("Added {@CartItem} to cart; cart contains {CartSize} items", ci, _cart.Count);
        }

        void RemoveItem()
        {
            var log = RequestLog("DELETE", "/cart/689069");
            log.Debug("Cart loaded from memory cache in {TimeMS}", 6.98);
            if (_cart.Count == 0)
            {
                log.Error("No matching item found in {CustomerName} cart to remove", _name);
                return;
            }

            var item = _cart.Last();
            log.Information("{CustomerName} removing {@CartItem} from cart", _name, item);
            _cart.Remove(item);
            log.Information("Item removed, {CartSize} items left", _cart.Count);
        }

        void Clear()
        {
            var log = RequestLog("POST", "/cart/_clear");
            log.Debug("Clearing {CustomerName} cart", _name);
            if (_cart.Count == 0)
            {
                log.Warning("Cart is already empty - clear operation has no effect");
                return;
            }

            log.Information("Removing {CartSize} items from {CustomerName} cart", _cart.Count, _name);
            _cart.Clear();
            log.Information("Cart is empty");
        }

        void CheckOut()
        {
            var log = RequestLog("POST", "/purchase/checkout");
            log.Debug("Customer {CustomerName} checking out", _name);
            if (_cart.Count == 0)
            {
                log.Error("No items in the customer's cart can be checked out");
                return;
            }

            var total = _cart.Sum(c => c.Total);
            log.Information("Checking out {CartSize} items for ${Total:0.00}", _cart.Count, total);
            _totalPurchases += total;
            _cart.Clear();
            log.Information("Customer paid using {PaymentMethod}", "CreditCard");
        }

        public void Dispose()
        {
            _ai.Dispose();
            RequestLog("POST", "/user/logoff").Information("Customer {CustomerName} logged off after ${TotalPurchases:0.00} spend", _name, _totalPurchases);
        }
    }
}
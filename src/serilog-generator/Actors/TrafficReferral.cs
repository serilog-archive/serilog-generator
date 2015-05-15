using System;
using System.Collections.Concurrent;
using Serilog.Generator.Model;

namespace Serilog.Generator.Actors
{
    public class TrafficReferral
    {
        readonly ConcurrentBag<Customer> _customers;
        readonly Catalog _catalog;
        readonly Random _random = new Random();
        readonly ActiveAgent _ai;

        public TrafficReferral(ConcurrentBag<Customer> customers, Catalog catalog)
        {
            if (customers == null) throw new ArgumentNullException("customers");
            if (catalog == null) throw new ArgumentNullException("catalog");
            _customers = customers;
            _catalog = catalog;
            _ai = new ActiveAgent(1000, NewVisit, EndVisit);
        }

        public void Start()
        {
            Log.Information("Traffic referral starting");
            _ai.Start();
        }

        void NewVisit()
        {
            var tod = 0.2 * Math.Abs(Math.Sin(DateTime.Now.TimeOfDay.TotalMinutes));
            var rnd = _random.NextDouble();
            var activity = tod + rnd;
            if (activity < 0.6) return;

            var cust = new Customer(_catalog);
            _customers.Add(cust);
            cust.Start();

            Log.Information("Imported customer record {CustomerName} from {IdentityStore}", cust.Name, "crm-xmpsvc-978");
        }

        void EndVisit()
        {
            if (_random.NextDouble() < 0.5) return;

            Customer removed;
            if (_customers.TryTake(out removed))
            {
                removed.Dispose();
            }
        }
    }
}
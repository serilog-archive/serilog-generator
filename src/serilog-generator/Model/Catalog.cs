using System;
using System.Collections.Generic;

namespace Serilog.Generator.Model
{
    class Catalog
    {
        readonly IList<Product> _products;
        readonly Random _random = new Random();
 
        public Catalog()
        {
            _products = new List<Product>
            {
                new Product { Id = 1543, ItemCost = 4.5m, Name = "Auto-Takumar 105mm f2.5", TaxRate = 0 },
                new Product { Id = 25432543, ItemCost = 34.95m, Name = "Super Takumar 35mm f3.5", TaxRate = 0.1m },
                new Product { Id = 54323, ItemCost = 109.45m, Name = "Super Takumar 35mm f2.0", TaxRate = 0.1m },
                new Product { Id = 4543, ItemCost = 9.05m, Name = "Super-Multi-Coated Takumar 50mm f1.4", TaxRate = 0 },
                new Product { Id = 55432, ItemCost = 1.89m, Name = "Skylight Filter 49mm", TaxRate = 0.1m },
                new Product { Id = 65342, ItemCost = 3.00m, Name = "M42/NEX Adapter", TaxRate = 0.1m }
            };
        }

        public Product GetProduct(ILogger log)
        {
            var idx = _random.Next() % _products.Count;
            var result = _products[idx];
            log.ForContext<Catalog>().Debug("Loaded {ProductId} from database in {TimeMS} ms", result.Id, (idx + 1) * 7);
            return result;
        }
    }
}
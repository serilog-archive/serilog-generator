using System;

namespace Serilog.Generator.Model
{
    class CartItem
    {
        public CartItem() { Id = Guid.NewGuid(); }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
    }
}
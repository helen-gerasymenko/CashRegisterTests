using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalRelay.Assignment.Discounts
{
    public class BulkDiscount : IDiscount
    {
        public readonly int Quantity;
        readonly List<Product> _bulkDiscountProducts = new List<Product>();

        public BulkDiscount(IEnumerable<Product> bulkDiscountProducts, int quantity)
        {
            if (bulkDiscountProducts == null)
                throw new ApplicationException("bulkDiscountProducts argument cannot be null.");

            if (!bulkDiscountProducts.Any())
                throw new ApplicationException("Bulk Discount Products list cannot be empty.");

            if (bulkDiscountProducts != null)
                _bulkDiscountProducts = bulkDiscountProducts.ToList();

            if (quantity <= 0)
            {
                throw new ApplicationException("Quantity cannot be negative or 0.");
            }

            Quantity = quantity;

        }

        public decimal Calculate(IEnumerable<CartProduct> cartProducts, decimal cartSubtotal)
        {
            if (cartProducts == null)
                throw new ApplicationException("No products specified.");

            if (!cartProducts.Any())
                throw new ApplicationException("No products specified.");

            Dictionary <Product, int> productCounters = new Dictionary<Product, int>();

            foreach (var cartProduct in cartProducts)
            {
                if (!_bulkDiscountProducts.Contains(cartProduct.Product))
                    continue;

                if (!productCounters.ContainsKey(cartProduct.Product))
                {
                    productCounters.Add(cartProduct.Product, 1);
                }
                else
                {
                    productCounters[cartProduct.Product]++;
                }
            }

            // TODO: need to implement further calculation for Can_get_several_bulk_discounts()
            var discount = 0m;
            foreach (var counter in productCounters)
            {
                if (counter.Value >= Quantity)
                {
                    discount += counter.Key.Price;
                }
            }

            return discount;
        }
    }
}
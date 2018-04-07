using System;
using System.Collections.Generic;
using System.Linq;
using GlobalRelay.Assignment.Discounts;

namespace GlobalRelay.Assignment
{
    public class CashRegister
    {
        readonly Dictionary<string, Product> _inventory = new Dictionary<string, Product>();
        readonly List<CartProduct> _shoppingCart = new List<CartProduct>();
        readonly List<IDiscount> _discounts = new List<IDiscount>();

        
        public CashRegister(IEnumerable<Product> products, 
                            IEnumerable<IDiscount> discounts = null)
        {
            if (products == null)
                throw new ApplicationException("Products argument cannot be null.");

            if (!products.Any())
                throw new ApplicationException("Products list cannot be empty.");

            /* TODO: need to implement further validation for test case
               CashRegister_throws_on_invalid_inventory():
               two products with the same code should not be accepted
            */

            foreach (var product in products)
            {
                _inventory.Add(product.Code, product);
            }
            if (discounts != null)
                _discounts = discounts.ToList();
        }

        public void ClearShoppingCart()
        {
            _shoppingCart.Clear();
        }

        public bool Scan(string productCode, double? weightKg=null, string couponCode=null)
        {
            if (!_inventory.ContainsKey(productCode))
                return false;

            var product = _inventory[productCode];

            if (product.IsPriceByWeight && weightKg == null)
                throw new ApplicationException("Product weight was not specified.");

            var cartProduct = new CartProduct(product, weightKg);
            _shoppingCart.Add(cartProduct);

            return true;
        }

        public decimal CalculatePrice()
        {
            decimal subTotal = 0.0m;
            foreach (var item in _shoppingCart)
            {
                if (item.Product.IsPriceByWeight)
                {
                    if (!item.WeightKg.HasValue)
                        throw new ApplicationException("Inconsistency in data. No product weight specified.");

                    subTotal += item.Product.Price * (decimal) item.WeightKg.Value;
                }
                else
                {
                    subTotal += item.Product.Price;
                }
            }
            decimal totalDiscount = 0.0m;
            foreach (var discount in _discounts)
            {
                totalDiscount += discount.Calculate(_shoppingCart, subTotal);
            }
            return subTotal - totalDiscount;
        }
    }
}


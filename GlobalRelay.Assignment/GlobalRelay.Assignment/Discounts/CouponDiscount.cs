using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalRelay.Assignment.Discounts
{
    public class CouponDiscount : IDiscount
    {
        public readonly decimal Threshold;
        public readonly decimal Discount;

        public CouponDiscount(decimal threshold, decimal discount)
        {
            if (threshold <= 0 || discount <= 0)
            {
                throw new ApplicationException("Threshold or discount cannot be negative or 0.");
            }

            Threshold = threshold;
            Discount = discount;
        }

        public decimal Calculate(IEnumerable<CartProduct> cartProducts, decimal cartSubtotal)
        {
            if (cartProducts == null)
                throw new ApplicationException("No products specified.");

            if (!cartProducts.Any())
                throw new ApplicationException("No products specified.");

            if (cartSubtotal <= 0)
                throw new ApplicationException("Invalid cart subtotal specified.");

            return cartSubtotal >= Threshold ? Discount : 0m;
        }

    }
}
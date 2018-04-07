using System.Collections.Generic;

namespace GlobalRelay.Assignment.Discounts
{
    public interface IDiscount
    {
        decimal Calculate(IEnumerable<CartProduct> cartProducts, decimal cartSubtotal);
    }
}
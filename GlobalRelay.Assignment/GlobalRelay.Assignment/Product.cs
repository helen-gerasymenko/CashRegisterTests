using System;

namespace GlobalRelay.Assignment
{
    public class Product
    {
        public string Code { get; }
        public string Name { get; }
        public decimal Price { get; }
        public bool IsPriceByWeight { get; }

        public Product(string code, string name, decimal price, bool isPriceByWeight = false)
        {
            if (string.IsNullOrEmpty(code))
                throw new ApplicationException("Product code or name cannot be null.");

            if (!IsDigitsOnly(code))
                throw new ApplicationException("Product code should contain numbers only.");

            if (string.IsNullOrEmpty(name))
                throw new ApplicationException("Product name or name cannot be null.");

            if (price < 0)
                throw new ApplicationException("Product price cannot be negative.");

            Code = code;
            Name = name;
            Price = price;
            IsPriceByWeight = isPriceByWeight;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

    }
}
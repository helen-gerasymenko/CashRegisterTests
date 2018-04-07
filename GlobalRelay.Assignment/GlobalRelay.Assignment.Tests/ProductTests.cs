using System;
using NUnit.Framework;

namespace GlobalRelay.Assignment.Tests
{
    [TestFixture]
    public class ProductTests
    {

        [Test]
        public void Can_instantiate_valid_product()
        {
            //setup
            const string code = "9501101530003";
            const string name = "Cheerios";
            const decimal price = 6.99m;

            //act
            var product = new Product(code, name, price);

            Assert.That(product.Code, Is.EqualTo(code));
            Assert.That(product.Name, Is.EqualTo(name));
            Assert.That(product.Price, Is.EqualTo(price));
        }

        [Test]
        public void Cannot_instantiate_product_with_invalid_code()
        {
            //setup
            const string emptyCode = "";
            const string invalidCode = "adz300";

            Assert.Throws<ApplicationException>(() => new Product(emptyCode, name: "Cheerios", price: 6.99m));
            Assert.Throws<ApplicationException>(() => new Product(invalidCode, name: "Cheerios", price: 6.99m)); 

        }

        [Test]
        public void Cannot_instantiate_product_with_negative_price()
        {
            //setup
            const decimal negativePrice = -6.99m;

            Assert.Throws<ApplicationException>(() => new Product(code:"9501101530003", name: "Cheerios", price: negativePrice));
        }
    }
}

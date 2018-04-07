using System;
using System.Collections.Generic;
using GlobalRelay.Assignment.Discounts;
using NUnit.Framework;

namespace GlobalRelay.Assignment.Tests
{
    [TestFixture]
    public class BulkDiscountTests
    {
        [Test]
        public void Can_instantiate_BulkDiscount_class()
        {
            var product = new Product(code: "9501101530002", name: "Cheerios", price: 6.99m);

            Assert.DoesNotThrow(() => new BulkDiscount(new[] { product }, 5));
        }

        [Test]
        public void Cannot_instantiate_BulkDiscount_class_with_invalid_products_list()
        {
            Assert.Throws<ApplicationException>(() => new BulkDiscount(bulkDiscountProducts:null, quantity:5));
            Assert.Throws<ApplicationException>(() => new BulkDiscount(bulkDiscountProducts: new Product[] { }, quantity:5));
        }

        [Test]
        public void Cannot_instantiate_BulkDiscount_class_with_negative_quantity()
        {
            var product = new Product(code: "9501101530002", name: "Cheerios", price: 6.99m);
            Assert.Throws<ApplicationException>(() => new BulkDiscount(bulkDiscountProducts: new[] { product }, quantity:- 1));
            Assert.Throws<ApplicationException>(() => new BulkDiscount(bulkDiscountProducts: new[] { product }, quantity: 0));
        }

        [Test]
        public void Cannot_calculate_bulk_discount_for_invalid_cart_products_list()
        {
            //setup
            var bulkDiscountProduct = new Product(code: "9501101530003", name: "Nutella", price: 5);
            var bulkDiscountList = new BulkDiscount(bulkDiscountProducts: new[] { bulkDiscountProduct }, quantity: 2);

            Assert.Throws<ApplicationException>(() =>
                bulkDiscountList.Calculate(cartProducts: null, cartSubtotal: 100));
            Assert.Throws<ApplicationException>(() =>
                bulkDiscountList.Calculate(cartProducts: new List<CartProduct>(), cartSubtotal: 100));
        }

    }
}

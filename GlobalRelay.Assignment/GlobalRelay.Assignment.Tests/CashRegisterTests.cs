using System;
using GlobalRelay.Assignment.Discounts;
using NUnit.Framework;

namespace GlobalRelay.Assignment.Tests
{
    [TestFixture]
    public class CashRegisterTests
    {
        [Test]
        public void Can_calculate_price_per_item()
        {
            //setup
            var product = new Product(code: "9501101530002", name:"Cheerios", price: 6.99m);
            var sut = new CashRegister(new [] { product});

            //act
            sut.Scan(product.Code);
            var totalPrice = sut.CalculatePrice();

            Assert.That(totalPrice, Is.EqualTo(product.Price));
        }

        [Test]
        public void Can_calculate_price_based_on_product_weight()
        {
            //setup
            var product = new Product(code: "9501101530003", name: "Red Delicious apple", price: 2, isPriceByWeight: true);
            var sut = new CashRegister(new[] { product });

            //act
            sut.Scan(product.Code, weightKg: 0.3);
            var totalPrice = sut.CalculatePrice();

            Assert.That(totalPrice, Is.EqualTo(0.6));
        }

        [Test]
        public void Can_calculate_price_for_products_per_item_and_weight()
        {
            //setup
            var productWithPricePerItem = new Product(code: "9501101530002", name: "Cheerios", price: 6.99m);
            var productWithPricePerWeight = new Product(code: "9501101530003", name: "Red Delicious apple", price: 2, isPriceByWeight: true);
            var sut = new CashRegister(new[] { productWithPricePerItem, productWithPricePerWeight });

            //act
            sut.Scan(productWithPricePerItem.Code);
            sut.Scan(productWithPricePerWeight.Code, weightKg: 0.3);
            var totalPrice = sut.CalculatePrice();

            Assert.That(totalPrice, Is.EqualTo(7.59));
        }

        [Test]
        public void Can_calculate_price_for_bulk_discount_product()
        {
            //setup
            var product = new Product(code: "9501101530004", name: "Nutella", price: 5);
            var bulkDiscountList = new BulkDiscount(bulkDiscountProducts: new[] { product}, quantity: 3);
            var sut = new CashRegister(products: new[] { product }, discounts: new [] { bulkDiscountList } );

            //act
            sut.Scan(product.Code);
            sut.Scan(product.Code);
            sut.Scan(product.Code);
            var totalPrice = sut.CalculatePrice();

            //3 * 5 - 5 = 10
            Assert.That(totalPrice, Is.EqualTo(10m));
        }

        [Test]
        public void Can_calculate_price_for_bulk_discount_product_and_regular_price_product()
        {
            //setup
            var regularPriceProduct = new Product(code: "9501101530004", name: "Chewing gum", price: 1.50m);
            var bulkDiscountProduct = new Product(code: "9501101530003", name: "Nutella", price: 5);
            var bulkDiscountList = new BulkDiscount(bulkDiscountProducts: new[] { bulkDiscountProduct }, quantity: 2);
            var sut = new CashRegister(products: new[] { regularPriceProduct, bulkDiscountProduct}, discounts: new[] { bulkDiscountList });

            //act
            sut.Scan(regularPriceProduct.Code);
            sut.Scan(regularPriceProduct.Code);
            sut.Scan(bulkDiscountProduct.Code);
            sut.Scan(bulkDiscountProduct.Code);
            sut.Scan(bulkDiscountProduct.Code);
            var totalPrice = sut.CalculatePrice();

            //(1.50 * 2) + (5 * 3) - 5 = 13
            Assert.That(totalPrice, Is.EqualTo(13m));
        }

        [Test]
        public void Cannot_calculate_total_for_nonexisting_item()
        {
            //setup
            const string nonExistingProductCode = "9501101530004";
            var product = new Product(code: "9501101530003", name: "Cheerios", price: 6.99m);
            var sut = new CashRegister(new[] { product });

            //act
            var productExists = sut.Scan(nonExistingProductCode);
            var totalPrice = sut.CalculatePrice();

            Assert.IsFalse(productExists);
            Assert.That(totalPrice, Is.EqualTo(0.0m));
        }

        [Test]
        public void CashRegister_throws_on_empty_inventory()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                new CashRegister(new Product[] { });
            });
            Assert.Throws<ApplicationException>(() =>
            {
                new CashRegister(null);
            });
        }

        [Test, Ignore("Needs further implementation")]
        public void CashRegister_throws_on_invalid_inventory()
        {
            
            var product1 = new Product(code: "9501101530007", name: "Apple juice", price: 2m);
            var product2 = new Product(code: "9501101530007", name: "Orange juice", price: 2m);

            Assert.Throws<ApplicationException>(() => new CashRegister(new[] {product1, product2}));
        }

        [Test]
        public void Can_get_discount_with_coupon()
        {
            //setup
            var product1 = new Product(code: "9501101530005", name: "Cheese set", price: 30m);
            var product2 = new Product(code: "9501101530006", name: "Hennessy cognac", price: 70);
            var discount = new CouponDiscount(threshold: 100m, discount: 5m);
            var sut = new CashRegister(new[] { product1, product2 }, new [] { discount });

            //act
            sut.Scan(product1.Code);
            sut.Scan(product2.Code);

            var totalPrice = sut.CalculatePrice();

            Assert.That(totalPrice, Is.EqualTo(95));
        }

        [Test, Ignore("Needs further implementation")]
        public void Can_get_several_bulk_discounts()
        {
            /* TODO: need to implement further calculation for cases
               like the following:
               if product quantity required to get a discount is 2, but
               shopping cart has 4 such products
            */

            //setup
            var product = new Product(code: "9501101530004", name: "Nutella", price: 5);
            var bulkDiscountProduct = new BulkDiscount(bulkDiscountProducts: new[] { product }, quantity: 2);
            var sut = new CashRegister(new[] { product }, new[] { bulkDiscountProduct });

            //act
            sut.Scan(product.Code);
            sut.Scan(product.Code);
            sut.Scan(product.Code);
            sut.Scan(product.Code);

            var totalPrice = sut.CalculatePrice();

            Assert.That(totalPrice, Is.EqualTo(10));

        }
    }
}

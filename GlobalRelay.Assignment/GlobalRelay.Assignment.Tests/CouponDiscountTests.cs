using System;
using System.Collections.Generic;
using GlobalRelay.Assignment.Discounts;
using NUnit.Framework;

namespace GlobalRelay.Assignment.Tests
{
    [TestFixture]
    public class CouponDiscountTests
    {
        [Test]
        public void Can_instantiate_CouponDiscount_class()
        {
            Assert.DoesNotThrow(() => new CouponDiscount(threshold: 100, discount: 5));
        }

        [Test]
        public void Cannot_instantiate_CouponDiscount_class_with_invalid_threshold()
        {
            Assert.Throws<ApplicationException>(() => new CouponDiscount(threshold: 0, discount: 5));
            Assert.Throws<ApplicationException>(() => new CouponDiscount(threshold: -1, discount: 5));
        }

        [Test]
        public void Cannot_instantiate_CouponDiscount_class_with_invalid_discount()
        {
            Assert.Throws<ApplicationException>(() => new CouponDiscount(threshold: 100, discount: 0));
            Assert.Throws<ApplicationException>(() => new CouponDiscount(threshold: 100, discount: -1));
        }

        [Test]
        public void Cannot_calculate_coupon_discount_for_empty_cart_products_list()
        {
            //setup
            var couponDiscount= new CouponDiscount(threshold: 100, discount: 5);

            Assert.Throws<ApplicationException>(() =>
                couponDiscount.Calculate(cartProducts: null, cartSubtotal: 100));
            Assert.Throws<ApplicationException>(() =>
                couponDiscount.Calculate(cartProducts: new List<CartProduct>(), cartSubtotal: 100));
        }

        [Test]
        public void Cannot_calculate_coupon_discount_for_invalid_cart_subTotal()
        {
            //setup
            var couponDiscount = new CouponDiscount(threshold: 100, discount: 5);
            var cartProduct = new CartProduct(new Product(code: "9501101530002", name: "Cheerios", price: 6.99m));

            Assert.Throws<ApplicationException>(() =>
                couponDiscount.Calculate(cartProducts: new List<CartProduct>() { cartProduct }, cartSubtotal: 0));
            Assert.Throws<ApplicationException>(() =>
                couponDiscount.Calculate(cartProducts: new List<CartProduct>() { cartProduct }, cartSubtotal: -100));
        }

    }
}

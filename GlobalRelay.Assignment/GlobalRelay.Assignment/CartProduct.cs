namespace GlobalRelay.Assignment
{
    public class CartProduct
    {
        public Product Product { get; }
        public double? WeightKg { get; }

        public CartProduct(Product product, double? weight=null)
        {
            Product = product;
            WeightKg = weight;
        }
    }
}
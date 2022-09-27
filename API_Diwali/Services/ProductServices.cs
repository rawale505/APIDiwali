using API_Diwali.Model;
using API_Diwali.Repository;
using API_Diwali.Response;
using MongoDB.Bson;

namespace API_Diwali.Services
{
   
    public interface IProductServices
    {
        object GetAllCategoriesServices();
        public List<Product> GetCategoryByIdServices(string _id);
        public object GetProductByNameServices(string productName);
        //public object BillingServices(string EmailId, string ProductName, int ProductCount);
        public object DescendingPriceServices(string CategoryName);
        public object AscendingPriceServices(string CategoryName);
        public object GetCartServices(string EmailId);
        public object CartCountServices(string EmailId);

        public object NestedAddToCartServices(string EmailId, string ProductName, double ProductPrice, string Image, string CategoryName);
        public object NestedGetCartServices(string EmailId);
        public double NestedCartCount(string EmailId);
        public object OrderHistoryServices(string EmailId);
        public object ProductIncrementServices(string EmailId, string ProductName);
        public object ProductDecrementServices(string EmailId, string ProductName);

        public object GetOederdHistory(string EmailId);
        public object NestedRemoveItemFromCartServices(string EmailId, string ProductName);
    }
    public class ProductServices:IProductServices
    {
        public IMongorepo _ProductRepository;
        public IRedisrepo _RedisRepository;
        public ProductServices(IMongorepo mongorepo, IRedisrepo redisRepository)
        {
            _ProductRepository = mongorepo;
            _RedisRepository = redisRepository;
        }
        public object GetAllCategoriesServices()
        {
            List<CategoryModel> Categories = _ProductRepository.GetAllCategories();
            return Categories;
        }
        public List<Product> GetCategoryByIdServices(string _id)
        {
            return _ProductRepository.GetCategoryById(_id);
        }
        public object GetProductByNameServices(string productName)
        {
            return _ProductRepository.GetProductByName(productName);
        }
        //public object BillingServices(string EmailId, string ProductName, int ProductCount)
        //{
        //    return _ProductRepository.AddBillingDetails(EmailId, ProductName, ProductCount);
        //}
        public object DescendingPriceServices(string CategoryName)
        {
            List<ProductResponse> Products = _ProductRepository.DescendingPrice(CategoryName);
            return Products;
        }
        public object AscendingPriceServices(string CategoryName)
        {
            List<ProductResponse> Products = _ProductRepository.AscendingPrice(CategoryName);
            return Products;
        }
        //Cart
        public object GetCartServices(string EmailId)
        {
            return _ProductRepository.GetCart(EmailId);
        }
        public object CartCountServices(string EmailId)
        {
            return _ProductRepository.CartCount(EmailId);
        }

        //Nested Cart
        public object NestedAddToCartServices(string EmailId, string ProductName, double ProductPrice, string Image, string CategoryName)
        {
            return _ProductRepository.NestedAddToCart(EmailId,ProductName,ProductPrice, Image,CategoryName);
        }
        public object NestedGetCartServices(string EmailId)
        {
            return _ProductRepository.NestedGetCart(EmailId) ;
        }
        public double NestedCartCount(string EmailId)
        {
            return _ProductRepository.NestedCartCount(EmailId);
        }
        public object OrderHistoryServices(string EmailId)
        {
            return _ProductRepository.OrderHistory(EmailId);
        }
        public object ProductIncrementServices(string EmailId, string ProductName)
        {
            return _ProductRepository.ProductIncrement(EmailId,ProductName);
        }
        public object ProductDecrementServices(string EmailId, string ProductName)
        {
            return _ProductRepository.ProductDecrement(EmailId,ProductName);
        }
       


        public object GetOederdHistory(string EmailId)
        {
            return _ProductRepository.GetOederdHistory(EmailId);
        }
        public object NestedRemoveItemFromCartServices(string EmailId, string ProductName)
        {
            return _ProductRepository.NestedRemoveItemFromCart(EmailId,ProductName);
        }
    }
    }



using API_Diwali.Model;
using API_Diwali.Response;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API_Diwali.Repository
{
    public interface IMongorepo
    {
        //====================UserLoginStart===================================
        public object RegisterUserMongo(string username, string password, string emailId, string Role);
        //=====================================================================
        public List<CategoryModel> GetAllCategories();
        public List<Product> GetCategoryById(string _id);
        public ProductResponse GetProductByName(string productName);
        public List<ProductResponse> DescendingPrice(string CategoryName);
        public List<ProductResponse> AscendingPrice(string CategoryName);
        public List<CartModel> GetCart(string EmailId);
        public double CartCount(string EmailId);
        public object NestedAddToCart(string EmailId, string ProductName, double ProductPrice, string Image, string CategoryName);
        public List<Product> NestedGetCart(string EmailId);
        public object NestedRemoveItemFromCart(string EmailId, string ProductName);
        public double NestedCartCount(string EmailId);
        public object OrderHistory(string EmailId);

        public List<Product> GetOederdHistory(string EmailId);
        public object ProductIncrement(string EmailId, string ProductName);
        public object ProductDecrement(string EmailId, string ProductName);
        //----------------------------ADMIN------------------------------------
        public CategoryModel AddCategory(CategoryModel categoryModel);
        public Products AddProductToCategory(string CategoryName, Products product);
    }
    public class MongoRepo : IMongorepo
    {
        public IMongoCollection<CategoryModel> _ProductCollection;
        public IMongoCollection<BiillingModel> _OrderHistoryCollection;
        public IMongoCollection<CartModel> _CartCollection;
        public IMongoCollection<NestedCartModel> _NestedCartCollection;
        public IMongoCollection<NestedCartModel> _BillingCollection;
        public IMongoCollection<UserLoginModel> _UserCollection;
        public MongoRepo(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Poc");
            var ProductCollection = database.GetCollection<CategoryModel>("#Diwali");
            var OrderHistoryCollection = database.GetCollection<BiillingModel>("#DiwaliOrderHistory");
            var CartCollection = database.GetCollection<CartModel>("#DiwaliCart");
            var NestedCartCollection = database.GetCollection<NestedCartModel>("#DiwaliNestedCart");
            var BillingCollection = database.GetCollection<NestedCartModel>("#DiwaliBilling");
            var UserCollection = database.GetCollection<UserLoginModel>("#DiwaliUsers");
            _ProductCollection = ProductCollection;
            _OrderHistoryCollection = OrderHistoryCollection;
            _CartCollection = CartCollection;
            _NestedCartCollection = NestedCartCollection;
            _UserCollection = UserCollection;
            _BillingCollection = BillingCollection; 
        }
        //DateTime 
        public static TimeZoneInfo India_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        DateTime dateTime_Indian = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, India_Standard_Time);

        //====================UserLoginStart===================================
        public object RegisterUserMongo(string username, string password, string emailId, string Role)
        {
            UserLoginModel user = new UserLoginModel();
            user.EmailId = emailId;
            user.UserName = username;
            user.Password = password;
            user.Role = Role;
            _UserCollection.InsertOne(user);
            return user;
        }
        //====================LoginEnd=====================================
        public List<CategoryModel> GetAllCategories()
        {
            try
            {
                List<CategoryModel> categories;
                categories = _ProductCollection.Find(x => x.IsActive == true).ToList();
                return categories;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public List<Product> GetCategoryById(string _id)
        {
            try
            {
                var filter = Builders<CategoryModel>.Filter.Eq("_id", _id);
                var result = _ProductCollection.Aggregate().Match(filter).Unwind("ProductList").ToList();
                if (result.Count > 0)
                {
                    List<Product> list = new List<Product>();
                    foreach (var item in result)
                    {
                        Product product = new Product();
                        product.CategoryName = item["_id"].AsString;
                        product.ProductName = item["ProductList"]["ProductName"].AsString;
                        product.Price = Convert.ToDouble(item["ProductList"]["Price"]);
                        product.Image = item["ProductList"]["Image"].AsString;
                        list.Add(product);
                    }
                    return list;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public ProductResponse GetProductByName(string productName)
        {
            try
            {
                BsonDocument projection = new BsonDocument();
                projection.Add("ProductList.ProductName", 1.0);
                projection.Add("ProductList.Price", 1.0);
                var filter = Builders<BsonDocument>.Filter.Eq("ProductList.ProductName", productName);
                var Product = _ProductCollection.Aggregate().Unwind("ProductList").Match(filter).Project(projection).FirstOrDefault();
                //return Product.ToString();
                ProductResponse response = new ProductResponse();
                response._id = Product["_id"].ToString();
                response.ProductName = Product["ProductList"]["ProductName"].ToString();
                response.Price = Product["ProductList"]["Price"].ToDouble();
                return response;
            }
            catch (Exception)
            {

                return null;
            }


        }
        public List<ProductResponse> DescendingPrice(string CategoryName)
        {
            try
            {
                var filter = Builders<CategoryModel>.Filter.Eq("_id", CategoryName);
                BsonDocument projection = new BsonDocument();
                projection.Add("ProductList.ProductName", 1.0);
                projection.Add("ProductList.Price", 1.0);
                var sort = Builders<BsonDocument>.Sort.Descending("ProductList.Price");
                var option = new FindOptions<BsonDocument, BsonDocument>()
                {
                    Sort = sort
                };
                List<ProductResponse> DesecendingProducts = new List<ProductResponse>();
                var Productlist = _ProductCollection.Aggregate().Match(filter).Unwind("ProductList").Sort(sort).Project(projection).ToList();
                foreach (var item in Productlist)
                {
                    ProductResponse response = new ProductResponse();
                    response._id = item["_id"].ToString();
                    response.ProductName = item["ProductList"]["ProductName"].ToString();
                    response.Price = item["ProductList"]["Price"].ToDouble();
                    DesecendingProducts.Add(response);
                }
                return DesecendingProducts;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<ProductResponse> AscendingPrice(string CategoryName)
        {
            try
            {
                var filter = Builders<CategoryModel>.Filter.Eq("_id", CategoryName);
                BsonDocument projection = new BsonDocument();
                projection.Add("ProductList.ProductName", 1.0);
                projection.Add("ProductList.Price", 1.0);
                var sort = Builders<BsonDocument>.Sort.Ascending("ProductList.Price");
                var option = new FindOptions<BsonDocument, BsonDocument>()
                {
                    Sort = sort
                };
                List<ProductResponse> AsecendingProducts = new List<ProductResponse>();
                var Productlist = _ProductCollection.Aggregate().Match(filter).Unwind("ProductList").Sort(sort).Project(projection).ToList();
                foreach (var item in Productlist)
                {
                    ProductResponse response = new ProductResponse();
                    response._id = item["_id"].ToString();
                    response.ProductName = item["ProductList"]["ProductName"].ToString();
                    response.Price = item["ProductList"]["Price"].ToDouble();
                    AsecendingProducts.Add(response);
                }
                return AsecendingProducts;
            }
            catch (Exception)
            {

                return null;
            }
        }
        //Cart
        public List<CartModel> GetCart(string EmailId)
        {
            try
            {
                var filter = Builders<CartModel>.Filter.Eq("EmailId", EmailId);
                var cart = _CartCollection.Find(filter).ToList();
                List<CartModel> result = new List<CartModel>();
                foreach (var item in cart)
                {
                    CartModel cartModel = new CartModel();
                    cartModel.EmailId = item.EmailId;
                    cartModel.ProductName = item.ProductName;
                    cartModel.ProductPrice = item.ProductPrice;
                    result.Add(cartModel);
                }
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public double CartCount(string EmailId)
        {
            try
            {
                var filter = Builders<CartModel>.Filter.Eq("EmailId", EmailId);
                var cartCount = _CartCollection.Find(filter).Count();
                return cartCount;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        //Nested Cart
        public object NestedAddToCart(string EmailId, string ProductName, double ProductPrice, string Image, string CategoryName)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var cart = _NestedCartCollection.Find(filter).FirstOrDefault();
                if (cart == null)
                {
                    NestedCartModel nestedCart = new NestedCartModel();
                    nestedCart._id = EmailId;
                    List<CartItem> items = new List<CartItem>();
                    CartItem item = new CartItem();
                    item.CategoryName = CategoryName;
                    item.ProductName = ProductName;
                    item.ProductPrice = ProductPrice;

                    item.ProductPricePerItem = ProductPrice;
                    item.ProductCount = 1;
                    item.Image = Image;
                    nestedCart.BillAmount = item.ProductPrice;
                    nestedCart.TotalProductCount = item.ProductCount;
                    items.Add(item);
                    nestedCart.CartItems = items;
                    _NestedCartCollection.InsertOne(nestedCart);
                    return nestedCart;
                }
                else
                {
                    var Productfilter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("CartItems.ProductName", ProductName));
                    var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").Match(Productfilter).FirstOrDefault();
                    if (ProductExists == null)
                    {
                        List<CartItem> items = new List<CartItem>();
                        CartItem item = new CartItem();
                        item.CategoryName = CategoryName;
                        item.ProductName = ProductName;
                        item.ProductPrice = ProductPrice;
                        item.ProductPricePerItem = ProductPrice;
                        item.Image = Image;
                        item.ProductCount = 1;
                        items.Add(item);

                        var update = Builders<NestedCartModel>.Update.Push("CartItems", item);
                        _NestedCartCollection.FindOneAndUpdate(filter, update);
                        //bill
                        var billamount = FinalBill(EmailId);
                        var Udate1 = Builders<NestedCartModel>.Update.Set("BillAmount", billamount);
                        _NestedCartCollection.FindOneAndUpdate(filter, Udate1);
                        //count
                        var TotalProductCount = NestedCartCount(EmailId);
                        var UdateTotalProductCount = Builders<NestedCartModel>.Update.Set("TotalProductCount", TotalProductCount);
                        _NestedCartCollection.FindOneAndUpdate(filter, UdateTotalProductCount);
                        return item;
                    }
                    else
                    {
                        CartItem item = new CartItem();
                        item.ProductCount = ProductExists["CartItems"]["ProductCount"].ToDouble();
                        var Price = ProductExists["CartItems"]["ProductPricePerItem"].ToDouble();
                        item.ProductCount++;
                        item.ProductPrice = Price * item.ProductCount;
                        //var billamount = FinalBill(EmailId);
                        var update = Builders<NestedCartModel>.Update.Set("CartItems.$.ProductCount", item.ProductCount).Set("CartItems.$.ProductPrice", item.ProductPrice)/*.Set("BillAmount", billamount)*/;
                        _NestedCartCollection.UpdateOne(ProductExists, update);
                        var billamount = FinalBill(EmailId);
                        var Update1 = Builders<NestedCartModel>.Update.Set("BillAmount", billamount);
                        _NestedCartCollection.FindOneAndUpdate(filter, Update1);
                        //count
                        var TotalProductCount = NestedCartCount(EmailId);
                        var UdateTotalProductCount = Builders<NestedCartModel>.Update.Set("TotalProductCount", TotalProductCount);
                        _NestedCartCollection.FindOneAndUpdate(filter, UdateTotalProductCount);
                        return item;
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        //pull
        public object NestedRemoveCart(string EmailId)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var cart = _NestedCartCollection.DeleteOne(filter);

                return cart;
            }
            catch (Exception)
            {

                return null;
            }
        }
        //Remove Cart item
        public object NestedRemoveItemFromCart(string EmailId, string ProductName)
        {


            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var Productfilter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("CartItems.ProductName", ProductName));
                var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").Match(Productfilter).FirstOrDefault();
                var update1 = Builders<NestedCartModel>.Update.PullFilter(x => x.CartItems, Builders<CartItem>.Filter.Eq(x => x.ProductName, ProductName));
                var result = _NestedCartCollection.UpdateOneAsync(x => x._id.Equals(EmailId), update1).Result;
                CartItem item = new CartItem();
                //var billamount = FinalBill(EmailId);
                var billamount = FinalBill(EmailId);
                var Update1 = Builders<NestedCartModel>.Update.Set("BillAmount", billamount);
                _NestedCartCollection.FindOneAndUpdate(filter, Update1);
                //count
                var TotalProductCount = NestedCartCount(EmailId);
                var UdateTotalProductCount = Builders<NestedCartModel>.Update.Set("TotalProductCount", TotalProductCount);
                _NestedCartCollection.FindOneAndUpdate(filter, UdateTotalProductCount);
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<Product> NestedGetCart(string EmailId)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                List<Product> products = new List<Product>();
                foreach (var item in ProductExists)
                {
                    Product product = new Product();
                    product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                    product.ProductName = item["CartItems"]["ProductName"].ToString();
                    product.Price = item["CartItems"]["ProductPrice"].ToDouble();
                    product.PricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                    product.Quantity = item["CartItems"]["ProductCount"].ToInt32();
                    product.Image = item["CartItems"]["Image"].ToString();
                    product.BillAmount = item["BillAmount"].ToDouble();
                    product.TotalProductCount = item["TotalProductCount"].ToDouble();
                    products.Add(product);
                }
                return products;
            }
            catch (Exception)
            {

                return null;
            }



        }
        //cart count
        public double NestedCartCount(string EmailId)
        {
            try
            {
                var Productfilter = Builders<NestedCartModel>.Filter.And(Builders<NestedCartModel>.Filter.Eq("_id", EmailId));
                var ProductExists = _NestedCartCollection.Aggregate().Match(Productfilter).Unwind("CartItems").ToList();
                var ProductsCount = (double)ProductExists.Count;
                return ProductsCount;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        //cart Bill
        public double FinalBill(string EmailId)
        {
            try
            {
                double BillPrice = 0;
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var cart = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                foreach (var item in cart)
                {
                    BillPrice = BillPrice += item["CartItems"]["ProductPrice"].ToDouble();
                }
                return BillPrice;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public object OrderHistory(string EmailId)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var filterbill = Builders<BiillingModel>.Filter.Eq("_id", EmailId);
                var available = _OrderHistoryCollection.Aggregate().Match(filterbill).FirstOrDefault();

                var map = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                if (available == null)
                {

                    BiillingModel billingModel = new BiillingModel();
                    billingModel._id = EmailId;
                    List<CartItems> items = new List<CartItems>();


                    foreach (var doc in map)
                    {
                        CartItems item = new CartItems();
                        item.CategoryName = doc["CartItems"]["CategoryName"].ToString();
                        item.ProductName = doc["CartItems"]["ProductName"].ToString();
                        item.ProductPrice = doc["CartItems"]["ProductPrice"].ToDouble();
                        item.ProductPricePerItem = doc["CartItems"]["ProductPricePerItem"].ToDouble();
                        item.ProductCount = doc["CartItems"]["ProductCount"].ToInt32();
                        item.Image = doc["CartItems"]["Image"].ToString();
                        item.OrderDate = dateTime_Indian;
                        items.Add(item);
                    }
                    billingModel.CartItems = items;
                    _OrderHistoryCollection.InsertOne(billingModel);
                    BillGenerator(EmailId);
                    _NestedCartCollection.DeleteOne(filter);
                    return billingModel;
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    //var billDetails = _NestedCartCollection.Aggregate().Match(filter).Out("#DiwaliBilling");//
                    //_NestedCartCollection.DeleteOne(filter);                                                //
                    //return billDetails;                                                                     //
                    ////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    foreach (var item in map)
                    {
                        Product product = new Product();
                        product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                        product.ProductName = item["CartItems"]["ProductName"].ToString();
                        product.Price = item["CartItems"]["ProductPrice"].ToDouble();
                        product.PricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                        product.Quantity = item["CartItems"]["ProductCount"].ToInt32();
                        product.Image = item["CartItems"]["Image"].ToString();

                        List<CartItems> items = new List<CartItems>();
                        CartItems cartItem = new CartItems();
                        cartItem.CategoryName = product.CategoryName;
                        cartItem.ProductName = product.ProductName;
                        cartItem.ProductPrice = product.Price;
                        cartItem.ProductPricePerItem = product.PricePerItem;
                        cartItem.Image = product.Image;
                        cartItem.ProductCount = product.Quantity;
                        cartItem.OrderDate = dateTime_Indian;
                        items.Add(cartItem);
                        var update = Builders<BiillingModel>.Update.Push("CartItems", cartItem);
                        _OrderHistoryCollection.FindOneAndUpdate(filterbill, update);
                    }
                    BillGenerator(EmailId);
                    _NestedCartCollection.DeleteOne(filter);
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public object BillGenerator(string EmailId)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var billDetails = _BillingCollection.Find(filter).FirstOrDefault();
                if (billDetails != null)
                {
                    _BillingCollection.DeleteOne(filter);
                    var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                    NestedCartModel nestedCartModel = new NestedCartModel();
                    nestedCartModel._id = EmailId;
                    List<CartItem> cartItems = new List<CartItem>();
                    foreach (var item in ProductExists)
                    {
                        CartItem product = new CartItem();
                        product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                        product.ProductName = item["CartItems"]["ProductName"].ToString();
                        product.ProductPrice = item["CartItems"]["ProductPrice"].ToDouble();
                        product.ProductPricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                        product.ProductCount = item["CartItems"]["ProductCount"].ToInt32();
                        product.Image = item["CartItems"]["Image"].ToString();
                        nestedCartModel.BillAmount = item["BillAmount"].ToDouble();
                        nestedCartModel.TotalProductCount = item["TotalProductCount"].ToDouble();
                        cartItems.Add(product);
                    }
                    nestedCartModel.CartItems = cartItems;
                    _BillingCollection.InsertOne(nestedCartModel);
                    return nestedCartModel;
                }
                else
                {
                    var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                    NestedCartModel nestedCartModel = new NestedCartModel();
                    nestedCartModel._id = EmailId;
                    List<CartItem> cartItems = new List<CartItem>();
                    foreach (var item in ProductExists)
                    {
                        CartItem product = new CartItem();
                        product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                        product.ProductName = item["CartItems"]["ProductName"].ToString();
                        product.ProductPrice = item["CartItems"]["ProductPrice"].ToDouble();
                        product.ProductPricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                        product.ProductCount = item["CartItems"]["ProductCount"].ToInt32();
                        product.Image = item["CartItems"]["Image"].ToString();
                        nestedCartModel.BillAmount = item["BillAmount"].ToDouble();
                        nestedCartModel.TotalProductCount = item["TotalProductCount"].ToDouble();
                        cartItems.Add(product);
                    }
                    nestedCartModel.CartItems = cartItems;
                    _BillingCollection.InsertOne(nestedCartModel);
                    return nestedCartModel;
                }
                //return billDetails;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Get Nested OederdHistory
        public List<Product> GetOederdHistory(string EmailId)
        {
            try
            {
                var filter = Builders<BiillingModel>.Filter.Eq("_id", EmailId);
                var ProductExists = _OrderHistoryCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                List<Product> products = new List<Product>();
                foreach (var item in ProductExists)
                {
                    Product product = new Product();
                    product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                    product.ProductName = item["CartItems"]["ProductName"].ToString();
                    product.Price = item["CartItems"]["ProductPrice"].ToDouble();
                    product.PricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                    product.Quantity = item["CartItems"]["ProductCount"].ToInt32();
                    product.Image = item["CartItems"]["Image"].ToString();
                    product.OrderDate = item["CartItems"]["OrderDate"].AsDateTime;
                    //product.BillAmount = item["BillAmount"].ToDouble();
                    //product.TotalProductCount = item["TotalProductCount"].ToDouble();
                    products.Add(product);
                }
                return products;
            }
            catch (Exception)
            {

                return null;
            }

        }
        public object ProductIncrement(string EmailId, string ProductName)
        {

            var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
            var Productfilter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("CartItems.ProductName", ProductName));
            var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").Match(Productfilter).FirstOrDefault();
            CartItem item = new CartItem();
            item.ProductCount = ProductExists["CartItems"]["ProductCount"].ToDouble();
            if (item.ProductCount > 9)
            {
                return null;
            }
            else
            {
                var Price = ProductExists["CartItems"]["ProductPricePerItem"].ToDouble();
                item.ProductCount++;
                item.ProductPrice = Price * item.ProductCount;
                //var billamount = FinalBill(EmailId);
                var update = Builders<NestedCartModel>.Update.Set("CartItems.$.ProductCount", item.ProductCount).Set("CartItems.$.ProductPrice", item.ProductPrice)/*.Set("BillAmount", billamount)*/;
                _NestedCartCollection.UpdateOne(ProductExists, update);
                var billamount = FinalBill(EmailId);
                var Update1 = Builders<NestedCartModel>.Update.Set("BillAmount", billamount);
                _NestedCartCollection.FindOneAndUpdate(filter, Update1);
                //count
                var TotalProductCount = NestedCartCount(EmailId);
                var UdateTotalProductCount = Builders<NestedCartModel>.Update.Set("TotalProductCount", TotalProductCount);
                _NestedCartCollection.FindOneAndUpdate(filter, UdateTotalProductCount);
                return item;
            }


        }
        public object ProductDecrement(string EmailId, string ProductName)
        {
            var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
            var Productfilter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("CartItems.ProductName", ProductName));
            var ProductExists = _NestedCartCollection.Aggregate().Match(filter).Unwind("CartItems").Match(Productfilter).FirstOrDefault();
            CartItem item = new CartItem();
            item.ProductCount = ProductExists["CartItems"]["ProductCount"].ToDouble();
            var Price = ProductExists["CartItems"]["ProductPricePerItem"].ToDouble();
            if (item.ProductCount < 2)
            {
                return null;
            }
            else
            {
                item.ProductCount--;
                item.ProductPrice = Price * item.ProductCount;
                //var billamount = FinalBill(EmailId);
                var update = Builders<NestedCartModel>.Update.Set("CartItems.$.ProductCount", item.ProductCount).Set("CartItems.$.ProductPrice", item.ProductPrice)/*.Set("BillAmount", billamount)*/;
                _NestedCartCollection.UpdateOne(ProductExists, update);
                var billamount = FinalBill(EmailId);
                var Update1 = Builders<NestedCartModel>.Update.Set("BillAmount", billamount);
                _NestedCartCollection.FindOneAndUpdate(filter, Update1);
                //count
                var TotalProductCount = NestedCartCount(EmailId);
                var UdateTotalProductCount = Builders<NestedCartModel>.Update.Set("TotalProductCount", TotalProductCount);
                _NestedCartCollection.FindOneAndUpdate(filter, UdateTotalProductCount);
                return item;
            }
        }
        public NestedCartModel GetBill(string EmailId)
        {
            try
            {
                var filter = Builders<NestedCartModel>.Filter.Eq("_id", EmailId);
                var ProductExists = _BillingCollection.Aggregate().Match(filter).Unwind("CartItems").ToList();
                NestedCartModel nestedCartModel = new NestedCartModel();
                nestedCartModel._id = EmailId;
                List<CartItem> cartItems = new List<CartItem>();
                foreach (var item in ProductExists)
                {
                    CartItem product = new CartItem();
                    product.CategoryName = item["CartItems"]["CategoryName"].ToString();
                    product.ProductName = item["CartItems"]["ProductName"].ToString();
                    product.ProductPrice = item["CartItems"]["ProductPrice"].ToDouble();
                    product.ProductPricePerItem = item["CartItems"]["ProductPricePerItem"].ToDouble();
                    product.ProductCount = item["CartItems"]["ProductCount"].ToInt32();
                    product.Image = item["CartItems"]["Image"].ToString();
                    nestedCartModel.BillAmount = item["BillAmount"].ToDouble();
                    nestedCartModel.TotalProductCount = item["TotalProductCount"].ToDouble();
                    cartItems.Add(product);
                }
                nestedCartModel.CartItems = cartItems;
                return nestedCartModel;
            }
            catch (Exception)
            {
                return null;
            }

        }


        //------------------------------------------ADMIN--------------------------------//
        public CategoryModel AddCategory(CategoryModel categoryModel)
        {
            try
            {
                var filter = Builders<CategoryModel>.Filter.Eq("_id", categoryModel._id);
                var CatExist = _ProductCollection.Find(filter).FirstOrDefault();
                if (CatExist == null)
                {
                    categoryModel.CategoryId = CategoryCount();
                    categoryModel.CategoryId++;
                    _ProductCollection.InsertOne(categoryModel);
                    return categoryModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        } 
        public Products AddProductToCategory(string CategoryName, Products product)
        {
            try
            {
                var filter = Builders<CategoryModel>.Filter.Eq("_id", CategoryName);
                var prodFilter = Builders<BsonDocument>.Filter.Eq("ProductList.ProductName", product.ProductName);
                var productExist = _ProductCollection.Aggregate().Match(filter).Unwind("ProductList").Match(prodFilter).FirstOrDefault();
                if (productExist == null)
                {
                    List<Products> products = new List<Products>();
                    Products item = new Products();
                    item.ProductName = product.ProductName;
                    item.ProductId = ProductCount(CategoryName);
                    item.ProductId++;
                    item.Image = product.Image;
                    item.IsActive = true;
                    item.Price = product.Price;
                    item.Quantity = product.Quantity;
                    item.CreatedBy = product.CreatedBy;
                    item.CreatedDate = product.CreatedDate;
                    item.UpdatedBy = product.UpdatedBy;
                    item.UpdatedDate = product.UpdatedDate;
                    products.Add(item);
                    var update = Builders<CategoryModel>.Update.Push("ProductList", item);
                    _ProductCollection.FindOneAndUpdate(filter, update);
                    return item;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public double CategoryCount()
        {
            var filter = Builders<CategoryModel>.Filter.Empty;
            var category_Count = _ProductCollection.Find(filter).Count();
            return category_Count;
        }
        public double ProductCount(string CategoryName)
        {
            var filter = Builders<CategoryModel>.Filter.Eq("_id",CategoryName);
            var product_Count = _ProductCollection.Aggregate().Match(filter).Unwind("ProductList").ToList();
            return product_Count.Count();
        }
    }
}

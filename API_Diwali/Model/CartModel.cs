using MongoDB.Bson;

namespace API_Diwali.Model
{
    public class CartModel
    {
        public ObjectId _id { get; set; }
        public string EmailId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
    }
}

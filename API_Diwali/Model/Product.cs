namespace API_Diwali.Model
{
    public class Product
    {
        public string CategoryName { get; set; }
        //public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public double PricePerItem { get; set; }
        public int Quantity { get; set; }
        //public bool IsActive { get; set; }
        public string Image { get; set; }
        public double TotalProductCount { get; set; }
        public double BillAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace API_Diwali.Model
{
    public class CategoryModel
    {
        [Required]
        public string? _id { get; set; }        
        public double? CategoryId { get; set; }       
        public List<Products> ProductList { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class Products
    {
        public double? ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public double? Price { get; set; }
        [Required]
        public double? Quantity { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class ProductValidator : AbstractValidator<Products>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName).NotNull();
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.ProductName).Length(1, 18);
            //RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Quantity).InclusiveBetween(1, 100);
        }
    }
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(x => x._id).NotNull();
            RuleFor(x => x._id).MaximumLength(18);
        }
    }
}

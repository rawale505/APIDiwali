using API_Diwali.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Diwali.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : Controller
    {
        public IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllCategories()
        {
            var products = _productServices.GetAllCategoriesServices();
            if (products == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }
            
        }
        [HttpGet]
        [Route("GetBy_id")]
        public IActionResult GetCategoryById(string _id)
        {
            var category = _productServices.GetCategoryByIdServices(_id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(category);
            }            
        }
        [HttpGet]
        [Route("ProductGetByName")]
        public IActionResult ProductGetByName(string productName)
        {
            var product = _productServices.GetProductByNameServices(productName);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }
     
        [HttpGet]
        [Route("DescendingByPrice")]
        public IActionResult DescendingByPrice(string CategoryName)
        {
            var DescendingProducts= _productServices.DescendingPriceServices(CategoryName);
            if (DescendingProducts == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(DescendingProducts);
            }
        }
        [HttpGet]
        [Route("AscendingByPrice")]
        public IActionResult AscendingByPrice(string CategoryName)
        {
            var AscendingProducts = _productServices.AscendingPriceServices(CategoryName);
            if (AscendingProducts == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(AscendingProducts);
            }
        }

        [HttpGet]
        [Route("GetCart")]
        public IActionResult Get_Cart(string EmailId)
        {
            var cart = _productServices.GetCartServices(EmailId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cart);
            }
        }

        [HttpGet]
        [Route("GetCartCount")]
        public IActionResult Get_Cart_Count(string EmailId)
        {
            var count = _productServices.CartCountServices(EmailId);
            if (count == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(count);
            }
        }

        //nested cart
        [HttpPost]
        [Route("PostNestedCart")]
        public IActionResult Post_NestedCart(string EmailId, string ProductName, double ProductPrice, string Image,string CategoryName)
        {
            var cart = _productServices.NestedAddToCartServices(EmailId, ProductName, ProductPrice, Image,CategoryName);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cart);
            }
        }
        [HttpGet]
        [Route("GetNestedCart")]
        public IActionResult Get_NestedCart(string EmailId)
        {
            var cart = _productServices.NestedGetCartServices(EmailId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cart);
            }
        }

        [HttpGet]
        [Route("GetNestedCartCount")]
        public IActionResult Get_NestedCartCount(string EmailId)
        {
            var cart = _productServices.NestedCartCount(EmailId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cart);
            }
        }
        [HttpPost]
        [Route("OrderHistory")]
        public IActionResult OrderHistory(string EmailId)
        {
            var sold = _productServices.OrderHistoryServices(EmailId);
            if (sold == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(sold);
            }            
        }
    

        [HttpGet]
        [Route("GetOederdHistory")]
        public IActionResult Get_OederdHistory(string EmailId)
        {
            var cart = _productServices.GetOederdHistory(EmailId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cart);
            }
        }
        [HttpPatch]
        [Route("ProductIncrement")]
        public IActionResult ProductIncrement(string EmailId, string ProductName)
        {
            var result = _productServices.ProductIncrementServices(EmailId, ProductName);
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPatch]
        [Route("ProductDecrement")]
        public IActionResult ProductDecrement(string EmailId, string ProductName)
        {
            var result = _productServices.ProductDecrementServices(EmailId, ProductName);
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpDelete]
        [Route("RemoveCartItem")]
        public IActionResult RemoveCartItem(string EmailId, string ProductName)
        {
            var result = _productServices.NestedRemoveItemFromCartServices(EmailId, ProductName);
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(result);
            }
        }
    }
    
}
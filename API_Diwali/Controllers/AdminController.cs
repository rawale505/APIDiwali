using API_Diwali.Model;
using API_Diwali.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Diwali.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        public IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }
        [HttpPost]
        [Route("AddCategory")]
        public IActionResult AddCategory(CategoryModel categoryModel)
        {
            var result = _adminServices.AddCategoryServices(categoryModel);
            if (result == null)
            {
                return BadRequest("Invalid Input");
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost]
        [Route("AddProductToCategory")]
        public IActionResult AddProductToCategory(string CategoryName, Products product)
        {
            var result = _adminServices.AddProductToCategoryServices(CategoryName, product);
            if (result == null)
            {
                return BadRequest("Invalid Input");
            }
            else
            {
                return Ok(result);
            }
        }
    }
}

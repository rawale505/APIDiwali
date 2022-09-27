using API_Diwali.Model;
using API_Diwali.Repository;

namespace API_Diwali.Services
{
    public interface IAdminServices
    {
        public CategoryModel AddCategoryServices(CategoryModel categoryModel);
        public Products AddProductToCategoryServices(string CategoryName, Products product);
    }
    public class AdminServices:IAdminServices
    {
        public IMongorepo _ProductRepository;
        public AdminServices(IMongorepo mongorepo)
        {
            _ProductRepository = mongorepo;
        }
        public CategoryModel AddCategoryServices(CategoryModel categoryModel)
        {
            return _ProductRepository.AddCategory(categoryModel);
        }
        public Products AddProductToCategoryServices(string CategoryName, Products product)
        {
            return _ProductRepository.AddProductToCategory(CategoryName, product);
        }
    }
}

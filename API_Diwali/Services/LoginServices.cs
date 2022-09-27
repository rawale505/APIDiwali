using API_Diwali.Repository;

namespace API_Diwali.Services
{
    public interface ILoginServices
    {
        internal object UserLoginServices(string emailId, string password);
        public object UserRegistrationServices(string Username, string Password, string emailId, string Role);
    }
    public class LoginServices : ILoginServices
    {
        public IMongorepo _ProductRepository;
        public IRedisrepo _RedisRepository;
        public LoginServices(IMongorepo mongorepo, IRedisrepo redisRepository)
        {
            _ProductRepository = mongorepo;
            _RedisRepository = redisRepository;
        }
        //==================================================
        public object UserLoginServices(string emailId, string password)
        {
            var result = _RedisRepository.UserLoginRepo(emailId, password);
            return result;
        }
        public object UserRegistrationServices(string Username, string Password, string emailId, string Role)
        {
            var ExistUser = _RedisRepository.RegistrationProcedure(Username, Password, emailId);
            if (ExistUser == null)
            {
                return null;
            }
            else
            {
                var mongoresult = _ProductRepository.RegisterUserMongo(Username, Password, emailId, Role);
                var result = _RedisRepository.RegisterUserRedis(emailId, Password);
                return result;
            }
        }
    }
}


using API_Diwali.Model;
using StackExchange.Redis;

namespace API_Diwali.Repository
{
    public interface IRedisrepo
    {
        object UserLoginRepo(string emailId, string password);
        public object RegistrationProcedure(string Username, string Password, string emailId);
        public object RegisterUserRedis(string emailId, string Password);
    }
    public class Redisrepo : IRedisrepo
    {
        public IDatabase db;

        public Redisrepo()
        {
            var option = new ConfigurationOptions
            {
                Password = "AdJdf5hr0Q1HnA3djK8YBRYQOCINYGDZ"
            };

            option.EndPoints.Add("redis-18172.c13161.ap-seast-1-mz.ec2.cloud.rlrcp.com:18172");
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(option);
            db = conn.GetDatabase();
        }

        public object UserLoginRepo(string emailId, string password)
        {
            UserLoginModel user = new UserLoginModel();
            user.EmailId = emailId;
            user.Password = password;
            //string emailid = admin.UserName;
            //string password = admin.Password;

            var filterName = db.HashExists("UsernamePackage", user.EmailId);
            if (filterName == true)
            {
                var filterPassword = db.HashGet("UsernamePackage", user.EmailId);

                if (filterPassword == user.Password)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public object RegistrationProcedure(string Username, string Password, string emailId)
        {
            var filterName = db.HashExists("UsernamePackage", emailId);
            if (filterName != true)
            {
                UserLoginModel user = new UserLoginModel();
                user.UserName = Username;
                user.Password = Password;
                user.EmailId = emailId;
                return user;
            }
            else
            {
                Console.WriteLine("Username Already Exists", "Try Something Different username");
                return null;
            }
        }
        public object RegisterUserRedis(string emailId, string Password)
        {
            HashEntry[] User =
            {
                new HashEntry($"{emailId}",$"{Password}")
            };
            db.HashSet("UsernamePackage", User);
            return User;
        }
    }
}

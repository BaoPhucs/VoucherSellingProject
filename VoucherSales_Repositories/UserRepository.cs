using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool Register(string fullname, string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                FullName = fullname,
                Email = email,
                PasswordHash = password,
                RoleId = 3,            // mặc định Customer
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            return UserDAO.Instance.CreateUser(user);
        }

        public User? ValidateLogin(string username, string password)
        {
            return UserDAO.Instance.GetByUsernameAndPassword(username, password);
        }
    }
}

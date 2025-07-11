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
        public bool Register(string fullname, string username, string email, string phone, string password)
        {
            var user = new User
            {
                Username = username,
                FullName = fullname,
                Email = email,
                Phone = phone,
                PasswordHash = password,
                RoleId = 3,            // mặc định Customer
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            return UserDAO.Instance.CreateUser(user);
        }

        public bool UpdateProfile(User user)
        {
            return UserDAO.Instance.UpdateProfile(user);
        }

        public User? ValidateLogin(string username, string password)
        {
            return UserDAO.Instance.GetByUsernameAndPassword(username, password);
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            return UserDAO.Instance.ChangePassword(userId, currentPassword, newPassword);
        }
        public List<User> GetAllUsers()
        {
            return UserDAO.Instance.GetAllUsers();
        }
    }
}

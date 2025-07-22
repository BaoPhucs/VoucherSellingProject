using System;
using System.Collections.Generic;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAllUsers() => UserDAO.Instance.GetAllUsers();
        public User? GetById(int id) => UserDAO.Instance.GetById(id);

        public bool Register(string fullname, string username, string email,
                             string phone, string password, int roleId = 3)
        {
            var user = new User
            {
                Username = username,
                FullName = fullname,
                Email = email,
                Phone = phone,
                PasswordHash = password,
                RoleId = roleId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            return UserDAO.Instance.CreateUser(user);
        }

        public bool UpdateProfile(User user)
            => UserDAO.Instance.UpdateProfile(user);

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
            => UserDAO.Instance.ChangePassword(userId, currentPassword, newPassword);

        public bool Delete(int id)
        {
            try
            {
                using var ctx = new VoucherSalesDbContext();
                var entity = ctx.Users.Find(id);
                if (entity == null) return false;

                ctx.Users.Remove(entity);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User? ValidateLogin(string username, string password)
            => UserDAO.Instance.GetByUsernameAndPassword(username, password);

        public List<int> GetAllRoleIds()
            => UserDAO.Instance.GetAllRoleIds();

        public List<Role> GetAllRoles()
            => UserDAO.Instance.GetAllRoles(); 
    }
}

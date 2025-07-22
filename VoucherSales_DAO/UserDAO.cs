using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class UserDAO
    {
        private static UserDAO _instance;
        private readonly VoucherSalesDbContext _context;
        public UserDAO()
        {
            _context = new VoucherSalesDbContext();
        }

        public static UserDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserDAO();
                }
                return _instance;
            }

        }
        //write GetAllRoleIds
        public List<int> GetAllRoleIds()
        {
            return _context.Roles.Select(r => r.RoleId).ToList();
                                
        }


        public User GetById(int userId) => _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);

        public User GetByUsernameAndPassword(string username, string password)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        }

        public bool CreateUser(User user)
        {
            // kiểm tra trùng username hoặc email
            if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
                return false;

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateProfile(User user)
        {
            var exist = _context.Users.Find(user.UserId);
            if (exist == null) return false;
            exist.FullName = user.FullName;
            exist.Email = user.Email;
            exist.Phone = user.Phone;
            _context.SaveChanges();
            return true;
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var ex = _context.Users.FirstOrDefault(x => x.UserId == userId && x.PasswordHash == currentPassword);
            if (ex == null) return false;
            ex.PasswordHash = newPassword;
            _context.SaveChanges();
            return true;
        }


        public List<User> GetAllUsers()
        {
            //return all users in the list
            using (var ctx = new VoucherSalesDbContext())
            {
                return ctx.Users.ToList();
            }
        }

       
    }
}

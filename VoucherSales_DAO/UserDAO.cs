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
    }
}

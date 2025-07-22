using System;
using System.Collections.Generic;
using System.Linq;
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

        public static UserDAO Instance => _instance ??= new UserDAO();

        public List<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public List<int> GetAllRoleIds()
        {
            return _context.Roles.Select(r => r.RoleId).ToList();
        }

        public User? GetById(int userId)
        {
            return _context.Users.Include(u => u.Role)
                                 .FirstOrDefault(u => u.UserId == userId);
        }

        public User? GetByUsernameAndPassword(string username, string password)
        {
            return _context.Users.Include(u => u.Role)
                                 .FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        }

        public bool CreateUser(User user)
        {
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
            exist.RoleId = user.RoleId;

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
            return _context.Users.Include(u => u.Role).ToList();
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}

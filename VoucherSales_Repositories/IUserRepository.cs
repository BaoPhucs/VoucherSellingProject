using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IUserRepository
    {
        User? ValidateLogin(string username, string password);
        bool Register(string fillname, string username, string email, string phone, string password);
        bool UpdateProfile(User user);
        bool ChangePassword(int userId, string currentPassword, string newPassword);

        User? GetById(int userId);
        //get all user IDs
        List<User> GetAllUsers();


    }
}

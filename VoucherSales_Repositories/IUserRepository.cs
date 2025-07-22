using System.Collections.Generic;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IUserRepository
    {
        User? ValidateLogin(string username, string password);
        bool Register(string fullname, string username, string email, string phone, string password, int roleId = 3);
        bool UpdateProfile(User user);
        bool ChangePassword(int userId, string currentPassword, string newPassword);
        User? GetById(int userId);
        List<User> GetAllUsers();
        bool Delete(int userId);
        List<int> GetAllRoleIds();        
        List<Role> GetAllRoles();         
    }
}

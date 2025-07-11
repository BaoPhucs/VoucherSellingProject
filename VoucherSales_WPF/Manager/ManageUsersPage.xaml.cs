using System.Windows;
using System.Windows.Controls;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_WPF.Manager
{
    public partial class ManageUsersPage : UserControl
    {
        private User? selectedUser;
        private List<User> allUsers = new List<User>();

        public ManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        // Load danh sách người dùng vào DataGrid
        private void LoadUsers()
        {
            allUsers = UserDAO.Instance.GetAllUsers();
            dgUsers.ItemsSource = allUsers;
        }

        // Khi chọn một dòng trong DataGrid
        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = dgUsers.SelectedItem as User;

            if (selectedUser != null)
            {
                txtUsername.Text = selectedUser.Username;
                txtFullName.Text = selectedUser.FullName;
                txtEmail.Text = selectedUser.Email;
                txtPhone.Text = selectedUser.Phone;
                txtPassword.Password = selectedUser.PasswordHash;
            }
        }

        // Thêm người dùng mới
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Username = txtUsername.Text,
                PasswordHash = txtPassword.Password,
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                RoleId = 3, // giả sử 2 là User
                IsActive = true
            };

            bool success = UserDAO.Instance.CreateUser(user);

            if (success)
            {
                MessageBox.Show("Thêm người dùng thành công.");
                LoadUsers();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Username hoặc email đã tồn tại.");
            }
        }

        // Cập nhật thông tin người dùng
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để cập nhật.");
                return;
            }

            selectedUser.FullName = txtFullName.Text;
            selectedUser.Email = txtEmail.Text;
            selectedUser.Phone = txtPhone.Text;

            bool result = UserDAO.Instance.UpdateProfile(selectedUser);

            if (result)
            {
                MessageBox.Show("Cập nhật thành công.");
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        // Xoá người dùng
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để xoá.");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xoá người dùng '{selectedUser.Username}'?", "Xác nhận xoá", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                var ctx = new VoucherSalesDbContext();
                var entity = ctx.Users.Find(selectedUser.UserId);
                if (entity != null)
                {
                    ctx.Users.Remove(entity);
                    ctx.SaveChanges();
                    MessageBox.Show("Xoá thành công.");
                    LoadUsers();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng trong CSDL.");
                }
            }
        }

        // Làm mới form
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            dgUsers.SelectedIndex = -1;
        }
        // Hàm làm sạch TextBox
        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            selectedUser = null;
        }
    }

}

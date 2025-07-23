using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoucherSales_Repositories;

namespace VoucherSales_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private IUserRepository _userRepository = new UserRepository();
        private TextBox txtLoginPassword, txtRegisterPassword, txtRegisterConfirmPassword;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLoginUsername.Text.Trim();
            string password = pwdLoginPassword.Password;

            var user = _userRepository.ValidateLogin(username, password);

            if (user == null)
            {
                MessageBox.Show("Wrong username or password!", "Login failed",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            App.CurrentUser = user;

            Window nextWindow;

            switch (user.Role.RoleName)
            {
                case "Admin":
                    nextWindow = new AdminMainWindow();
                    break;

                case "Staff":
                    nextWindow = new StaffMainWindow(user);
                    break;

                case "Customer":
                    nextWindow = new CustomerMainWindow(user);
                    break;

                default:
                    MessageBox.Show($"Role “{user.Role.RoleName}” không hợp lệ.", "Lỗi phân quyền",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            nextWindow.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // 1. Đọc dữ liệu
            string username = txtRegisterUsername.Text.Trim();
            string fullname = txtRegisterFullName.Text.Trim();
            string email = txtRegisterEmail.Text.Trim();
            string phone = txtRegisterPhone.Text.Trim();
            string pass = pwdRegisterPassword.Password;
            string confirm = pwdRegisterConfirmPassword.Password;

            // 2. Validate cơ bản
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(fullname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (pass != confirm)
            {
                MessageBox.Show("Password and Confirm Password do not match.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Validate email
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Please enter a valid email address (e.g., example@domain.com).", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 4. Validate phone (10 số, bắt đầu bằng 0)
            if (phone.Length != 10 || !phone.StartsWith("0") || !long.TryParse(phone, out _))
            {
                MessageBox.Show("Phone number must be 10 digits and start with 0 (e.g., 0123456789).", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 5. Gọi repository
            bool created = _userRepository.Register(fullname, username, email, phone, pass);
            if (!created)
            {
                MessageBox.Show("Username or Email already exists.", "Register Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 6. Thành công
            MessageBox.Show("Registration successful! Please log in.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear form & chuyển về tab Login
            txtRegisterUsername.Clear();
            txtRegisterEmail.Clear();
            txtRegisterPhone.Clear();
            pwdRegisterPassword.Clear();
            pwdRegisterConfirmPassword.Clear();
            tbiLogin.IsSelected = true;
        }

        private void pwdLoginPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtLoginPassword != null)
                txtLoginPassword.Text = ((PasswordBox)sender).Password;
        }

        private void pwdRegisterPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtRegisterPassword != null)
                txtRegisterPassword.Text = ((PasswordBox)sender).Password;
        }

        private void pwdRegisterConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtRegisterConfirmPassword != null)
                txtRegisterConfirmPassword.Text = ((PasswordBox)sender).Password;
        }

        private void chkShowPasswordLogin_Checked(object sender, RoutedEventArgs e)
        {
            var passwordBox = pwdLoginPassword;
            txtLoginPassword = new TextBox
            {
                Text = passwordBox.Password,
                Height = 30,
                Margin = passwordBox.Margin,
                BorderBrush = passwordBox.BorderBrush,
                Background = passwordBox.Background
            };
            txtLoginPassword.TextChanged += (s, args) => passwordBox.Password = txtLoginPassword.Text; // Thay PasswordChanged bằng TextChanged
            var parent = (StackPanel)passwordBox.Parent;
            int index = parent.Children.IndexOf(passwordBox);
            parent.Children.Remove(passwordBox);
            parent.Children.Insert(index, txtLoginPassword);
        }

        private void chkShowPasswordLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            var passwordBox = new PasswordBox
            {
                Password = txtLoginPassword.Text,
                Height = 30,
                Margin = txtLoginPassword.Margin,
                BorderBrush = txtLoginPassword.BorderBrush,
                Background = txtLoginPassword.Background
            };
            passwordBox.PasswordChanged += pwdLoginPassword_PasswordChanged;
            var parent = (StackPanel)txtLoginPassword.Parent;
            int index = parent.Children.IndexOf(txtLoginPassword);
            parent.Children.Remove(txtLoginPassword);
            parent.Children.Insert(index, passwordBox);
            pwdLoginPassword = passwordBox;
        }

        private void chkShowPasswordRegister_Checked(object sender, RoutedEventArgs e)
        {
            var passwordBox = pwdRegisterPassword;
            txtRegisterPassword = new TextBox
            {
                Text = passwordBox.Password,
                Height = 30,
                Margin = passwordBox.Margin,
                BorderBrush = passwordBox.BorderBrush,
                Background = passwordBox.Background
            };
            txtRegisterPassword.TextChanged += (s, args) => passwordBox.Password = txtRegisterPassword.Text; // Thay PasswordChanged bằng TextChanged
            var parent = (StackPanel)passwordBox.Parent;
            int index = parent.Children.IndexOf(passwordBox);
            parent.Children.Remove(passwordBox);
            parent.Children.Insert(index, txtRegisterPassword);
        }

        private void chkShowPasswordRegister_Unchecked(object sender, RoutedEventArgs e)
        {
            var passwordBox = new PasswordBox
            {
                Password = txtRegisterPassword.Text,
                Height = 30,
                Margin = txtRegisterPassword.Margin,
                BorderBrush = txtRegisterPassword.BorderBrush,
                Background = txtRegisterPassword.Background
            };
            passwordBox.PasswordChanged += pwdRegisterPassword_PasswordChanged;
            var parent = (StackPanel)txtRegisterPassword.Parent;
            int index = parent.Children.IndexOf(txtRegisterPassword);
            parent.Children.Remove(txtRegisterPassword);
            parent.Children.Insert(index, passwordBox);
            pwdRegisterPassword = passwordBox;
        }

        private void chkShowConfirmPasswordRegister_Checked(object sender, RoutedEventArgs e)
        {
            var passwordBox = pwdRegisterConfirmPassword;
            txtRegisterConfirmPassword = new TextBox
            {
                Text = passwordBox.Password,
                Height = 30,
                Margin = passwordBox.Margin,
                BorderBrush = passwordBox.BorderBrush,
                Background = passwordBox.Background
            };
            txtRegisterConfirmPassword.TextChanged += (s, args) => passwordBox.Password = txtRegisterConfirmPassword.Text; // Thay PasswordChanged bằng TextChanged
            var parent = (StackPanel)passwordBox.Parent;
            int index = parent.Children.IndexOf(passwordBox);
            parent.Children.Remove(passwordBox);
            parent.Children.Insert(index, txtRegisterConfirmPassword);
        }

        private void chkShowConfirmPasswordRegister_Unchecked(object sender, RoutedEventArgs e)
        {
            var passwordBox = new PasswordBox
            {
                Password = txtRegisterConfirmPassword.Text,
                Height = 30,
                Margin = txtRegisterConfirmPassword.Margin,
                BorderBrush = txtRegisterConfirmPassword.BorderBrush,
                Background = txtRegisterConfirmPassword.Background
            };
            passwordBox.PasswordChanged += pwdRegisterConfirmPassword_PasswordChanged;
            var parent = (StackPanel)txtRegisterConfirmPassword.Parent;
            int index = parent.Children.IndexOf(txtRegisterConfirmPassword);
            parent.Children.Remove(txtRegisterConfirmPassword);
            parent.Children.Insert(index, passwordBox);
            pwdRegisterConfirmPassword = passwordBox;
        }
    }
}
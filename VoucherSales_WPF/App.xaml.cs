using System.Configuration;
using System.Data;
using System.Windows;

namespace VoucherSales_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static VoucherSales_BO.User CurrentUser { get; set; }
    }

}

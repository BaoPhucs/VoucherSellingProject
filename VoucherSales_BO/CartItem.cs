using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSales_BO
{
    public class CartItem : INotifyCollectionChanged
    {
        public int CartItemId { get; set; }
        public int UserId { get; set; }
        public int VoucherTypeId { get; set; }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(Subtotal));
            }
        }
        public DateTime AddedAt { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        // navigation
        public User User { get; set; }
        public VoucherType VoucherType { get; set; }


        public string VoucherName => VoucherType?.Name ?? "";
        public decimal UnitPrice => VoucherType.DiscountValue;
        public decimal Subtotal => UnitPrice * Quantity;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        protected void OnPropertyChanged(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

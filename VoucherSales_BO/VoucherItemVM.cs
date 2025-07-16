using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoucherSales_BO
{
    public class VoucherItemVM
    {
        public Guid VoucherId { get; set; }
        public string Code { get; set; }           // đã có
        public DateTime ValidTo { get; set; }      // đã có
        public string Location { get; set; }       // mới
        public string Category { get; set; }       // mới
        public string VoucherTypeName { get; set; }// hoặc giá trị, mô tả gì bạn cần
    }
}

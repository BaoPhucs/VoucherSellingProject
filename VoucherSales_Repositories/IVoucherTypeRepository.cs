using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IVoucherTypeRepository
    {
        List<VoucherType> GetAll();

        VoucherType GetByID(int id);

        //add what the DAO methods are doing
        bool CreateVoucherType(VoucherType voucherType);
        bool UpdateVoucherType(VoucherType voucherType);
        bool DeleteVoucherType(int id);

    }
}

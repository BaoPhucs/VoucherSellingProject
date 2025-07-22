using System.Collections.Generic;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IVoucherTypeRepository
    {
        List<VoucherType> GetAll();
        VoucherType GetByID(int id);
        bool Create(VoucherType voucherType);
        bool Update(VoucherType voucherType);
        bool Delete(int id);
    }
}

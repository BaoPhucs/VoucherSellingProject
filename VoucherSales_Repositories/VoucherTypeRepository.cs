using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_Repositories
{
    public class VoucherTypeRepository : IVoucherTypeRepository
    {
        public List<VoucherType> GetAll()
        {
            return VoucherTypeDAO.Instance.GetAllVoucherTypes();
        }

        public VoucherType GetByID(int id)
        {
            return VoucherTypeDAO.Instance.GetById(id);
        }
        public bool CreateVoucherType(VoucherType voucherType)
        {
            return VoucherTypeDAO.Instance.CreateVoucherType(voucherType);
        }
        public bool UpdateVoucherType(VoucherType voucherType)
        {
            return VoucherTypeDAO.Instance.UpdateVoucherType(voucherType);
        }
        public bool DeleteVoucherType(int id)
        {
            return VoucherTypeDAO.Instance.DeleteVoucherType(id);

        }
    }
}

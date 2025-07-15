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
            return VoucherTypeDAO.Instance.GetByID(id);
        }
        public bool Create(VoucherType voucherType)
        {
            return VoucherTypeDAO.Instance.Create(voucherType);
        }
        public bool Update(VoucherType voucherType)
        {
            return VoucherTypeDAO.Instance.Update(voucherType);
        }
        public bool Delete(int id)
        {
            var voucherType = VoucherTypeDAO.Instance.GetByID(id);
            if (voucherType == null) return false;

            try
            {
                VoucherTypeDAO.Instance.Delete(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

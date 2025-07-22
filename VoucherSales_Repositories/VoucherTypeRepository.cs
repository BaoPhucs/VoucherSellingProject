using System;
using System.Collections.Generic;
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
            var voucherType = VoucherTypeDAO.Instance.GetById(id);
            if (voucherType == null) return false;

            return VoucherTypeDAO.Instance.Delete(id);
        }
    }
}

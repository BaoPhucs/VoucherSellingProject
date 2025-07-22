using System;
using System.Collections.Generic;
using System.Linq;
using VoucherSales_BO;

namespace VoucherSales_DAO
{
    public class VoucherTypeDAO
    {
        private static VoucherTypeDAO? _instance;
        private readonly VoucherSalesDbContext _context = new VoucherSalesDbContext();
        public static VoucherTypeDAO Instance => _instance ??= new VoucherTypeDAO();

        private VoucherTypeDAO() { }

        public List<VoucherType> GetAllVoucherTypes()
        {
            using var ctx = new VoucherSalesDbContext();
            return ctx.VoucherTypes.ToList();
        }

        public VoucherType? GetById(int id)
        {
            using var ctx = new VoucherSalesDbContext();
            return ctx.VoucherTypes.Find(id);
        }

        public bool CreateVoucherType(VoucherType voucherType)
        {
            try
            {
                using var ctx = new VoucherSalesDbContext();
                ctx.VoucherTypes.Add(voucherType);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateVoucherType(VoucherType voucherType)
        {
            try
            {
                using var ctx = new VoucherSalesDbContext();
                var existing = ctx.VoucherTypes.Find(voucherType.VoucherTypeId);
                if (existing == null) return false;

                ctx.Entry(existing).CurrentValues.SetValues(voucherType);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteVoucherType(int id)
        {
            try
            {
                using var ctx = new VoucherSalesDbContext();
                var voucherType = ctx.VoucherTypes.Find(id);
                if (voucherType == null) return false;

                ctx.VoucherTypes.Remove(voucherType);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Create(VoucherType vt)
        {
            try
            {
                _context.VoucherTypes.Add(vt);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(VoucherType vt)
        {
            try
            {
                var existing = _context.VoucherTypes.FirstOrDefault(x => x.VoucherTypeId == vt.VoucherTypeId);
                if (existing == null) return false;

                existing.Name = vt.Name;
                existing.Description = vt.Description;
                existing.DiscountType = vt.DiscountType;
                existing.DiscountValue = vt.DiscountValue;
                existing.MinOrderValue = vt.MinOrderValue;
                existing.TotalQuantity = vt.TotalQuantity;
                existing.ValidFrom = vt.ValidFrom;
                existing.ValidTo = vt.ValidTo;
                existing.Category = vt.Category;
                existing.Location = vt.Location;

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var vt = _context.VoucherTypes.Find(id);
                if (vt == null) return false;

                _context.VoucherTypes.Remove(vt);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

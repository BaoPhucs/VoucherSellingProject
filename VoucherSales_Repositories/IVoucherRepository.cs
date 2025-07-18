﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IVoucherRepository
    {
        List<Voucher> GetMyWalletVouchers(int userId);
        void Redeem(Guid voucherId, string location = null);
        void GenerateForOrder(int orderId);
        //write func to add and delete vouchers
        void AddVoucher(Voucher voucher);
        void DeleteVoucher(Guid voucherId);

    }
}

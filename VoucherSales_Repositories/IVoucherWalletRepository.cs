﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoucherSales_BO;

namespace VoucherSales_Repositories
{
    public interface IVoucherWalletRepository
    {
        List<VoucherType> GetWalletForUser(int userId);
    }
}

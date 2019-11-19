﻿using System;
using System.Collections.Generic;

namespace Pharus.Domain.PharusProd
{
    public partial class TbDomNavFrequency
    {
        public TbDomNavFrequency()
        {
            TbHistorySubFund = new HashSet<TbHistorySubFund>();
        }

        public int NfId { get; set; }
        public string NfDesc { get; set; }

        public virtual ICollection<TbHistorySubFund> TbHistorySubFund { get; set; }
    }
}

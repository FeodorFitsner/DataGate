﻿using System;
using System.Collections.Generic;

namespace Pharus.Domain.PharusProd
{
    public partial class TbDomCompanyType
    {
        public TbDomCompanyType()
        {
            TbHistoryFund = new HashSet<TbHistoryFund>();
        }

        public int CtId { get; set; }
        public string CtDesc { get; set; }
        public string CtAcronym { get; set; }

        public virtual ICollection<TbHistoryFund> TbHistoryFund { get; set; }
    }
}

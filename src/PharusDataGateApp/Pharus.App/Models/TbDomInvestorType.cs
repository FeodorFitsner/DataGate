﻿using System;
using System.Collections.Generic;

namespace Pharus.App.Models
{
    public partial class TbDomInvestorType
    {
        public TbDomInvestorType()
        {
            TbHistoryShareClass = new HashSet<TbHistoryShareClass>();
        }

        public int ItId { get; set; }
        public string ItDesc { get; set; }

        public virtual ICollection<TbHistoryShareClass> TbHistoryShareClass { get; set; }
    }
}

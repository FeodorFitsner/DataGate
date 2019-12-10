﻿namespace Pharus.Domain.Models.Pharus_vFinale
{
    using System.Collections.Generic;

    public partial class TbDomTypeOfMarket
    {
        public TbDomTypeOfMarket()
        {
            TbHistorySubFund = new HashSet<TbHistorySubFund>();
        }

        public int TomId { get; set; }

        public string TomDesc { get; set; }

        public virtual ICollection<TbHistorySubFund> TbHistorySubFund { get; set; }
    }
}
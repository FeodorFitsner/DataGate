﻿namespace Pharus.Domain.Models.Pharus_vFinale
{
    using System.Collections.Generic;

    public partial class TbDomGlobalExposure
    {
        public TbDomGlobalExposure()
        {
            TbHistorySubFund = new HashSet<TbHistorySubFund>();
        }

        public int GeId { get; set; }

        public string GeDesc { get; set; }

        public virtual ICollection<TbHistorySubFund> TbHistorySubFund { get; set; }
    }
}
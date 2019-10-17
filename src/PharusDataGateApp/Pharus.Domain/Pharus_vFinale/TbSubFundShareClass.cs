﻿using System;
using System.Collections.Generic;

namespace Pharus.Domain.Pharus_vFinale
{
    public partial class TbSubFundShareClass
    {
        public int ScId { get; set; }
        public int? SfId { get; set; }
        public DateTime SfscStartConnection { get; set; }
        public DateTime? SfscEndConnection { get; set; }

        public virtual TbShareClass Sc { get; set; }
        public virtual TbSubFund Sf { get; set; }
    }
}
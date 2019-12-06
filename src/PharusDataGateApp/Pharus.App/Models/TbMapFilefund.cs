﻿using System;
using System.Collections.Generic;

namespace Pharus.App.Models
{
    public partial class TbMapFilefund
    {
        public Guid FileStreamId { get; set; }
        public int FundId { get; set; }
        public DateTime StartConnection { get; set; }
        public DateTime? EndConnection { get; set; }
        public int FiletypeId { get; set; }

        public virtual TbDomFileType Filetype { get; set; }
    }
}

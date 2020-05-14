﻿namespace DataGate.Web.ViewModels.Documents
{
    using System.ComponentModel.DataAnnotations;

    using DataGate.Services.Mapping;
    using DataGate.Web.Dtos.Queries;

    public class DistinctDocViewModel : IMapFrom<DistinctDocDto>
    {
        [Display(Name = "File Description")]
        public string Description { get; set; }

        [Display(Name = "File Name")]
        public string DocumentName { get; set; }
    }
}

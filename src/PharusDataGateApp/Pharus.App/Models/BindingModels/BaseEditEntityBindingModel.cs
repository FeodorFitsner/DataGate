﻿// Abstract model class for bind entity
// for code reuse

// Created: 10/2019
// Author:  Philip Shishov

// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
namespace Pharus.App.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Pharus.App.Models.BindingModels.Contracts;

    public abstract class BaseEditEntityBindingModel /*: IBaseEditEntityBindingModel*/
    {
        [Required(ErrorMessage = "Initial Date cannot be null")]
        [Display(Name = "Initial Date")]
        public DateTime InitialDate { get; set; }

        [RegularExpression(@"^[A-Z0-9_]+$", ErrorMessage = "Not in correct format!")]
        [Display(Name = "CSSF Code")]
        public string CSSFCode { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Not in correct format!")]
        [Display(Name = "Fund Admin Code")]
        [Required]
        public string FACode { get; set; }

        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Not in correct format!")]
        [Display(Name = "Transfer Agent Code")]
        public string TACode { get; set; }

        [RegularExpression(@"^[A-Z0-9_]+$", ErrorMessage = "Not in correct format!")]
        [Display(Name = "LEI Code")]
        public string LEICode { get; set; }

        [Required]
        [Display(Name = "Comment Title")]
        public string CommentTitle { get; set; }

        [Display(Name = "Comment Description")]
        public string CommentArea { get; set; }

        public List<string[]> EntityProperties { get; set; }
    }
}

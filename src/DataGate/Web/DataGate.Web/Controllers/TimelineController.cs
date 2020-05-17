﻿namespace DataGate.Web.Controllers
{
    using System.Collections.Generic;

    using DataGate.Common;
    using DataGate.Services.Data.Timelines.Contracts;
    using DataGate.Web.ViewModels.Timelines;

    using Microsoft.AspNetCore.Mvc;

    public class TimelineController : Controller
    {
        private readonly IFundTimelineService fundService;

        public TimelineController(
                        IFundTimelineService fundService)
        {
            this.fundService = fundService;
        }

        [Route("loadTimelines")]
        public IActionResult GetAllTimelines(int id, string areaName)
        {
            IEnumerable<TimelineViewModel> model = null;

            if (areaName == GlobalConstants.FundAreaName)
            {
                model = this.fundService.GetTimeline<TimelineViewModel>(id);
            }

            return this.PartialView("SpecificEntity/_Timeline", model);
        }
    }
}
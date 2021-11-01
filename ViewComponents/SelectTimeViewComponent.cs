using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.ViewComponents
{
    public class SelectTimeViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<EachTime> eachTimeList)
        {
            return View(eachTimeList);
        }
    }
}

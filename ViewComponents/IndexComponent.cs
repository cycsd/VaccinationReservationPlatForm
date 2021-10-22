using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewComponents
{
    [ViewComponent(Name = "Index")]
    public class IndexComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string controller)
        {
            return View(controller);            

        }
    }
}


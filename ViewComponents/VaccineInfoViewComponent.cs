using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewComponents
{
    [Microsoft.AspNetCore.Mvc.ViewComponent]
    public class VaccineInfoViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            ViewBag.vcInfo = "疫苗資料";
            return View();
        }
    }
}

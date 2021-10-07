using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Controllers.NumberInfo
{
    public class NumberInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

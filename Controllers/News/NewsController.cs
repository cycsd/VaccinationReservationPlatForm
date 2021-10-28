using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Controllers.News
{
    public class NewsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VaccineInfo()
        {
            return View();
        }
    }
}

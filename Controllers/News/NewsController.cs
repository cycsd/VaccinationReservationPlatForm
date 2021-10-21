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
            ViewBag.aa = "News";

            return View();
        }

        public IActionResult News()
        {
            ViewBag.aa = "News";
            return View();
        }
    }
}

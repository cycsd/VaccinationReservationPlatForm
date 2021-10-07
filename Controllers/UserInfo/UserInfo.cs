using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Controllers.UserInfo
{
    public class UserInfo : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

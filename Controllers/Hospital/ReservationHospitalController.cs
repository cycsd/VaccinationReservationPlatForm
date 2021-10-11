using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Controllers.Hospital
{
    public class ReservationHospitalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

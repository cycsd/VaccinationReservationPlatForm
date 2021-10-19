using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly VaccinationBookingSystemContext _context;

        public HomeController(ILogger<HomeController> logger,VaccinationBookingSystemContext vaccinationBookingSystemContext)
        {
            _logger = logger;
            _context = vaccinationBookingSystemContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(CLoginViewModel model)
        {
            Person cust = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Trim().Equals(model.txtPersonIdentityID) && c.PersonHealthId.Trim().Equals(model.txtPersonHealthID));
            if (cust != null && cust.PersonIdentityId.Trim().Equals(model.txtPersonIdentityID) && cust.PersonHealthId.Trim().Equals(model.txtPersonHealthID))
            {
                string json = JsonSerializer.Serialize(cust);
                HttpContext.Session.SetString(CDictionary.SK_LOGIN_CLIENT, json);
                ViewBag.Error = "";
                return RedirectToAction("Index");
            }
            if (cust == null)
            {
                ViewBag.Error = "身分證或健保卡卡號錯誤";
            }
            return View(cust);
        }

        public IActionResult Index()
        {
            var q = _context.People.First();
            return View(q);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

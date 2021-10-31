using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModel;

namespace VaccinationReservationPlatForm.Controllers.Charts
{
    public class ChartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VaccineTrack()
        {
            VaccinationRecordTrackDBmanager dBmanager = new VaccinationRecordTrackDBmanager();

            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                List < VaccinationRecordTrackViewModel> recordNon = new List<VaccinationRecordTrackViewModel>();
                ViewBag.records = recordNon;
                return View();
            }
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            Person userlogin = JsonSerializer.Deserialize<Person>(json);
            List<VaccinationRecordTrackViewModel> records = dBmanager.GetRecord(userlogin.PersonId);
            ViewBag.records = records;
            return View();
            
        }
    }
}

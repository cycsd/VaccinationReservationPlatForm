using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult VaccineTrack(int personID)
        {
            VaccinationRecordTrackDBmanager dBmanager = new VaccinationRecordTrackDBmanager();
            List<VaccinationRecordTrackViewModel> records = dBmanager.GetRecord(personID);
            ViewBag.records = records;
            return View();
        }
    }
}

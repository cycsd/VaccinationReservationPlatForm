using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using static VaccinationReservationPlatForm.Models.NewsService;
using static VaccinationReservationPlatForm.ViewModel.NewsViewModel;

namespace VaccinationReservationPlatForm.ViewComponents 
{
    [Microsoft.AspNetCore.Mvc.ViewComponent]
    public class NewsViewComponent: Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke(string url)
        {
            var svc = new NewsService();
            var data = svc.Crawl(url);
            ViewBag.title = data;
            ViewBag.url = data;
            return View();
        }
    }
}

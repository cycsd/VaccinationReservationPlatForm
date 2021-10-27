using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.ViewComponents
{
    [ViewComponent(Name = "Index")]
    public class IndexComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string controller)
        {
            //if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            //{
            //    TempData["Error"] = "請先登入！";
                
            //}
            //string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            //Person userlogin = JsonSerializer.Deserialize<Person>(json);

            ViewBag.abc = "123";
            return View(controller);            

        }
    }
}


using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class LogicModel
    {
        public void CheckLogin()
        {
            //if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            //{
            //    TempData["Error"] = "尚未登入，請先登入。";
            //}
        }
                    

}
}

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
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                ViewBag.vad = "false";
            }
            else 
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
                Person user = JsonSerializer.Deserialize<Person>(json);
                Person userlogin = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Trim().Equals(user.PersonIdentityId));
                if (userlogin.PersonBirthday != null)
                {
                    string Bir = userlogin.PersonBirthday.ToString();
                    DateTime x = DateTime.Parse(Bir);
                    string Birthday = x.ToString("yyyy/MM/dd");
                    ViewBag.Birthdayreal = Birthday;
                    ViewBag.Birthday = null;
                }
                else {
                    ViewBag.Birthday = userlogin.PersonBirthday;
                    ViewBag.Birthdayreal = "";
                }
                if (TempData["login"] != null)
                {
                    string login = TempData["login"].ToString();
                    ViewBag.login = login;
                }
                else 
                {
                    ViewBag.errormsg = "";
                }
                string? Name = userlogin.PersonName;
                string? Address = userlogin.PersonAdress;
                string? Phone = userlogin.PersonCellphoneNumber;
                string? Email = userlogin.PersonMail;
                string? Sex = userlogin.PersonSex;

                ViewBag.Name = Name;
                ViewBag.Address = Address;
                ViewBag.Phone = Phone;
                ViewBag.Email = Email;
                ViewBag.Sex = Sex;





                ViewBag.vad = "true";
            }

            
            return View(controller);            

        }
    }
}


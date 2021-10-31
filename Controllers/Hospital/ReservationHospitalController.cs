using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers.Hospital
{
    public class ReservationHospitalController : Controller
    {
        private string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            return sb.ToString();
        }
        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }



        public ActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = RandomCode(5);
            TempData["code"] = code;
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(20, 8));
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Index(CHospitalUser model)
        {
            string code = Request.Form["code"].ToString();
            if (code == TempData["code"].ToString())
            {
                HospitalUser hospital = (new VaccinationBookingSystemContext()).HospitalUsers.FirstOrDefault(
                    h => h.HospitalUserName.Trim().Equals(model.hospitalUserName) && h.HospitalUserPassword.Trim().Equals(model.hospitalUserPassword));
                if (hospital != null && hospital.HospitalUserName.Trim().Equals(model.hospitalUserName) && hospital.HospitalUserPassword.Trim().Equals(model.hospitalUserPassword))
                {
                    string json = JsonSerializer.Serialize(hospital);
                    HttpContext.Session.SetString(CDictionary.SK_LOGIN_HOSPITAL, json);
                    ViewBag.Error = "";
                    return RedirectToAction("Index", "Home");
                }
                if (hospital == null)
                {
                    ViewBag.Error = "帳號或密碼有誤";
                }
                return View(hospital);
            }
            ViewBag.Error = "圖形驗證碼有誤！";
            return View();
        }
    }
}

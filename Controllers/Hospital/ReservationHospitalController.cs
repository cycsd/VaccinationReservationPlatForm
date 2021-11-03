using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers.Hospital
{
    public class ReservationHospitalController : Controller
    {

        private readonly VaccinationBookingSystemContext context;
        public ReservationHospitalController(VaccinationBookingSystemContext context)
        {
            this.context = context;
        }

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


        public IActionResult HospitalWorkTime()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_HOSPITAL))
            {
                return RedirectToAction("Index", "ReservationHospital");
            }
            return View();
        }

        public IActionResult SaveHospitalWorkTime(List<WorkDay> workDays)
        {
            //Get hospitalId
            HospitalUser hospitalAccountToGetHospital = JsonSerializer.Deserialize<HospitalUser>(HttpContext.Session.GetString(CDictionary.SK_LOGIN_HOSPITAL));

            int? hospitalId = context.UserForHospitals.FirstOrDefault(s => s.HospitalUserId == hospitalAccountToGetHospital.HospitalUserId).HospitalId;

            //取得醫院對應日期的ID，如果裡面沒資料則新增
            var weekdays = context.HospitalBusinessDays.Where(s => s.HospitalId == hospitalId);
            if (weekdays.Count() == 0)
            {
                foreach (var workDay in workDays)
                {
                    HospitalBusinessDay hospitalBusinessDay = new HospitalBusinessDay();
                    hospitalBusinessDay.HospitalId = hospitalId;
                    hospitalBusinessDay.Hbdweekday = workDay.dayOfWeek;
                    context.HospitalBusinessDays.Add(hospitalBusinessDay);
                }
                context.SaveChanges();
                weekdays = context.HospitalBusinessDays.Where(s => s.HospitalId == hospitalId);
            }
            //End 取得醫院對應日期

            var allHospitalBusinessHourParts = context.HospitalBusinessHours;

            //取得醫院對應全部日期的時段，如果沒有則新增
            var hospitalBusinessHourParts = weekdays.Join(allHospitalBusinessHourParts, o => o.HospitalBusinessDayId, i => i.HospitalBusinessDayId, (o, i) => i);

            if (hospitalBusinessHourParts.Count() == 0)
            {
                foreach (var item in weekdays)
                {
                    var workDay = workDays.FirstOrDefault(workDay => workDay.dayOfWeek == item.Hbdweekday);
                    foreach (var workTime in workDay.workTimes)
                    {
                        HospitalBusinessHour hospitalBusinessHour = new HospitalBusinessHour()
                        {
                            HospitalBusinessDayId = item.HospitalBusinessDayId,
                            HbhstartTime = workTime.startTime,
                            HbhendTime = workTime.endTime,
                            Hbhmark = workTime.workMark,
                        };
                        context.HospitalBusinessHours.Add(hospitalBusinessHour);
                    }
                }
                context.SaveChanges();
                hospitalBusinessHourParts = weekdays.Join(allHospitalBusinessHourParts, o => o.HospitalBusinessDayId, i => i.HospitalBusinessDayId, (o, i) => i);
            }
            //End 取得醫院對應全部日期的時段

            //先將營業時段全部設為off，避免之後時段調整有些關掉了，卻忘記將資料庫內的時段關掉
            //foreach (var item in hospitalBusinessHourParts)
            //{
            //    item.Hbhmark = 0;
            //}
            ////End 將營業時段設為off
            //context.SaveChanges();

            //將前端發過來的workDays資料儲存至Context裡
            //foreach (var workday in workDays)
            //{
            //    int dayOfWeekInt = workday.dayOfWeek;
            //    //int dayOfWeekInt = workDays[1].dayOfWeek;

            //    int hospitalBusinessDayId = weekdays.FirstOrDefault(s => s.Hbdweekday == dayOfWeekInt).HospitalBusinessDayId;

            //    var workTimeInContext = hospitalBusinessHourParts.Where(s => s.HospitalBusinessDayId == hospitalBusinessDayId);
            //    var workdayOrder = workday.workTimes.OrderBy(s => s.startTime).ToList();
            //    //var workdayOrder = workDays[1].workTimes.OrderBy(s => s.startTime).ToList();

            //    //for (int i = 0; i < workdayOrder.Count(); i++)
            //    //{
            //    //    workTimeInContext[i].HbhstartTime = workdayOrder[i].startTime;
            //    //    workTimeInContext[i].HbhendTime = workdayOrder[i].endTime;
            //    //    workTimeInContext[i].Hbhmark = workdayOrder[i].workMark;
            //    //}
            //    int i = 0;
            //    foreach (var item in workTimeInContext)
            //    {
            //        item.HbhstartTime = workdayOrder[i].startTime;
            //        item.HbhendTime = workdayOrder[i].endTime;
            //        item.Hbhmark = workdayOrder[i].workMark;
            //        i++;
            //    }
            //}
            var hopitalhourparts = context.HospitalBusinessHours.ToList();
            var groups = hopitalhourparts.GroupBy(s => (int)s.HospitalBusinessDayId);
            //var groups = from s in hospitalBusinessHourParts
            //             group s by s.HospitalBusinessDayId;

            foreach (var group in groups)
            {
                var q = workDays.Join(weekdays, o => o.dayOfWeek, i => i.Hbdweekday, (o, i) => new { i.HospitalBusinessDayId, workday = o });
                if (q.FirstOrDefault(q => q.HospitalBusinessDayId == group.Key) == null)
                {
                    continue;
                }
                var workTimes = q.FirstOrDefault(q => q.HospitalBusinessDayId == group.Key).workday.workTimes;

                int i = 0;
                foreach (var item in group)
                {
                    try
                    {
                        item.HbhstartTime = workTimes[i].startTime;
                        item.HbhendTime = workTimes[i].endTime;
                        item.Hbhmark = workTimes[i].workMark;
                        i++;
                        context.SaveChanges();

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
            }
            //End 將前端發過來的workDays資料儲存至Context裡

            return Ok(workDays);
        }


    }
}

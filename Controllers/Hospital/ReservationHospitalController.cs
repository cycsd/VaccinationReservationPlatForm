using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HospitalWorkTime()
        {
            return View();
        }

        public IActionResult SaveHospitalWorkTime(List<WorkDay> workDays)
        {
            //todo get hospitalId
            int hospitalId = 1;

            //取得醫院對應日期的ID，如果裡面沒資料則新增
            var weekdays = context.HospitalBusinessDays.Where(s => s.HospitalId == hospitalId);
            if (weekdays == null)
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

            if (hospitalBusinessHourParts == null)
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
            foreach (var item in hospitalBusinessHourParts)
            {
                item.Hbhmark = 0;
            }
            //End 將營業時段設為off

            foreach (var item in workDays)
            {
                int dayOfWeek = item.dayOfWeek;

            }




            return Ok(workDays);
        }
    }
}

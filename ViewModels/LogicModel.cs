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

    public class WorkDay
    {
        public int dayOfWeek { get; set; }

        public List<WorkTime> workTimes { get; set; }
    }

    public class WorkTime
    {
        public int workMark { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
    }

    public class EachTime
    {
        public string 日期 { get; set; }

        public string 時段 { get; set; }

        public string 預約狀態 = "可預約";

        public DateTime date { get; set; }
        public TimeSpan startTime;
        public TimeSpan endTime;

        public EachTime(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            日期 = date.GetDateTimeFormats('M')[0].ToString();
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
            this.GetTimeSpanToString();
        }
        public void GetTimeSpanToString()
        {
            this.時段 = this.startTime.ToString(@"hh\:mm") + "~" + this.endTime.ToString(@"hh\:mm");
        }

    }

    public class BookingInfo
    {
        public int PersonId { get; set; }
        public int hospital { get; set; }
        public int vaccine { get; set; }
        public DateTime date { get; set; }

        public TimeSpan timeStart { get; set; }

        public DateTime clickMoment { get; set; }

    }
}

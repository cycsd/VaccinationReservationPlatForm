using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers.Reservation
{
    public class ReservationController : Controller
    {
        private readonly VaccinationBookingSystemContext context;
        private readonly IWebHostEnvironment environment;
        //private Person person;
        private IQueryable<int?> vaccinationWanted;
        public ReservationController(VaccinationBookingSystemContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            //確認是否有登入
            if (!CheckLogin())
            {
                return RedirectToAction("Login", "UserInfo");
            }

            Person personToGetID = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT));
            Person person = context.People.FirstOrDefault(s => s.PersonId == personToGetID.PersonId);

            //todo: 邏輯驗證，這個人是否可以預約疫苗，職業、年紀、是否已經預約過、是否有疫苗可預約...

            return View(person);
        }



        //[HttpPost]
        public IActionResult GetVaccineWanted(int userid)
        {

            this.vaccinationWanted = context.VaccinationWanteds.Where(person => person.PersonId == userid).Select(person => person.VaccineId);
            var vaccines = vaccinationWanted.Join(context.Vaccines, o => o.Value, i => i.VaccineId, (o, i) =>
            new
            {
                i.VaccineId,
                i.VaccineName
            }
            );
            //todo: 個人疫苗選擇資訊，如果這個人沒有選擇疫苗怎麼處理?
            //todo: 第二次預約需要判別前一次是打甚麼疫苗
            //if (vaccines.Count() == 0)
            //{
            //    TempData["Error"] = "尚未登記過疫苗。";
            //    //return StatusCode(StatusCodes.Status404NotFound,"message");
            //    return NotFound();
            //}

            return Ok(vaccines);
        }

        public IActionResult GetHospitalInfo(string county, string town, int zipcode, int vaccineID)
        {

            // todo:判斷使用者選擇的疫苗醫院是否還有庫存
            int vaccineBook = context.VaccinationBookings.Where(person => person.VaccineId == vaccineID).Count();
            var hospitalhaveStock = context.VaccineStocks.
                Where(hospital => hospital.VaccineId == vaccineID && (hospital.VaccineStockUnit - hospital.VaccineStockConsumption - vaccineBook) > 0);

            var hospitalcontext = context.Hospitals;
            var hospital = hospitalcontext.Join(hospitalhaveStock, i => i.HospitalId, o => o.HospitalId, (i, o) => i);
            var hospialSelect = hospital.Where(delegate (VaccinationReservationPlatForm.Models.Hospital s)
            {
                if (s.HospitalAdress == null)
                {
                    return false;
                }
                else
                {
                    int contType;
                    int hospitalType;
                    if (Int32.TryParse(s.HospitalContType, out contType) && Int32.TryParse(s.HospitalType, out hospitalType))
                    {
                        return s.CountyPostalCode == zipcode && contType <= 4 && (hospitalType <= 3 || hospitalType == 8) && s.HospitalCategory == "A";
                    }
                    return false;
                }
            });

            return Ok(hospialSelect);

        }
        public IActionResult TimeSelect(int hospitalId)
        {
            hospitalId = 5690; //測試用需註解掉
            List<EachTime> eachTimeList = new List<EachTime>();
            int bookingTimeSpan = 7;
            int bookingStartOffsetDays = 2;

            //取得醫院營業日id
            var businessDayTimeTable = context.HospitalBusinessDays.Where(s => s.HospitalId == hospitalId)
                .Join(context.HospitalBusinessHours, o => o.HospitalBusinessDayId, i => i.HospitalBusinessDayId, (o, i) => new
                {
                    o.Hbdweekday,
                    i.HbhstartTime,
                    i.HbhendTime,
                    i.Hbhmark
                }).ToArray().GroupBy(s => s.Hbdweekday);

            for (int i = 0; i < bookingTimeSpan; i++)           //取得可預約的日期
            {
                int offsetDay = i + bookingStartOffsetDays;
                DateTime bookingday = DateTime.Now.AddDays(offsetDay);
                int weekday = (int)bookingday.DayOfWeek;

                //try     ///可以思考一下如果日期為非營業時段該如何處理，.First會取不到值
                //{
                var q = businessDayTimeTable.First(g => g.Key == weekday).OrderBy(s => s.HbhstartTime);  //取得一個符合日期weekday的群組

                foreach (var item in q)
                {
                    if (item.Hbhmark == 1) //有營業才取值
                    {
                        TimeSpan startTime = (TimeSpan)item.HbhstartTime;
                        TimeSpan endTime = (TimeSpan)item.HbhendTime;
                        eachTimeList.AddRange(this.AddEachTimeToList(bookingday.Date, startTime, endTime));
                    }

                }
                //}
                //catch (Exception)
                //{

                //}

            }
            //todo 判斷當前預約數是否超出醫院庫存，需知道是哪個疫苗
            //int? hospitalStock = context.VaccineStocks.FirstOrDefault(s => s.HospitalId == hospitalId).VaccineStockUnit;
            //hospitalStock = hospitalStock == null ? 0 : hospitalStock;

            //判斷當前預約人數是否已超出時段人數限制
            int? hospitalCapacity = context.Hospitals.FirstOrDefault(s => s.HospitalId == hospitalId).HospitalCapacity;
            hospitalCapacity = hospitalCapacity == null ? 0 : hospitalCapacity;

            var eachTimeListGroup = eachTimeList.GroupBy(s => s.date);
            foreach (var item in eachTimeListGroup)
            {
                DateTime date = item.Key;
                var bookingPeopleInDay = context.VaccinationBookings.Where(s => s.VbbookingDate == date).ToArray();
                foreach (var eachtime in item)
                {
                    var startTime = eachtime.startTime;
                    int eachTimeBookingPeropleCount = bookingPeopleInDay.Where(s => s.VbbookingTime == startTime).Count();
                    if (eachTimeBookingPeropleCount >= hospitalCapacity)
                    {
                        eachtime.預約狀態 = "額滿";
                    }
                }
            }

            return ViewComponent("SelectTime", new { eachTimeList = eachTimeList });
        }

        [HttpGet]
        public IActionResult UserTimeSelect()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.Key_BookingTime_Select))
            {
                return NotFound();
            }
            string stringJson = HttpContext.Session.GetString(CDictionary.Key_BookingTime_Select);


            return Ok(stringJson);
        }

        [HttpPost]
        public IActionResult UserTimeSelect(DateTime date,TimeSpan startTime,string timePart)
        {
            object ob = new
            {
                date = date,
                startTime = startTime,
                timePart = timePart,
            };
            string strignTimeJson = JsonSerializer.Serialize(ob);
            HttpContext.Session.SetString(CDictionary.Key_BookingTime_Select, strignTimeJson);

            return Ok(HttpContext.Session.GetString(CDictionary.Key_BookingTime_Select));
        }

        public IActionResult SaveBookingInfo(BookingInfo bookingInfo)
        {
            int hospitalId = bookingInfo.hospital;
            int vaccineId = bookingInfo.vaccine;
            TimeSpan startTime = bookingInfo.timeStart;

            //判斷醫院疫苗是否還有庫存(取決於現有庫存跟現階段預約人數)
            int? vaccineStockInHospital = context.VaccineStocks.FirstOrDefault(s => s.HospitalId == hospitalId && s.VaccineId == vaccineId).VaccineStockUnit;
            vaccineStockInHospital = vaccineStockInHospital == null ? 0 : vaccineStockInHospital;

            //todo 是否需驗證此人是否有登記過，感覺應該再進到預約畫面前就該判斷，不該在此步驟判斷

            var peopleInBooking = context.VaccinationBookings.Where(s => s.HospitalId == hospitalId && s.VaccineId == vaccineId).ToArray();
            int peopleInBookingAmount = peopleInBooking.Count();

            var hospital = context.Hospitals.FirstOrDefault(s => s.HospitalId == hospitalId);
            int? capacity = hospital.HospitalCapacity;
            capacity = capacity == null ? 0 : capacity;
            //計算目前各時段預約人數得到號碼牌
            int peopleInBookingCount = peopleInBooking.Where(s => s.VbbookingTime == startTime).Count();
            int booknumber = peopleInBookingCount + 1;
            //判斷預約時段人數是否大於量能及人數是否大於庫存
            if (peopleInBookingAmount >= vaccineStockInHospital && peopleInBookingCount >= capacity)
            {
                TempData["BookFailMessage"] = "已額滿，請選擇其他醫院或時段";
                return RedirectToAction("Index");
            }

            Person personToGetID = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT));
            VaccinationBooking vaccinationBookingUnit = new VaccinationBooking()
            {
                PersonId = personToGetID.PersonId,
                HospitalId = hospitalId,
                VaccineId = vaccineId,
                VbbookingDate = bookingInfo.date,
                VbbookingTime = startTime,
                VbclickMoment = DateTime.Now,
                Vbnumber = booknumber,

            };
            //context.VaccinationBookings.Add(vaccinationBookingUnit);
            //context.SaveChanges();

            //填寫預約資訊
            var person = context.People.FirstOrDefault(s => s.PersonId == personToGetID.PersonId);
            bookingInfo.clickMoment = DateTime.Now;
            bookingInfo.PersonIdentityId = personToGetID.PersonIdentityId;
            bookingInfo.PersonName = person.PersonName;
            bookingInfo.PhoneNumber = person.PersonCellphoneNumber;
            bookingInfo.MailAddress = person.PersonMail;

            bookingInfo.hospitalName = hospital.HospitalName;
            bookingInfo.hospitalAddress = hospital.HospitalAdress;

            var VaccineInfo = context.Vaccines.FirstOrDefault(s=>s.VaccineId == bookingInfo.vaccine);
            bookingInfo.vaccineName = VaccineInfo.VaccineName;

            return View(bookingInfo);
        }
        public IActionResult SendMail(string toMailAddress,string printContent)
        {
            string username = "";
            string password = "";
            NetworkCredential credential = new NetworkCredential(username, password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(toMailAddress));
            mailMessage.From = new MailAddress(username, username);
            mailMessage.Subject = "預約接種結果";
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(printContent, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            smtpClient.Credentials = credential;
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                Console.WriteLine("寄信失敗");
            }
          
            return Ok();
        }

        public IActionResult HospitalList()
        {
            var hospital = context.Hospitals;
            return View(hospital);
        }

        public IActionResult AddZipCodeToHospital()
        {
            //string path = Url.Content("wwwroot/XML/taiwanCountyToTown.xml");

            //依賴注入IWebHostEnviroment取得網址絕對位置
            string path = environment.WebRootPath + "/XML/taiwanCountyToTown.xml";

            //載入XML文件
            XElement root = XElement.Load(path);

            //取得county節點(物件)，並將名稱及其內容area取出
            var countiesZip = root.Elements("county").Select(s => new
            {
                area = s.Elements("area"),
                countyname = (string)s.Attribute("name")
            });

            Dictionary<string, int> zipCountyTown = new Dictionary<string, int>();
            foreach (var item in countiesZip)
            {
                string countyName = item.countyname;
                foreach (var area in item.area)
                {
                    string areaName = (string)area;
                    int zip = (int)area.Attribute("zip");
                    string countyTown = countyName + areaName;
                    zipCountyTown.Add(countyTown, zip);
                }

            }

            //將郵遞區號給各hospital
            DbSet<VaccinationReservationPlatForm.Models.Hospital> hospitals = context.Hospitals;

            foreach (var hospital in hospitals)
            {
                if (hospital.HospitalAdress != null)
                {
                    foreach (var zipTown in zipCountyTown)
                    {
                        string keyReplaceTai = zipTown.Key.Replace("臺", "台");
                        if (hospital.HospitalAdress.Contains(zipTown.Key) | hospital.HospitalAdress.Contains(keyReplaceTai))
                        {
                            hospital.CountyPostalCode = zipTown.Value;
                            break;
                        }
                    }

                }
            }

            context.SaveChanges();
            return Ok(zipCountyTown);
        }

        //新增疫苗庫存
        public IActionResult AssignVaccineToHospital()
        {
            //取得現有疫苗
            var vaccines = context.Vaccines.Select(vaccine => vaccine.VaccineId).ToList();
            var hospitals = context.Hospitals;

            //讓各醫院有現有疫苗的庫存
            foreach (var hospital in hospitals)
            {
                int contType;
                int hospitalType;
                if (Int32.TryParse(hospital.HospitalContType, out contType) && Int32.TryParse(hospital.HospitalType, out hospitalType))
                {
                    if (contType <= 4 && (hospitalType <= 3 || hospitalType == 8) && hospital.HospitalCategory == "A")
                    {
                        if (!hospital.HospitalName.Contains("眼") && !hospital.HospitalName.Contains("皮膚"))
                        {

                            foreach (var vaccine in vaccines)
                            {
                                Random random = new Random(Guid.NewGuid().GetHashCode());
                                int vaccinequanty = random.Next(0, 5) * 50;

                                VaccineStock vaccineStock = new VaccineStock()
                                {
                                    HospitalId = hospital.HospitalId,
                                    VaccineId = vaccine,
                                    VaccineStockUnit = vaccinequanty,
                                    VaccineStockConsumption = 0,
                                };
                                context.VaccineStocks.Add(vaccineStock);

                            }
                        }
                    }

                }

            }
            context.SaveChanges();
            return Ok("sucess");
        }


        public bool CheckLogin()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                return false;
            }
            return true;
        }

        public List<EachTime> AddEachTimeToList(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            List<EachTime> eachTimeList = new List<EachTime>();
            double totalHour = (endTime - startTime).TotalHours;
            for (int i = 0; i < totalHour; i++)
            {
                //TimeSpan oneHour = new TimeSpan(1, 0, 0);
                TimeSpan stepStartTime = startTime + new TimeSpan(i, 0, 0);
                TimeSpan stepEndTime = startTime + new TimeSpan(i + 1, 0, 0);

                if (stepEndTime > endTime)
                {
                    TimeSpan residueTimeSpan = endTime - stepStartTime;
                    double residueMinutes = residueTimeSpan.TotalMinutes;
                    if (residueMinutes >= 45)
                    {
                        EachTime eachTime = new EachTime(date, stepStartTime, stepEndTime);
                        eachTimeList.Add(eachTime);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            return eachTimeList;
                        }
                        else
                        {
                            EachTime lastTime = eachTimeList.Last();
                            lastTime.endTime = lastTime.endTime + residueTimeSpan;
                            lastTime.GetTimeSpanToString();
                        }
                    }
                }
                else
                {
                    EachTime eachTime = new EachTime(date, stepStartTime, stepEndTime);
                    eachTimeList.Add(eachTime);
                }
            }
            return eachTimeList;
        }

    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.Controllers.Reservation
{
    public class ReservationController : Controller
    {
        private readonly VaccinationBookingSystemContext context;
        private readonly IWebHostEnvironment environment;
        private Person person;
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

            this.person = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT));

            //todo: 邏輯驗證，這個人是否可以預約疫苗，職業、年紀、是否已經預約過、是否有疫苗可預約...

            return View(this.person);
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

            //讓各醫院有限有疫苗的庫存
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

    }
}

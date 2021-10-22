using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VaccinationReservationPlatForm.Models;

namespace VaccinationReservationPlatForm.Controllers.Reservation
{
    public class ReservationController : Controller
    {
        private readonly VaccinationBookingSystemContext context;
        private readonly IWebHostEnvironment environment;
        public ReservationController(VaccinationBookingSystemContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            // todo:將個人資訊傳到表單
            return View();
        }

        public IActionResult GetHospitalInfo(string county,string town,int zipcode,string vaccine)
        {
            
            // todo:判斷使用者選擇的疫苗醫院是否還有庫存
            
            var hospital = context.Hospitals;
            var hospialSelect = hospital.Where(delegate (VaccinationReservationPlatForm.Models.Hospital s)
            {
                if (s.HospitalAdress == null)
                {
                    return false;
                }
                else
                {
                    return s.HospitalAdress.Contains("中正區");
                }
            });

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
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers.UserInfo
{
    public class UserInfo : Controller
    {
        protected string QnA(string words)
        {
            string Ans = "";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (words == null)
            {
                Ans = "請輸入訊息";
                return Ans;
            }
            var ret = CallQna(words);
            //ret[0].translations[0].text
            if (ret.answers[0].answer == null)
            {
                Ans = "error";
                return Ans;
            }
            Ans = ret.answers[0].answer.ToString();

            return Ans;
            
            //Console.Write($"結果: {ret.answers[0].answer}");
        }

        static dynamic CallQna(string msg)
        {
            HttpClient client = new HttpClient();
            string endpoint = "https://vaccinationqnamaker.azurewebsites.net/qnamaker/knowledgebases/0d22a768-9839-4f40-a22b-d78b4c7a2691/generateAnswer";

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Authorization", "EndpointKey d4663526-2df8-45d8-860b-3fa00d05567d");

            var JsonString = "{\"question\":\"" + msg + "\"}";
            var content =
               new StringContent(JsonString, System.Text.Encoding.UTF8, "application/json");
            //呼叫
            var response = client.PostAsync(endpoint, content).Result;
            //取得回傳
            var JSON = response.Content.ReadAsStringAsync().Result;
            //轉型
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(JSON);
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

        public IActionResult Login()
        {
            if (TempData["Error"] != null)
            {
                string error = TempData["Error"].ToString();
                ViewBag.Error = error;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(CLoginViewModel model)
        {
            string code = Request.Form["code"].ToString();
            if (code == TempData["code"].ToString())
            {
                Person cust = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Trim().Equals(model.txtPersonIdentityID) && c.PersonHealthId.Trim().Equals(model.txtPersonHealthID));
                if (cust != null && cust.PersonIdentityId.Trim().Equals(model.txtPersonIdentityID) && cust.PersonHealthId.Trim().Equals(model.txtPersonHealthID))
                {
                    string json = JsonSerializer.Serialize(cust);
                    HttpContext.Session.SetString(CDictionary.SK_LOGIN_CLIENT, json);
                    ViewBag.Error = "";
                    return RedirectToAction("Index","Home");
                }
                if (cust == null)
                {
                    ViewBag.Error = "身分證或健保卡卡號有誤";
                }
                return View(cust);
            }
            ViewBag.Error = "圖形驗證碼有誤！";
            return View();
        }

        public IActionResult WantedVaccine()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                TempData["Error"] = "請先登入！";
                return RedirectToAction("Login");
            }
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            Person userlogin = JsonSerializer.Deserialize<Person>(json);
            Person user = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Trim().Equals(userlogin.PersonIdentityId) && c.PersonHealthId.Trim().Equals(userlogin.PersonHealthId));
            ViewData["user"] = user;
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();
            IEnumerable<int?> wanted = from v in db.VaccinationWanteds
                               where v.PersonId == userlogin.PersonId
                               select v.VaccineId;
            if (wanted.ToList().FirstOrDefault() != null)
            {
                string Number = "";
                List<int?> VaccineInt = new List<int?>(wanted);
                foreach (var item in VaccineInt)
                {
                    Number += item.ToString();
                }
                ViewBag.Number = Number;
            }
            return View(user);
        }
        
        [HttpPost]
        public IActionResult WantedVaccine(Person model)
        {
            string CountyText = Request.Form["County"].ToString().Trim();
            string RoadText = Request.Form["Road"].ToString().Trim();
            string CountyTownText = Request.Form["CountyTownText"].ToString().Trim();
            string Address = Path.Combine(CountyText, CountyTownText, RoadText);
            string AddressName = Address.Replace("\\","");
            string CountyTown = Request.Form["CountyTown"].ToString().Trim();
            int CountyTown2 = Int32.Parse(CountyTown);
            string Vaccine = Request.Form["Vaccine"].ToString();
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();
            Person user = db.People.FirstOrDefault(p => p.PersonIdentityId.Trim().Equals(model.PersonIdentityId));
            IEnumerable<int?> wanted = from v in db.VaccinationWanteds
                                       where v.PersonId == user.PersonId
                                       select v.VaccineId;
            if (user != null )
            {
                user.PersonIdentityId = model.PersonIdentityId.Trim();
                user.PersonName = model.PersonName.Trim();
                user.PersonCellphoneNumber = model.PersonCellphoneNumber.Trim();
                user.CountyPostalCode = CountyTown2;
                user.PersonAdress = AddressName.Trim();
                db.SaveChanges();
            }
            if (user != null )
            {
                string Number ="";
                List<int?> VaccineInt = new List<int?>(wanted);
                foreach (var item in VaccineInt)
                {
                    Number += item;
                }
                if (Vaccine.Contains("1"))
                {
                    if (!Number.Contains("1"))
                    {
                        VaccinationWanted x = new VaccinationWanted()
                        {
                            PersonId = user.PersonId,
                            VaccineId = 1,
                        };
                        db.VaccinationWanteds.Add(x);
                    }
                }
                else
                {
                    if (Number.Contains("1"))
                    {
                        VaccinationWanted delete = db.VaccinationWanteds.FirstOrDefault(w => w.PersonId.Equals(user.PersonId) && w.VaccineId.Equals(1));
                        if (delete != null)
                        {
                            db.VaccinationWanteds.Remove(delete);
                        }
                    }
                }
                if (Vaccine.Contains("2"))
                {
                    if (!Number.Contains("2"))
                    {
                        VaccinationWanted x = new VaccinationWanted()
                        {
                            PersonId = user.PersonId,
                            VaccineId = 2,
                        };
                        db.VaccinationWanteds.Add(x);
                    }
                }
                else
                {
                    if (Number.Contains("2"))
                    {
                        VaccinationWanted delete = db.VaccinationWanteds.FirstOrDefault(w => w.PersonId.Equals(user.PersonId) && w.VaccineId.Equals(2));
                        if (delete != null)
                        {
                            db.VaccinationWanteds.Remove(delete);
                        }
                    }
                }
                if (Vaccine.Contains("3"))
                {
                    if (!Number.Contains("3"))
                    {
                        VaccinationWanted x = new VaccinationWanted()
                        {
                            PersonId = user.PersonId,
                            VaccineId = 3,
                        };
                        db.VaccinationWanteds.Add(x);
                    }
                }
                else
                {
                    if (Number.Contains("3"))
                    {
                        VaccinationWanted delete = db.VaccinationWanteds.FirstOrDefault(w => w.PersonId.Equals(user.PersonId) && w.VaccineId.Equals(3));
                        if (delete != null)
                        {
                            db.VaccinationWanteds.Remove(delete);
                        }
                    }
                }
                if (Vaccine.Contains("4"))
                {
                    if (!Number.Contains("4"))
                    {
                        VaccinationWanted x = new VaccinationWanted()
                        {
                            PersonId = user.PersonId,
                            VaccineId = 4,
                        };
                        db.VaccinationWanteds.Add(x);
                    }
                }
                else
                {
                    if (Number.Contains("4"))
                    {
                        VaccinationWanted delete = db.VaccinationWanteds.FirstOrDefault(w => w.PersonId.Equals(user.PersonId) && w.VaccineId.Equals(4));
                        if (delete != null)
                        {
                            db.VaccinationWanteds.Remove(delete);
                        }
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
        public IActionResult List(CUserInfoModel model)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                return RedirectToAction("Login");
            }
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            Person userlogin = JsonSerializer.Deserialize<Person>(json);
            Person user = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Trim().Equals(userlogin.PersonIdentityId));
            return View(user);
        }

        public IActionResult Edit()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                return RedirectToAction("Login");
            }
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            Person userlogin = JsonSerializer.Deserialize<Person>(json);
            Person user = (new VaccinationBookingSystemContext()).People.FirstOrDefault(
                c => c.PersonIdentityId.Equals(userlogin.PersonIdentityId));
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(CPerson x)
        {
            string CountyText = Request.Form["County"].ToString().Trim();
            string RoadText = Request.Form["Road"].ToString().Trim();
            string CountyTownText = Request.Form["CountyTownText"].ToString().Trim();
            string Address = Path.Combine(CountyText, CountyTownText, RoadText);
            string AddressName = Address.Replace("\\", "");
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();
            Person user = db.People.FirstOrDefault(p => p.PersonIdentityId == x.PersonIdentityId);
            if (user != null)
            {
                user.PersonCellphoneNumber = x.PersonCellphoneNumber.Trim();
                user.PersonMail = x.PersonMail.Trim();
                user.PersonAdress = AddressName.Trim();
                user.PersonJob = x.PersonJob.Trim();
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home","contact");
        }

        public IActionResult Query()
        {
            if (TempData["Error"] != null)
            {
                string error = TempData["Error"].ToString();
                ViewBag.Result = error;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Query(string UserIdentityId, string PhoneNumber)
        {

            string code = Request.Form["code"].ToString();
            if (code == TempData["code"].ToString())
            {
                Person cust = (new VaccinationBookingSystemContext()).People.FirstOrDefault(c => c.PersonIdentityId.Trim().Equals(UserIdentityId));
                if (cust != null && cust.PersonIdentityId.Trim() == UserIdentityId && cust.PersonCellphoneNumber.Trim() == PhoneNumber)
                {
                    TempData["Identity"] = cust.PersonIdentityId.Trim().ToString();
                    return RedirectToAction("QueryResult");
                }
                else
                {
                    ViewBag.Result = "身分證號或手機號碼錯誤！請重新輸入。";
                    return View();
                }
            }
            ViewBag.Result = "驗證錯誤";
            return View();

        }

        public IActionResult QueryResult()
        {
            string customerId = TempData["Identity"].ToString();
            //SQL
            VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();
            IEnumerable<CQueryResult> cust = from p in db.People
                                             join c in db.VaccinationWanteds on p.PersonId equals c.PersonId
                                             join n in db.Counties on p.CountyPostalCode equals n.CountyPostalCode
                                             join v in db.Vaccines on c.VaccineId equals v.VaccineId
                                             where p.PersonIdentityId.Trim().Equals(customerId)
                                             select new CQueryResult
                                             {
                                                 PersonIdentityId = p.PersonIdentityId,
                                                 PersonName = p.PersonName,
                                                 PersonCellphoneNumber = p.PersonCellphoneNumber,
                                                 CountyTownName = n.CountyTownName,
                                                 CountyName = n.CountyName,
                                                 VaccineName = v.VaccineName,
                                             };
            if (cust.ToList().FirstOrDefault() != null)
            {
                return View(cust);
            }
            else
            {
                TempData["Error"] = "身分證號或手機號碼錯誤！請重新輸入。";
                return RedirectToAction("Query");

            }

        }

        public IActionResult SignalR()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                Random rand = new Random();
                string t = rand.Next(1000, 10000).ToString();
                string name = "user" + t;
                ViewBag.Name = name;
                return View();
            }
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGIN_CLIENT);
            Person userlogin = JsonSerializer.Deserialize<Person>(json);
            ViewBag.Name = userlogin.PersonName.ToString();

            return View();
        }

        public IActionResult QandA()
        {
            if (ViewData["Answer"] == null)
            {
                ViewData["Answer"] = "";
            }
            return View();
        }

        [HttpPost]
        public IActionResult QandA(string msg) 
        {
            string Answer = QnA(msg);

            ViewData["Answer"] = Answer;



            return View();
        }

        public IActionResult Logout() 
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_CLIENT))
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGIN_CLIENT);
            }
            return RedirectToAction("Index","Home");
        }
    }
}

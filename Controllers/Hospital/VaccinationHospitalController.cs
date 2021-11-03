using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using VaccinationReservationPlatForm.Models;
using VaccinationReservationPlatForm.ViewModels;

namespace VaccinationReservationPlatForm.Controllers.Hospital
{
    public class VaccinationHospitalController : Controller
    {
        VaccinationBookingSystemContext db = new VaccinationBookingSystemContext();       

        // Part1
        public IActionResult PickVaccinationWorkingDay()
        {            
            List<SelectListItem> VaccineList = new List<SelectListItem>();
            var va2 = from v in db.Vaccines
                      select new
                      {
                          v.VaccineId,
                          v.VaccineName
                      };
            foreach (var item in va2)
            {
                VaccineList.Add(new SelectListItem
                {
                    Value = item.VaccineId.ToString(),
                    Text = item.VaccineName
                });
            }
            ViewBag.VaccineList = VaccineList;
            return View();
        }
        public static string SelectDate = "";
        public static string Vaccineid = "";
        public IActionResult NeedSessionFirst()
        {
            SelectDate = Request.Form["date"];
            Vaccineid = Request.Form["vaccineId"];
            
            if (BookingList().Count() == 0)
            {
                TempData["Emptylist"] = "nodata";
                return RedirectToAction("PickVaccinationWorkingDay");

            }

            return RedirectToAction("VWorkingPage");
        }
        public IEnumerable<Work2ViewModel> BookingList()
        {           
            DateTime date = Convert.ToDateTime(SelectDate).Date;
            int vid = Convert.ToInt32(Vaccineid);

            var workModel = from b in db.VaccinationBookings
                            where b.VbbookingDate == date
                            && b.VaccineId == vid
                            && (b.VbcheckRemark == "登記"
                            || b.VbcheckRemark == "已接踵" //  &&以及 ; ||或
                            || b.VbcheckRemark == "缺席")
                            join p in db.People
                            on b.PersonId equals p.PersonId
                            select new Work2ViewModel
                            {
                                PersonId = p.PersonId,
                                PersonName = p.PersonName,
                                PersonIdentityId = p.PersonIdentityId,
                                Vbnumber = b.Vbnumber,
                                VbcheckRemark = b.VbcheckRemark,
                                VaccineSerialNumber = b.VaccineSerialNumber,
                                VbbookingDate = b.VbbookingDate,
                                VbbookingTime = b.VbbookingTime,
                                VbappointmentDate = b.VbappointmentDate
                            };
            IEnumerable<Work2ViewModel> WorkModel = workModel;
            return WorkModel;
        }

        // Part2
        public IActionResult VWorkingPage(int? PId)
        {
            DateTime date = Convert.ToDateTime(SelectDate).Date;
            int vid = Convert.ToInt32(Vaccineid);

            ViewBag.date = date.ToString("yyyy-MM-dd");

            var vv = from v in db.Vaccines
                     where v.VaccineId == vid
                     select v.VaccineName;
            foreach (var v in vv)
            {
                ViewBag.vName = v;
            }

            List<SelectListItem> VaccineSerialNumber = new List<SelectListItem>();
            var SerialNumber = from sn in db.VaccinePsis
                               where sn.VaccineId == vid
                               && sn.VaccinePsistateRemark == "可施打"
                               && sn.VaccinePsiexpDate >= date
                               //join b in db.VaccinationBookings
                               //on sn.VaccineId equals b.VaccineId
                               //where sn.HospitalId == b.HospitalId
                               select new
                               {
                                   sn.BatchNumber,
                                   sn.VaccinePsiexpDate
                               };

            foreach (var ss in SerialNumber)
            {
                string st = ss.VaccinePsiexpDate.ToString();
                VaccineSerialNumber.Add(new SelectListItem()
                {
                    Text = ss.BatchNumber + "(有效期限-" + st.Remove(st.Length - 11, 11) + ")",
                    Value = ss.BatchNumber
                });
            }

            ViewBag.vn = VaccineSerialNumber;
            ViewBag.thisone = PId;

            return View(BookingList());
        }

        public IActionResult VSucceed()
        {
            DateTime date = Convert.ToDateTime(SelectDate).Date;
            int vid = Convert.ToInt32(Vaccineid);

            DateTime dateSave = Convert.ToDateTime(date).AddMonths(3).Date;
            string vSerialN = Request.Form["vSerialN"].ToString();
            int stateEditID = Convert.ToInt32(Request.Form["stateEditID"]);

            //Booking表單狀態變更
            var VB = from v in db.VaccinationBookings
                     where v.VbbookingDate == date
                     && v.VaccineId == vid
                     && v.PersonId == stateEditID
                     select v;
            if (VB != null)
            {
                if (VB.Where(S => S.VbcheckRemark == "已接踵").Count() == 1)
                {
                    TempData["Succeedalready"] = "Succeedalready";
                    return RedirectToAction("VWorkingPage");
                }
                //else if(VB.Where(S => S.VbcheckRemark == "缺席").Count() == 1)
                //{
                //    TempData["Succeedchangeto"] = "Succeedchangeto";
                //    return RedirectToAction("VWorkingPage");
                //}
                else
                {
                    foreach (var b in VB)
                    {
                        b.VbcheckRemark = "已接踵";
                        b.VbappointmentDate = dateSave;
                        b.VaccineSerialNumber = Convert.ToInt32(vSerialN);
                    }
                }

                ///Record表單新增
                //db.VaccinationRecords.Add(new VaccinationRecord
                //{
                //    PersonId = stateEditID,
                //    HospitalId = 2, //session?
                //    VaccineId = vid,
                //    VrgivenDate = date,
                //    VaccineSerialNumber = Convert.ToInt32(vSerialN),
                //});

                ///Teack表單新增
                int VTtimes = 0;
                var VT1 = from vt in db.VaccinationTracks
                          where vt.PersonId == stateEditID
                          select vt.Vttimes;
                foreach (var t1 in VT1)
                {
                    VTtimes = t1.Value;
                }

                db.VaccinationTracks.Add(new VaccinationTrack
                {
                    PersonId = stateEditID,
                    VaccineId = Convert.ToInt32(vid),
                    Vttimes = VTtimes + 1,
                    VtappointmentDate = dateSave,
                });

                ///扣除
                var PSI = from psi in db.VaccinePsis
                          where psi.BatchNumber == vSerialN
                          select psi;
                          
                foreach (var p in PSI)
                {
                    p.VaccinePsiquantity = p.VaccinePsiquantity-1;
                    if (p.VaccinePsiquantity == 0)
                    {
                        p.VaccinePsirecordTime = DateTime.Now;
                        p.VaccinePsistateRemark = "使用完畢";
                    }
                }
                db.SaveChanges();
                TempData["MarkSuccess"] = "OK";
            }
            return RedirectToAction("VWorkingPage");
        }
        public IActionResult Absence()
        {            
            int stateEdit = Convert.ToInt32(Request.Form["stateEditID"]);

            var vb = from v in db.VaccinationBookings
                     where v.PersonId == stateEdit
                     select v;

            if (vb != null)
            {
                if(vb.Where(a=>a.VbcheckRemark == "缺席").Count() == 1)
                {
                    TempData["Absencealreadt"] = "Absencealreadt";
                    return RedirectToAction("VWorkingPage");
                }
                else if (vb.Where(a => a.VbcheckRemark == "已接踵").Count() == 1)
                {
                    TempData["Absencechangefail"] = "Absencechangefail";
                    return RedirectToAction("VWorkingPage");
                }
                else
                {
                    foreach (var b in vb)
                    {
                        b.VbcheckRemark = "缺席";
                        b.VbappointmentDate = null;
                        b.VaccineSerialNumber = null;
                    }
                    db.SaveChanges();
                }                
            }
            return RedirectToAction("VWorkingPage");
        }

        public IActionResult FinalPage()
        {
            DateTime date = Convert.ToDateTime(SelectDate).Date;
            int vid = Convert.ToInt32(Vaccineid);

            var sss = from v in db.VaccinationBookings
                     where v.VbbookingDate == date
                     && v.VaccineId == vid
                     select v;
            if (sss != null)
            {
                if (sss.Where(S => S.VbcheckRemark == "登記").Count() > 0)
                {
                    TempData["sss"] = "sss";
                    return RedirectToAction("VWorkingPage");
                }
            }

            ViewBag.date = date.ToString("yyyy-MM-dd");     

            var VB = from b in db.VaccinationBookings       
                     where b.VbbookingDate == date
                     && b.VaccineId == vid
                     && b.VbcheckRemark == "已接踵"                     
                     group b by b.VaccineSerialNumber into g
                     select new { 
                         gSerialNumber = g.Key, 
                         gCount = g.Count(),
                     };

            var FQ = from psi in db.VaccinePsis
                     join v in db.Vaccines
                     on psi.VaccineId equals v.VaccineId
                     join fq in VB
                     on Convert.ToInt32(psi.BatchNumber) equals fq.gSerialNumber
                     orderby psi.VaccinePsiid
                     select new CFinalQuery{
                         VaccineNameFQ=v.VaccineName,
                         SerialNumberFQ=fq.gSerialNumber,
                         SingalUseFQ=fq.gCount,
                         RestquantityFQ=psi.VaccinePsiquantity
                     };
            IEnumerable<CFinalQuery> FinalQuery = FQ;

            int TotalUse = 0;
            foreach (var q in FQ)
            {
                 TotalUse += Convert.ToInt32(q.SingalUseFQ);
            }
            ViewBag.TotalUse = TotalUse.ToString();          
            return View(FinalQuery);            
        }

        // Part3
        [HttpPost]
        public IActionResult ReplaceQuantities(string[] newSingalValue)
        {
            DateTime date = Convert.ToDateTime(SelectDate).Date;
            int vid = Convert.ToInt32(Vaccineid);

            var VB = from b in db.VaccinationBookings
                     where b.VbbookingDate == date
                     && b.VaccineId == vid
                     && b.VbcheckRemark == "已接踵"
                     group b by b.VaccineSerialNumber into g                     
                     select new
                     {
                         gSerialNumber = g.Key,
                         gCount = g.Count()
                     };

            List<string> VBlist = new List<string>();
            List<int> intVBlist = new List<int>();
            foreach(var b in VB)
            {
                VBlist.Add(b.gSerialNumber.ToString());
                intVBlist.Add(b.gCount);
            }

            var PSI = from psi in db.VaccinePsis                      
                      where VBlist.Contains(psi.BatchNumber)
                      orderby psi.VaccinePsiid
                      select psi;
            foreach (var p in PSI)
            {
                for (int i = 0; i < newSingalValue.Length; i++)
                {
                    if (p.BatchNumber == VBlist[i])
                    {                        
                        if (newSingalValue[i] == null /*|| Convert.ToInt32(newSingalValue[i]) ==0*/)                        
                            break;                        
                        p.VaccinePsiquantity += intVBlist[i];
                        p.VaccinePsiquantity -= Convert.ToInt32(newSingalValue[i]);
                        break;
                    }                        
                }
            }
            db.SaveChanges();
            TempData["fromReplace"] = "fromReplace";
            return RedirectToAction("PickVaccinationWorkingDay");
        }

        // 疫苗管理
        public IActionResult VaccineManagement()
        {
            return View();
        }

        IEnumerable<CVManageViewModel> PSIquery(string Remark)
        {
            var PSI = from psi in db.VaccinePsis
                      where psi.HospitalId != 1
                      && psi.VaccinePsistateRemark == Remark
                      join v in db.Vaccines
                      on psi.VaccineId equals v.VaccineId
                      join vtd in db.VaccineToDiseases
                      on v.VaccineId equals vtd.VaccineId
                      join d in db.Diseases
                      on vtd.DiseaseId equals d.DiseaseId
                      select new CVManageViewModel
                      {
                          VaccinePsiid=psi.VaccinePsiid,
                          BatchNumber = psi.BatchNumber,
                          VaccineName = v.VaccineName,
                          VaccinePsiquantity = psi.VaccinePsiquantity,
                          DiseaseName = d.DiseaseName,
                          VaccinePsiexpDate = psi.VaccinePsiexpDate,
                          VaccineExp = v.VaccineExp,
                          VaccinePsirecordTime=psi.VaccinePsirecordTime,
                          VaccinePsimfDate=psi.VaccinePsimfDate,
                          VaccinePsistateRemark=psi.VaccinePsistateRemark                        
                      };
            IEnumerable<CVManageViewModel> Iepsi = PSI;
            return Iepsi;
        }
        public IActionResult OnRoad()
        {
            return PartialView("_OnRoad", PSIquery("在途"));                
        }
        public IActionResult Stocks()
        {         
            return PartialView("_Stocks", PSIquery("可施打"));
        }    
        public IActionResult OnRoadOK(int? psiID)
        {
            var PSI = from psi in db.VaccinePsis
                      where psi.VaccinePsiid == psiID
                      select psi;
            foreach(var s in PSI)
            {
                s.VaccinePsistateRemark = "可施打";           
            }           

            db.SaveChanges();
            return RedirectToAction("VaccineManagement");
        }
        public IActionResult VClose()
        {
            return PartialView("_VClose", PSIquery("使用完畢"));
        }
    }
}
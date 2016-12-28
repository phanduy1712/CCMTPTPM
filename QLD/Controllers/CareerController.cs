using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using QLD.Library;
using QLD.Models;

namespace QLD.Controllers
{
    [Authorize, RoutePrefix("Admin/Career")]
    public class CareerController : Controller
    {
        private Entities db = new Entities();
        private SharedFuntionController shared = new SharedFuntionController();
        [Route("Index")]
        [Authorize(Roles = "QL,Career,CareerV")]
        public ActionResult Index(string sortOrder, string searchString, int? numberRow, int? page)
        {
            ViewBag.Mess = 0;
            ViewBag.ArrRow = DefineFuntion.ArrRow;
            ViewBag.NumberRow = Convert.ToInt32(numberRow) <= 0 ? 20 : Convert.ToInt32(numberRow);
            ViewBag.SearchString = searchString;
            DefineSort(sortOrder);
            var obj = db.Careers.Where(t => t.Status != 3).ToList();
            obj = Authorities(obj);
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                var searchStr = DefineFuntion.RemoveCharAndNoSign(searchString);
                obj = obj.Where(s => (s.Name ?? "").ToLower().Contains(searchStr)).ToList();
            }
            obj = EvenSort(obj, sortOrder);
            int pageSize = Convert.ToInt32(numberRow) <= 0 ? 20 : Convert.ToInt32(numberRow);
            int pageNumber = (page ?? 1);
            return View(obj.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [Route("Index")]
        [Authorize(Roles = "QL,Career,CareerV")]
        public ActionResult Index(FormCollection form, string typeName, string searchString, int? numberRow)
        {
            SetDefineSort();
            ViewBag.SearchString = searchString;
            ViewBag.ArrRow = DefineFuntion.ArrRow;
            ViewBag.NumberRow = Convert.ToInt32(numberRow) <= 0 ? 20 : Convert.ToInt32(numberRow);
            var inputCheck = form.GetValues("inputCheck");
            ViewBag.Mess = EvenList(inputCheck, typeName);
            if (ViewBag.Mess == 7)
                return RedirectToAction("Edit", "Career", new { Id = ViewBag.id, mess = 0 });
            var obj = db.Careers.Where(t => t.Status < 3).ToList();
            obj = Authorities(obj);
            if (typeName == "refresh")
            {
                ViewBag.SearchString = "";
            }
            else if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                searchString = searchString.ToLower();
                obj = db.Careers.Where(c => (c.Name ?? "").ToLower().Contains(searchString)).ToList();
            }
            int pageSize = (numberRow ?? 20);
            int pageNumber = 1;
            return View(obj.ToPagedList(pageNumber, pageSize));

        }
        [Authorize(Roles = "QL,Career,CareerU")]
        private int EventStatus(string[] inputCheck, string typeName)
        {
            int rowFinish = 0;
            if (inputCheck == null) return rowFinish;
            foreach (var s in inputCheck)
            {
                try
                {
                    int id = Convert.ToInt32(DefineFuntion.Decrypt(s));
                    var obj = db.Careers.Find(id);
                    if (typeName == "activity")
                        obj.Status = 1;//Cho phép dùng
                    else if (typeName == "inactive")
                        obj.Status = 2;//Không cho dung 
                    else if (typeName == "trash")
                        obj.Status = 3;//Dưa vào thùng rác
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    rowFinish++;
                }
                catch (Exception) { }
            }
            return rowFinish;
        }
        private int EvenList(string[] inputCheck, string typeName)
        {
            int rowFinish = 0;
            int ms = 0;
            if (inputCheck != null)
            {
                int rowTotla = inputCheck.Count();
                switch (typeName)
                {
                    case "activity":
                        ms = 5; rowFinish = EventStatus(inputCheck, "activity");
                        break;
                    case "inactive":
                        ms = 5; rowFinish = EventStatus(inputCheck, "inactive");
                        break;
                    case "public":
                        ms = 5; rowFinish = EventStatus(inputCheck, "public");
                        break;
                    case "private":
                        ms = 5; rowFinish = EventStatus(inputCheck, "private");
                        break;
                    case "publicnote":
                        ms = 5; rowFinish = EventStatus(inputCheck, "publicnote");
                        break;
                    case "privatenote":
                        ms = 5; rowFinish = EventStatus(inputCheck, "privatenote");
                        break;
                    case "trash":
                        ms = 5; rowFinish = EventStatus(inputCheck, "trash");
                        break;
                    case "delete":
                        ms = 6; rowFinish = DeleteConfirmed(inputCheck);
                        break;
                    case "edit":
                        ms = 7; ViewBag.id = inputCheck.First();
                        break;

                }
                ViewBag.rowError = rowTotla - rowFinish;
            } ViewBag.rowFinish = rowFinish;
            return ms;
        }
        private void DefineSort(string sortOrder)
        {
            ViewBag.Code = sortOrder == "Code" ? "Code_desc" : "Code";
            ViewBag.Name = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.Status = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.CreateBy = sortOrder == "CreateBy" ? "CreateBy_desc" : "CreateBy";
            ViewBag.CreateDay = sortOrder == "CreateDay" ? "CreateDay_desc" : "CreateDay";
        }
        private List<Career> EvenSort(List<Career> obj, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Name":
                    obj = obj.OrderBy(s => s.Name).ToList();
                    break;
                case "Name_desc":
                    obj = obj.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Status":
                    obj = obj.OrderBy(s => s.Status).ToList();
                    break;
                case "Status_desc":
                    obj = obj.OrderByDescending(s => s.Status).ToList();
                    break;
                 
                default:
                    obj = obj.OrderByDescending(s => s.CareerId).ToList();
                    break;
            }
            return obj;
        }

        private List<Career> Authorities(List<Career> obj)
        {
            //var objTourGroup = DefineFuntion.CheckRole("CareerGroup");

            //if (objTourGroup)
            //{
            //    var objU = db.Users.AsEnumerable().Where(t => t.Type == 1 && t.UserId == DefineFuntion.UserId()).FirstOrDefault();
            //    var department = objU.Department.Split(',');
            //    obj = obj.AsEnumerable().Where(t => t.DepartmentId != "null" && department.Intersect(JsonConvert.DeserializeObject<string[]>(t.DepartmentId ?? "[]")).Count() > 0).ToList();
            //}

            return obj;
        }
        private void SetDefineSort()
        {
            ViewBag.Code = "Code";
            ViewBag.Name = "Name";
            ViewBag.Status = "Status";
            ViewBag.CreateBy = "CreateBy";
            ViewBag.CreateDay = "CreateDay";

        }
        [Authorize(Roles = "QL,Career,CareerD")]
        public int DeleteConfirmed(string[] inputCheck)
        {
            int rowFinish = 0;
            if (inputCheck == null) return rowFinish;
            foreach (var s in inputCheck)
            {

                try
                {
                    int idMenu = Convert.ToInt32(DefineFuntion.Decrypt(s));
                    Career obj = db.Careers.Find(idMenu);
                    //1-them, 2- sua, 3-xoa, 4- khac
                    shared.CreateHistory(new History()
                    {
                        Name = "Xóa ngành",
                         Contens = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }),
                        ItemId = obj.CareerId, 
                        Type = (int)DefineFuntion.TypeHistory.Career,
                    }); 
                    db.Careers.Remove(obj);
                    db.SaveChanges();
                    rowFinish++;

                }
                catch (Exception)
                {
                    int idMenu = Convert.ToInt32(DefineFuntion.Decrypt(s));
                    Career obj = db.Careers.Find(idMenu);
                    obj.Status = 3;
                    db.SaveChanges();

                }
            } 
            return rowFinish;
        }
        [Route("Trash")]
        [Authorize(Roles = "QL,Career,CareerV")]
        public ActionResult Trash(string sortOrder, string searchString, int? nunberRow, int? page)
        {
            ViewBag.Mess = 0;
            ViewBag.ArrRow = DefineFuntion.ArrRow;
            ViewBag.NunberRow = Convert.ToInt32(nunberRow) <= 0 ? 20 : Convert.ToInt32(nunberRow);
            ViewBag.SearchString = searchString;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "Id_desc" : "";
            DefineSort(sortOrder);
            var obj = db.Careers.Where(t => t.Status == 3).ToList();
            obj = Authorities(obj);
            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
                obj = obj.Where(s => (s.Name ?? "").ToLower().Contains(searchString.ToLower()) && s.Status == 3).ToList();
            }
            obj = EvenSort(obj, sortOrder);  
            int pageSize = Convert.ToInt32(nunberRow) <= 0 ? 20 : Convert.ToInt32(nunberRow);
            int pageNumber = (page ?? 1);
            return View(obj.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [Route("Trash")]
        [Authorize(Roles = "QL,Career,CareerV")]
        public ActionResult Trash(FormCollection form, string typeName, string searchString, int? nunberRow)
        { 
            SetDefineSort();
            ViewBag.SearchString = searchString;
            ViewBag.ArrRow = DefineFuntion.ArrRow;
            ViewBag.NunberRow = Convert.ToInt32(nunberRow) <= 0 ? 20 : Convert.ToInt32(nunberRow);
            var inputCheck = form.GetValues("inputCheck");
            int rowFinish = 0;
            int ms = 0;
            #region even
            if (inputCheck != null)
            {
                int rowTotla = inputCheck.Count();
                switch (typeName)
                {
                    case "activity":
                        ms = 5; rowFinish = EventStatus(inputCheck, "activity");
                        break;
                    case "inactive":
                        ms = 5; rowFinish = EventStatus(inputCheck, "inactive");
                        break;
                    case "public":
                        ms = 5; rowFinish = EventStatus(inputCheck, "public");
                        break;
                    case "private":
                        ms = 5; rowFinish = EventStatus(inputCheck, "private");
                        break;
                    case "publicnote":
                        ms = 5; rowFinish = EventStatus(inputCheck, "publicnote");
                        break;
                    case "privatenote":
                        ms = 5; rowFinish = EventStatus(inputCheck, "privatenote");
                        break;
                    case "trash":
                        ms = 5; rowFinish = EventStatus(inputCheck, "trash");
                        break;
                    case "delete":
                        ms = 6; rowFinish = DeleteConfirmed(inputCheck);
                        break;
                    case "edit":
                        return RedirectToAction("Edit", new { id = inputCheck.First(), type = "saveedit" });

                } ViewBag.rowError = rowTotla - rowFinish;
            }
            ViewBag.rowFinish = rowFinish;
            ViewBag.Mess = ms;
            #endregion
            var obj = db.Careers.Where(t => t.Status == 3).OrderByDescending(t => t.CareerId).ToList();
            obj = Authorities(obj);
            if (typeName == "refresh")
            {
                ViewBag.SearchString = "";
            }
            else if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.SearchString = searchString;
                searchString = searchString.ToLower();
                obj = db.Careers.Where(c => (c.Name ?? "").ToLower().Contains(searchString)).ToList();
            }
            int pageSize = (nunberRow ?? 20);
            int pageNumber = 1;
            return View(obj.ToPagedList(pageNumber, pageSize));

        }
        [Route("Create")]
        [Authorize(Roles = "QL,Career,CareerC")]
        public ActionResult Create(int? mess)
        {
            ViewBag.Mess = (mess ?? 0);
            ViewBag.Status = new SelectList(DefineFuntion.ListStatus, "Value", "Text", 1);
            return View( );
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [ValidateInput(false)]
        [Authorize(Roles = "QL,Career,CareerC")]
        public ActionResult Create(Career career, string typeName )
        { 
            var shared = new SharedFuntionController();
            int ms = 0; 
            try
            {  

                db.Careers.Add(career);
                db.SaveChanges();
                ms = 1;
                //1-them, 2- sua, 3-xoa, 4- khac
                shared.CreateHistory(new History()
                {
                    Name = "Thêm ngành",
                    Contens = "",
                     ItemId = career.CareerId, 
                    Type = (int)DefineFuntion.TypeHistory.Career,
                }); 
            }
            catch (Exception)
            {
                ms = 2;
            }
            if (ms != 2)
            {
                if (typeName == "savenew" || typeName == "")
                    return RedirectToAction("Create", new { mess = ms });
                else
                {
                    return RedirectToAction("Edit", new { id = DefineFuntion.Encrypt(career.CareerId), mess = ms });
                }
            }
            ViewBag.Status = new SelectList(DefineFuntion.ListStatus, "Value", "Text", career.Status);
            return View();
        }
        // GET: /Menu/Edit/5
        [Route("Edit")]
        [Authorize(Roles = "QL,Career,CareerU")]
        public ActionResult Edit(string id, int? mess)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int obj = Convert.ToInt32(DefineFuntion.Decrypt(id));
            var career = db.Careers.Find(obj);
            if (career == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mess = (mess ?? 0);
            ViewBag.Status = new SelectList(DefineFuntion.ListStatus, "Value", "Text", career.Status);

            return View(career);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        [ValidateInput(false)]
        [Authorize(Roles = "QL,Career,CareerU")]
        public ActionResult Edit(Career career,  string codeSystem, string typeName )
        {
            career.CareerId = Convert.ToInt32(DefineFuntion.Decrypt(codeSystem));
            int ms = 0;
            var me = db.Careers.Find(career.CareerId);
            try
            {  
                me.Name = career.Name;
                me.Status = career.Status; 
                me.Note = career.Note;  
                db.SaveChanges(); 
                ms = 3;//Nếu cập nhật thành công
                //1-them, 2- sua, 3-xoa, 4- khac
                shared.CreateHistory(new History()
                {
                    Name = "Xóa ngành",
                    Contens = JsonConvert.SerializeObject(me, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }),
                    ItemId = me.CareerId, 
                    Type = (int)DefineFuntion.TypeHistory.Career,
                });  
            }
            catch (Exception)
            {
                ms = 4;//Nếu cập nhật không thành công
            } 
            if (typeName == "savenew" || typeName == "")
                return RedirectToAction("Create", new { mess = ms });
            else
                return RedirectToAction("Edit", new { id = codeSystem, mess = ms });  
        }
    
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
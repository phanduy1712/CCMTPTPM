using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QLD.Controllers;
using QLD.Library;
using QLD.Models;

namespace QLD.Controllers.BackEnd
{

    public class AdminController : Controller
    {
        private  Entities db = new  Entities();
        private SharedFuntionController share = new SharedFuntionController();
        // GET: /Admin/share
        [AllowAnonymous]
        public ActionResult Header()
        {
            var id = DefineFuntion.UserId();
            ViewBag.User = db.Users.Find(id);
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult MenuLeft()
        {
            //var obj = Membership.GetUser();
            var idUs = DefineFuntion.UserId();
            ViewBag.UserRoles = share.GetRolesUser();
            // db.UserRoles.Where(t => t.UserId == idUs).Select(t => t.Role.Code).ToArray();
            ViewBag.UserRolesMenu = share.GetRolesUserMenu();
            ViewBag.User = db.Users.Find(idUs);

            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult ContentHeader()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult Footer()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult Notice(int? mess, int? rowError, int? rowFinish)
        {
            //1-Thành công, 2-Thất bại, 3-Cảnh báo
            if (mess == 0)
            {
                ViewBag.Mess = 0;
                ViewBag.Notice = DefineFuntion.Notice[0];
                return PartialView();
            }
            else if (mess == 5)
            {
                if (rowFinish != 0 && rowError == 0)
                {
                    ViewBag.Mess = 1;
                    ViewBag.Notice = String.Format(DefineFuntion.Notice[5], rowError + rowFinish, rowFinish);
                }
                else
                {
                    ViewBag.Mess = 3;
                    ViewBag.Notice = String.Format(DefineFuntion.Notice[6], rowError + rowFinish, rowFinish, rowError);
                }
            }
            else if (mess == 6)
            {
                if (rowFinish != 0 && rowError == 0)
                {
                    ViewBag.Mess = 1;
                    ViewBag.Notice = String.Format(DefineFuntion.Notice[7], rowError + rowFinish, rowFinish);
                }
                else
                {
                    ViewBag.Mess = 3;
                    ViewBag.Notice = String.Format(DefineFuntion.Notice[8], rowError + rowFinish, rowFinish, rowError);
                }
            }
            else if (mess == 9)
            {

                ViewBag.Mess = 3;
                ViewBag.Notice = String.Format(DefineFuntion.Notice[9]);
            }
            else
            {
                if ((mess % 2) == 0) ViewBag.Mess = 2;
                else ViewBag.Mess = 1;
                ViewBag.Notice = DefineFuntion.Notice[mess ?? 0];
            }

            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult NoticeString(int mess, string str)
        {
            //1-Thành công, 2-Thất bại, 3-Cảnh báo 
            ViewBag.Mess = mess;
            ViewBag.Notice = str;
            return PartialView();
        }

        [Route("Admin/Index")]
        public ActionResult Index()
        {
            return View();
        }
        #region -------------Tài khoản-------------
        [Route("Admin/Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous, Route("Admin/Login")]
        public ActionResult Login(User model, string returnUrl)
        {

            if (Membership.ValidateUser(model.Account, model.Password) && ModelState.IsValid)
            {
                //SessionHelper.SetSession(new UserSession(){UserName = model.UserName});
                var obj = db.Users.AsNoTracking().Where(t => t.Account == model.Account && t.Status ==1).FirstOrDefault();
                if (obj != null)
                {
                    FormsAuthentication.SetAuthCookie(obj.Account, obj.Remember);
                    var objU = db.Users.Find(obj.UserId);
                    Session["Account"] = obj.Account;
                    Session["UserId"] = obj.UserId; 
                    if (objU != null)
                    {
                        //var objRS = db.UserGroupDetails.Where(t => t.UserId == obj.UserId).Select(t => t.RoleGroupId).ToArray();
                        //var objR = db.RoleGroupDetails.Where(t => objRS.Contains(t.RoleGroupId)).Select(t => t.Role.Code).ToArray();
                        //if (objR.Contains("QL"))
                        //{
                        //    Session["FolderFile"] = "";
                        //}
                        //else
                        //{
                        //    Session["FolderFile"] = " ";
                        //}

                          objU = db.Users.Find(obj.UserId);
                        ViewBag.ReturnUrl = "admin/index";
                        ViewBag.action = "ExternalLogin";
                    }

                    else { RedirectToAction("Login", "Admin"); }
                    if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("../" + returnUrl);
                    return RedirectToAction("Index", "Admin");
                }
            }
            else { RedirectToAction("Login", "Admin"); }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");

            return View(model);
        }
        [AllowAnonymous, Route("Admin/Logout")]
        public ActionResult Logout()
        {
               FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        } 
        [AllowAnonymous, Route("ChangePassword")]
        public ActionResult ChangePassword(int? mess)
        {
            ViewBag.Mess = (mess ?? 0);
            return View();
        }
        [HttpPost]
        [AllowAnonymous, Route("ChangePassword")]
        public ActionResult ChangePassword(string OldPass, string NewPass, string ReNewPass)
        {
            int ms = 0;
            string str = "";
            if (NewPass == ReNewPass)
            {
                if (!string.IsNullOrWhiteSpace(OldPass) && !string.IsNullOrWhiteSpace(NewPass) &&
                    !string.IsNullOrWhiteSpace(ReNewPass))
                {
                    var id = DefineFuntion.UserId();
                    var obj = db.Users.Find(id);
                    if (obj != null)
                    {
                        if (obj.Password == DefineFuntion.Encrypt(OldPass))
                        {
                               obj.Password = DefineFuntion.Encrypt(ReNewPass);
                            obj.RePassDay = DateTime.Now;
                            db.SaveChanges();
                        }
                        else
                        {
                            ms = 3;
                            str = "Mật khẩu củ không đúng!";
                        }
                    }
                    else
                    {
                        ms = 3;
                        str = "Vui lòng đăng nhập!";
                    }
                }
                else
                {
                    ms = 3;
                    str = "Vui lòng nhập đầy đủ thông tin!";
                }
            }
            else
            {
                ms = 3;
                str = "Nhập lại mật khẩu sai!";
            }
            ViewBag.Mess = ms;
            ViewBag.Str = str;
            return View();
        }
        #endregion

       
        public ActionResult Forgot()
        {
            return View();
        }
         
        public ActionResult ViewButtomEdit(int? type, bool? showE, bool? showC)
        {
            ViewBag.Type = type;
            ViewBag.ShowE = showE;
            ViewBag.ShowC = showC;
            return PartialView();
        }
        public ActionResult ViewButtomCreate(int? type, bool? showE, bool? showC)
        {
            ViewBag.Type = type;
            ViewBag.ShowE = showE;
            ViewBag.ShowC = showC;
            return PartialView();
        }
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using QLD.Controllers;

namespace QLD.Library
{
    public static class DefineFuntion
    {
        public enum MusterStatus
        {
            Co = 1,
            VangKp = 2,
            VangCp = 3
        }

        public enum TypeHistory
        {
            Career = 1,
            Council = 2,
            Muster = 3,
            Plan = 4,
            Point = 5,
            Progress = 6,
            RoleGroup = 7,
            RoleGroupDetail = 8,
            School = 9,
            Semester = 10,
            Standard = 11,
            Student = 12,
            Teacher = 13,
            User = 14,
            UserGroupDetail = 15,
            Year = 16,
            paymen = 17,
            service = 18,
            moneydeal = 19,
            carddeal = 20,
            Partner = 21
        }

        public enum TypeUser
        {
            User = 1,
            Teacher = 2
        }

        public static readonly int[] ArrRow = {20, 30, 50, 100, 150, 200, 300};

        public static readonly List<SelectListItem> ListStatus = new List<SelectListItem>
        {
            new SelectListItem {Selected = false, Text = "Cho phép dùng", Value = "1"},
            new SelectListItem {Selected = true, Text = "Không cho dùng", Value = "2"}
        };

        public static readonly List<SelectListItem> ListMusterStatus = new List<SelectListItem>
        {
            new SelectListItem {Selected = false, Text = "Có", Value = "1"},
            new SelectListItem {Selected = true, Text = "Vắng KP", Value = "2"},
            new SelectListItem {Selected = true, Text = "Vắng CP", Value = "3"}
        };

        public static readonly List<SelectListItem> ListType = new List<SelectListItem>
        {
            new SelectListItem {Selected = false, Text = "Đại lý", Value = "1"},
            new SelectListItem {Selected = true, Text = "Nhà cung cấp", Value = "2"}
        };
        public static readonly List<SelectListItem> ListSex = new List<SelectListItem>
        {
            new SelectListItem {Selected = false, Text = "Nam", Value = "1"},
            new SelectListItem {Selected = true, Text = "Nữ", Value = "2"}
        };

        public static readonly string[] Notice =
        {
            "",
            "Thêm thành công!",
            "Thêm không thành công!",
            "Cập nhật thành công!",
            "Cập nhật không thành công!",
            "Tổng cộng {0} dòng, hiện thành công {0} dòng!",
            "Tổng cộng {0} dòng, hiện thành công {1} dòng, hiện không thành công {2} dòng!",
            "Tổng cộng {0} dòng, xóa thành công {1} dòng!",
            "Tổng cộng {0} dòng, xóa thành công {1} dòng, xóa không thành công {2} dòng!",
            "Không thành công"
        };

        private static readonly string[] VietNamChar =
        {
            "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "ệéèẹẻẽêếềểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ"
        };

        private static readonly string strRegex =
            @"/!|@|%|–|\^|\*|\\|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|̣|\&|\#|\[|\]|~|$|!|\||\$";

        public static string Host = WebConfigurationManager.AppSettings["Host"];


        public static string Name = WebConfigurationManager.AppSettings["Name"];
        public static string CDN = WebConfigurationManager.AppSettings["CDN"];

        public static string CodeGuid()
        {
            //Guid guidId = Guid.NewGuid();
            var date = DateTime.Now;
            //return guidId.ToString();
            return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond + "";
        }

        public static string ConvertNoSign(string str)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            return str;
        }

        public static string RemoveChar(string str)
        {
            str = Regex.Replace(str, strRegex, "");
            return str;
        }

        public static string RemoveCharAndNoSign(string str)
        {
            str = ConvertNoSign(Regex.Replace(str.Trim().Replace(" ", "-"), strRegex, ""));

            return str.Replace("-", "");
        }

        public static string ConvertSulg(string str)
        {
            var strChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890";
            if (str == null)
                return "";
            var strF = str.Replace(" ", "-");
            var strL = "";
            for (int i = 0; i < strF.Length; i++)
                if (strChar.Contains(strF[i].ToString()))
                    strL = strL + ConvertNoSign(strF[i].ToString());
                else
                    foreach (var straV in VietNamChar)
                    {
                        var a = straV.Contains(strF[i].ToString());
                        if (a)
                            strL = strL + ConvertNoSign(strF[i].ToString());
                    }
            str = ConvertNoSign(Regex.Replace(strL.Trim().Replace(" ", "-"), strRegex, ""));
            str = str.Trim().Replace("--", "");
            return str.ToLower();
        }

        public static string ConvertNoWhiteSpace(string str)
        {
            str = ConvertNoSign(Regex.Replace(str.Trim().Replace(" ", ""), strRegex, ""));
            str = str.Trim().Replace("--", "");
            return str.ToLower();
        }

        public static string GetSlug(string str)
        {
            str = ConvertSulg(str).ToLower().Replace(" ", "-");
            return str;
        }

        public static string GetSearch(string str)
        {
            str = RemoveCharAndNoSign(str).ToLower().Replace(" ", "");
            return str;
        }

        public static string Encrypt(dynamic pstrText)
        {
            try
            {
                string key = "jdsg432387#a";
                //string key = "txOGZuyY^va9IqG*G#6^FP^W";
                byte[] EncryptKey;
                byte[] IV = {55, 34, 87, 64, 87, 195, 54, 21};
                EncryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByte = Encoding.UTF8.GetBytes(pstrText.ToString());
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV),
                    CryptoStreamMode.Write);
                cStream.Write(inputByte, 0, inputByte.Length);
                cStream.FlushFinalBlock();
                return HttpUtility.UrlEncode(Convert.ToBase64String(mStream.ToArray()));
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Decrypt(dynamic pstrText)
        {
            try
            {
                var str = (pstrText.ToString() + "").Contains("%")
                    ? HttpUtility.UrlDecode(pstrText.ToString())
                    : pstrText.ToString();
                str = (str.ToString() + "").Contains("%") ? HttpUtility.UrlDecode(str.ToString()) : str.ToString();
                //string key = "txOGZuyY^va9IqG*G#6^FP^W";
                byte[] DecryptKey = {};
                byte[] IV = {55, 34, 87, 64, 87, 195, 54, 21};
                byte[] inputByte = new byte[str.Length];
                string key = "jdsg432387#a";
                //string key = "txOGZuyY^va9IqG*G#6^FP^W";
                DecryptKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByte = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByte, 0, inputByte.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static int UserId()
        {
            var id = 0;
            try
            {
                var obj = Membership.GetUser();
                // id = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                if (obj != null)
                    id = Convert.ToInt32(obj.ProviderUserKey);
                if (id == 0) HttpContext.Current.Response.Redirect("/Admin/Login");
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Redirect("/Admin/Login");
            }
            return id;
        }

        public static string UserAcount()
        {
            var id = "";
            try
            {
                id = HttpContext.Current.Session["Account"].ToString();
                if (string.IsNullOrWhiteSpace(id)) HttpContext.Current.Response.Redirect("/Admin/Login");
            }
            catch (Exception)
            {
                HttpContext.Current.Response.Redirect("/Admin/Login");
            }
            return id;
        }

        public static bool UserType()
        {
            var obj = Membership.GetUser();
            if (obj != null)
                if (obj.PasswordQuestion == "1") return true;
                else return false;
            return false;
        }

        public static void DeleteFile(string fileName)
        {
            if (File.Exists(Host + fileName))
                File.Delete(Host + fileName);
        }

        public static string GetFomartDay(DateTime? day)
        {
            if (day != null)
                return (day ?? DateTime.MinValue).ToString("dd/MM/yyyy");
            return "";
        }

        public static bool CheckRole(string keyRole)
        {
            //if (DefineFuntion.UserName() == "Admin") return true;
            var objKey = keyRole.Split(',');
            var roler = new SharedFuntionController().GetRolesUser();
            var obj = roler.Intersect(objKey);
            return obj.Count() > 0 ? true : false;
        }

        public static bool CheckRole(string keyRole, int idUser)
        {
            //if (DefineFuntion.UserName() == "Admin") return true;
            var objKey = keyRole.Split(',');
            var roler = new SharedFuntionController().GetRolesUser(idUser);
            var obj = roler.Intersect(objKey);
            return obj.Count() > 0 ? true : false;
        }

        public static bool CheckRoleMenu(string keyRole)
        {
            //if (DefineFuntion.UserName() == "Admin") return true;
            var objKey = keyRole.Split(',');
            var roler = new SharedFuntionController().GetRolesUserMenu();
            var obj = roler.Intersect(objKey);
            return obj.Count() > 0 ? true : false;
        }

        public static decimal ConvertPrice(string strPrice)
        {
            return decimal.Parse(strPrice.Replace(".", "").Replace("đ", "") ?? "0");
        }

        public static string GetSlugTour(string slug)
        {
            string str = Host + slug;
            return str;
        }

        public static string GetSlugHotel(string slug)
        {
            string str = "";
            if (!slug.Contains("/"))
                str = Host + "khach-san/" + slug;
            else
                str = Host + slug;
            //var db = new Entities();
            //var share = new SharedFuntionController();
            //var objCategory = db.Categories.SqlQuery("SELECT * FROM Category WHERE MetaSlug=@p0", share.UrlDecode(slug)).FirstOrDefault(); 
            //var objC = GetCategoryParent(objCategory.MetaSlug, 1);

            return str;
        }

        public static string GetSlugMenu(string slug)
        {
            string str = Host + slug;
            return str;
        }

        public static string FormatDay(DateTime? day)
        {
            var da = (day ?? DateTime.MinValue).ToString("dd-MM-yyyy");
            return da;
        }

        public static string FormatDay(DateTime? day, string custom)
        {
            var da = (day ?? DateTime.MinValue).ToString(custom);
            return da;
        }

        public static string generateCode()
        {
            Guid guidId = Guid.NewGuid();
            return guidId.ToString();
        }

        public static string RandomStringGenerate()
        {
            int size = 20;
            Random _rng = new Random();
            string _chars = "jasdh566hJH123AKJ@45SDHJEQ&WE87312U@asd7a6869dsfdsf@asdzx90#c8809d9879@3D06768$7A634S43D";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            return new string(buffer);
        }


        public static string GetImageUrl(string src)
        {
            if (src != null)
            {
                string str = CDN.Trim() == "" ? Host.Trim() : CDN.Trim();
                str = str + src;
                return str;
            }
            return "";
        }

        public static string GetNameWeb()
        {
            return Name;
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static string GetLinkBookTour(dynamic id)
        {
            return "/thanh-toan-tour-du-lich?id=" + Encrypt(id);
        }

        public static string GetLinkBookVisa(dynamic id)
        {
            return "/thanh-toan-mua-visa?id=" + Encrypt(id);
        }

        public static string GetLinkBookCar(dynamic id)
        {
            return "/thanh-toan-thue-xe-du-lich?id=" + Encrypt(id);
        }
    }
}
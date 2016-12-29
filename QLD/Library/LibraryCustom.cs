using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLD.Models;

namespace QLD.Library
{
    public class LibraryCustom
    {
        private  Entities db = new  Entities(); 
           public SelectList GetStatus(int? id)
        {
            var list =
            new List<SelectListItem>
            {
                new SelectListItem {Selected = false, Text = "Cho phép dùng", Value = "1"},
                new SelectListItem {Selected = true, Text = "Không cho dùng", Value = "2"}
            };
            var obj = new SelectList(list, "Value", "Text", id);
            return obj;
        }
      
        public SelectList GetSort(int? id)
        {
            string[] str = new[] { "Sấp xếp", "Tăng năm sản xuất", "Giản năm sản xuất", "Tăng ngày đăng", "Giảm ngày đăng", "Tăng ngày cập nhật", "Giảm ngày cập nhật" };
          
            var objS = str.Select((r, index) => new   {Text = r, Value = index  });
            var obj = new SelectList(objS, "Value", "Text", id);
            return obj;
        }
      
        public string GetLanguage()
        {
            string Language = "";
            try
            {
                Language = HttpContext.Current.Request.Cookies["_Language"].Value;
            }
            catch (Exception)
            {
                Language = "vi-vn";
            }
            return Language;
        } 
        public bool DeleteFile(string str)
        {
            var fullPath = System.Web.HttpContext.Current.Server.MapPath("~" + str);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
              return true;
            }
            return false;
        }
    }
    public class SubMenuBackEnd
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<int> Leve { get; set; }
    }

}
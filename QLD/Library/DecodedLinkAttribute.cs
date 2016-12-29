using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace QLD.Library
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DecodedLinkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();
            if (HttpContext.Current.Request.QueryString.Get("id") != null)
            {
                string encryptedQueryString = HttpContext.Current.Request.QueryString.Get("id");
                string decrptedString = Decrypt(encryptedQueryString.ToString());
                string fullQuery = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.ToString()).Replace(encryptedQueryString, decrptedString);
                string[] paramsArrs = fullQuery.Split('&');
                for (int i = 0; i < paramsArrs.Length; i++)
                {
                    string[] paramArr = paramsArrs[i].Split('=');
                    try
                    {
                        decryptedParameters.Add(paramArr[0], Convert.ToInt32(paramArr[1]));
                    }
                    catch (Exception)
                    {
                        decryptedParameters.Add(paramArr[0], paramArr[1]);
                    }

                }
            }
            for (int i = 0; i < decryptedParameters.Count; i++)
            {
                filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = decryptedParameters.Values.ElementAt(i);
            }
            base.OnActionExecuting(filterContext);

        }
        private string Decrypt(string encryptedText)
        {
            string key = "jdsg432387#";
            byte[] DecryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] inputByte = new byte[encryptedText.Length];

            DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByte = Convert.FromBase64String(encryptedText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByte, 0, inputByte.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
    }
}
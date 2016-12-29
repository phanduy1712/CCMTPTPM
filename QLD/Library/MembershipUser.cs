using System; 
using System.Linq;
using System.Web;
using System.Web.Security; 
using QLD.Library;
using QLD.Models;

namespace QLD.Library
{
    public class MembershipUser : MembershipProvider
    {

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var db = new Entities();
            try
            {
              var  passwordA = DefineFuntion.Decrypt("fJnv5JVOJ4Rx%2bylSTX60kHVPGZPfEn8u");
                password = DefineFuntion.Encrypt(password.Trim());
                var obj = db.Users.Where(t => t. Account == username && t.Password == password).FirstOrDefault();  
                if (obj != null)
                {
                    if (obj.Status != 1) return false;
                    HttpContext.Current.Session["UserId"] = obj.UserId;
                    db.Dispose();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            db.Dispose();

            return false;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            var db = new  Entities();
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Account == username);
                if (user != null)
                {
                    var type = 1;  
                    var memUser = new System.Web.Security.MembershipUser("CustomMembershipProvider", username,
                        user.UserId, type.ToString(), string.Empty, string.Empty, true, false, DateTime.MinValue,
                        DateTime.MinValue, DateTime.MinValue, DateTime.Now, DateTime.Now);

                    db.Dispose();

                    return memUser;
                }
                else
                {
                    db.Dispose();

                    HttpContext.Current.Response.Redirect(  "/Admin/Login");
                }
            }
            catch (Exception)
            {
                db.Dispose();

                HttpContext.Current.Response.Redirect(  "Admin/Login");
            }

            db.Dispose();

            return null;
        }
   
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}
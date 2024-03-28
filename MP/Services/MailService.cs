using System.Net.Mail;

namespace MP.Services
{
    public class MailService
    {
        private readonly static string gmail_Account = "lu24309044";
        private readonly static string gmail_Password = "bgvd tskt wsoz wccx";
        private readonly static string gmail_mail = "lu24309044@gmail.com";
        #region 產生驗證碼
        public string AuthCode()
        {
            string[] Code = { "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
                              "P","Q","R","S","T","U","V","W","X","Y","Z","a","b","c",
                              "d","e","f","g","h","i","j","k","l","m","n","p","q","r",
                              "s","t","u","v","w","x","y","z","1","2","3","4","5","6","7","8","9"};
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for(int i=0;i<10;i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }
        #endregion
        #region 產生驗證信
        public string GetMailBody(string Temp,string account,string ValidatrUrl)
        {
            Temp = Temp.Replace("{{UserName}}", account);
            Temp = Temp.Replace("{{ValidateUrl}}", ValidatrUrl);
            return Temp;
        }
        #endregion
        #region 寄送驗證信
        public void SendMail(string mailBody, string toEmail)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential(gmail_Account,gmail_Password);
            smtp.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_mail);
            mail.To.Add(toEmail);
            mail.Subject = "會員註冊信";
            mail.Body = mailBody;
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }
        #endregion
    }
}

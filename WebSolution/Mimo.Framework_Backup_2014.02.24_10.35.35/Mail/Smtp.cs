using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using log4net;
using System.Text.RegularExpressions;

namespace Mimo.Framework.Mail
{
    public class Smtp
    {
        /// <summary>
        /// logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Smtp));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpPort"></param>
        /// <param name="smtpHost"></param>
        /// <param name="from"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        public void Send(int smtpPort,
            string smtpHost,
                        string from,
                         string login,
                         string password,
                         string to,
                         string subject,
                         string body,
                         bool isHTML)
        {
            Send(smtpPort,
                smtpHost,
                from,
                 login,
                 password,
                 to,
                 subject,
                 body,
                 isHTML,
                 null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpPort"></param>
        /// <param name="smtpHost"></param>
        /// <param name="from"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        /// <param name="file"></param>
        public void Send(int smtpPort,
            string smtpHost,
            string from,
                         string login,
                         string password,
                         string to,
                         string subject,
                         string body,
                         bool isHTML,
                         HttpPostedFile file)
        {
            try
            {
                MailAddress maFrom = new MailAddress(from);
                MailAddress maTo = new MailAddress(to);
                MailMessage mmMessage = new MailMessage(maFrom,
                                                        maTo);

                //mmMessage.ReplyTo = maFrom;
                mmMessage.ReplyToList.Add(maFrom);
                mmMessage.Subject = subject;
                mmMessage.SubjectEncoding = Encoding.UTF8;
                mmMessage.BodyEncoding = Encoding.UTF8;
                mmMessage.IsBodyHtml = isHTML;

                if (isHTML)
                {
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(Regex.Replace(body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    mmMessage.AlternateViews.Add(plainView);
                    mmMessage.AlternateViews.Add(htmlView);
                }
                else
                {
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(body);
                    mmMessage.AlternateViews.Add(plainView);
                }

                if (file != null)
                {
                    Attachment attachment = new Attachment(file.InputStream,
                                                           file.ContentType);
                    attachment.ContentDisposition.FileName = Path.GetFileName(file.FileName);
                    mmMessage.Attachments.Add(attachment);
                }


                SmtpClient smtp = new SmtpClient();
                smtp.Port = smtpPort;
                smtp.Host = smtpHost;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = false;
                smtp.Credentials = new NetworkCredential(login,
                                                         password);
                smtp.Send(mmMessage);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error Send Function",
                              ex);
                throw new System.Exception("Smpt - Error Send Function",
                                    ex);
            }
        }
    }
}
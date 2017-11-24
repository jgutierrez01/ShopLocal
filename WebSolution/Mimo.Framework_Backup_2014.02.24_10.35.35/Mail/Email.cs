using System.Threading;
using System.Web;
using log4net;

namespace Mimo.Framework.Mail
{
    public class Email
    {
        /// <summary>
        /// logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Email));

        #region Private class variables

        private int _smtpPort = 25;
        private string _smptHost = string.Empty;
        private string _from = string.Empty;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _subject = string.Empty;
        private string _to = string.Empty;
        private string _body = string.Empty;
        private HttpPostedFile _attachedFile;
        private string[] _toArray;
        private string[] _bccArray;
        private string[] _ccArray;
        private byte[] _pdf;
        private string _xml;


        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SmptHost
        {
            get { return _smptHost; }
            set { _smptHost = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpPostedFile AttachedFile
        {
            get
            {
                return _attachedFile;
            }
            set
            {
                _attachedFile = value;
            }
        }

        public string[] ToArray
        {
            get
            {
                return _toArray;
            }
            set
            {
                _toArray = value;
            }
        }

        public string[] CCArray
        {
            get
            {
                return _ccArray;
            }
            set
            {
                _ccArray = value;
            }
        }

        public string[] BCCArray
        {
            get
            {
                return _bccArray;
            }
            set
            {
                _bccArray = value;
            }
        }

        public byte[] pdf
        {
            get
            {
                return _pdf;
            }
            set
            {
                _pdf = value;
            }
        }

        public string xml
        {
            get
            {
                return _xml;
            }
            set
            {
                _xml = value;
            }
        }
        #endregion

        #region Async send methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendAsync()
        {
            ThreadStart start = new ThreadStart(SendThreadProc);
            Thread thread = new Thread(start);
            thread.Priority = ThreadPriority.Normal;
            thread.Start();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendThreadProc()
        {
            try
            {
                Smtp smtp = new Smtp();

                smtp.Send(_smtpPort,
                           _smptHost,
                          _from,
                          _login,
                          _password,
                          _to,
                          _subject,
                          _body,
                          true);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error while sending email",
                              ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SendWithAttachmentAsync()
        {
            ThreadStart start = new ThreadStart(SendThreadProcWithAttachment);
            Thread thread = new Thread(start);
            thread.Priority = ThreadPriority.Normal;
            thread.Start();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendThreadProcWithAttachment()
        {
            try
            {
                Smtp smtp = new Smtp();

                smtp.Send(_smtpPort,
                    _smptHost,
                    _from,
                          _login,
                          _password,
                          _to,
                          _subject,
                          _body,
                          true,
                          _attachedFile);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error while sending email with attachment",
                              ex);
            }
        }

        #endregion

        public bool Send()
        {
            try
            {
                Smtp smtp = new Smtp();

                smtp.Send(_smtpPort,
                    _smptHost,
                    _from,
                          _login,
                          _password,
                          _to,
                          _subject,
                          _body,
                          true);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error while sending email",
                              ex);
                return false;
            }
        }
    }
}
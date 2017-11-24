namespace Mimo.Framework.Mail
{
    public class MailToken
    {
        private string _from;
        private string _to;
        private string _login;
        private string _password;
        private string _subject;


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

        public MailToken(string from,
                         string to,
                         string login,
                         string password,
                         string subject)
        {
            _from = from;
            _to = to;
            _login = login;
            _password = password;
            _subject = subject;
        }

        public override string ToString()
        {
            return string.Format("from [{0}], to [{1}], login [{2}], password [{3}], subject [{4}].",
                                 _from,
                                 _to,
                                 _login,
                                 _password,
                                 _subject);
        }
    }
}
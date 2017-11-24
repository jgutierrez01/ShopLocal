using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace SAM.Web.Classes
{
    public class MyReportServerCredentials : IReportServerCredentials
    {
        protected string username;
        protected string pwd;
        protected string domain;
        public MyReportServerCredentials(string username, string pwd, string domain)
        {
            this.username = username;
            this.pwd = pwd;
            this.domain = domain;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // Use default identity.
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                return new System.Net.NetworkCredential(username, pwd, domain);
            }
        }

        public bool GetFormsCredentials(out System.Net.Cookie authCookie,
                out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;  // Not use forms credentials to authenticate.
        }
    }


}
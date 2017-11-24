using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Extensions;
using System.Configuration;

namespace Mimo.Framework.Common
{
    public static class Config
    {
        public static int SecurityKeySize
        {
            get
            {
                return ConfigurationManager.AppSettings["Mimo.SecurityKeySize"].SafeIntParse();
            }
        }

        public static byte [] SecurityKey
        {
            get
            {
                return Convert.FromBase64String(ConfigurationManager.AppSettings["Mimo.SecurityKey"]);
            }        
        }

        public static byte [] SecurityIV
        {
            get
            {
                return Convert.FromBase64String(ConfigurationManager.AppSettings["Mimo.SecurityIV"]);
            }
        }

        public static string UsernameReportingServices
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.UsernameReportingServices"];
            }
        }

        public static string SamConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SamDB"].ConnectionString;
            }
        }

        public static string PasswordReportingServices
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.PasswordReportingServices"];
            }
        }

        public static string DomainReportingServices
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.DomainReportingServices"];
            }
        }

        public static string UrlReportingServices
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.UrlReportingServices"];
            }
        }

        public static string UrlReportingServicesWebService
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.UrlReportingServicesWebService"];
            }
        }

        public static string ReportingServicesDefaultFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.ReportingServicesDefaultFolder"];
            }
        }
    }
}

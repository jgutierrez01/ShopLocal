using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using SAM.Entities;
using SAM.Web.Common;
using SAM.Entities.Cache;
using SAM.Web.Shop.Models;
using System.Collections.Generic;
using Mimo.Framework;


namespace SAM.Web.Shop.Utils
{
    public class HttpNavigationContext : INavigationContext
    {
        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }


        private void WriteCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value,
                HttpOnly = true
            };


            Response.SetCookie(cookie);
        }


        private void WriteCookie(string key, NameValueCollection values)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                HttpOnly = true
            };


            foreach (string kvp in values.AllKeys)
            {
                cookie.Values[kvp] = values[kvp];
            }


            Response.SetCookie(cookie);
        }


        private int GetIdFromCookie(string cookieName)
        {
            NameValueCollection nvc = GetCookieValues(cookieName);


            if (nvc != null)
            {
                string value = nvc.Get(0);
                int id;


                if (int.TryParse(value, out id))
                {
                    return id;
                }
            }


            return -1;
        }


        private string GetFromCookie(string cookieName)
        {
            NameValueCollection nvc = GetCookieValues(cookieName);
            string value = string.Empty;
            if (nvc != null)
            {
                value = nvc.Get(0);
            }


            return value;
        }


        private NameValueCollection GetCookieValues(string cookieName)
        {
            HttpCookie cookie;


            //Try from response first
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(cookieName))
            {
                cookie = HttpContext.Current.Response.Cookies[cookieName];


                if (cookie != null)
                {
                    return cookie.Values;
                }
            }


            //Try from request second
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(cookieName))
            {
                cookie = HttpContext.Current.Request.Cookies[cookieName];


                if (cookie != null)
                {
                    return cookie.Values;
                }
            }


            return null;
        }
        private NameValueCollection GetCookieValuesList(string cookieName)
        {
            HttpCookie cookie;


            //Try from response first
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(cookieName))
            {
                cookie = HttpContext.Current.Response.Cookies[cookieName];


                if (cookie != null)
                {
                    return cookie.Values;
                }
            }


            //Try from request second



            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(cookieName))
            {
                cookie = HttpContext.Current.Request.Cookies[cookieName];


                if (cookie != null)
                {
                    return cookie.Values;
                }
            }


            return null;
        }
        public void SetYard(int yardId)
        {
            PatioCache yard = UserScope.MisPatios.SingleOrDefault(y => y.ID == yardId);


            if (yard == null)
            {
                throw new SecurityException(string.Format("The requested yardId {0} is not available for the looged user {1}", yardId, SessionFacade.Username));
            }


            WriteCookie("yardId", yardId.ToString(CultureInfo.CurrentCulture));
        }


        public void SetNumbersControl(string spools)
        {
            if (spools == null)
            {
                throw new SecurityException(string.Format("The requested spools {0} is not available for the looged user {1}", spools, SessionFacade.Username));
            }


            WriteCookie("spools", spools);
        }

        public void SetNumbersControlCuadranteSQ(string numeroControlCuadranteSQ)
        {
            if (numeroControlCuadranteSQ == null)
            {
                throw new SecurityException(string.Format("The requested numberconrol {0} is not available for the looged user {1}", numeroControlCuadranteSQ, SessionFacade.Username));
            }


            WriteCookie("ListaElementos", numeroControlCuadranteSQ);
        }

        public void SetNumbersControlCuadranteSQEditar(string numeroControlCuadranteSQ)
        {
            if (numeroControlCuadranteSQ == null)
            {
                throw new SecurityException(string.Format("The requested numberconrol {0} is not available for the looged user {1}", numeroControlCuadranteSQ, SessionFacade.Username));
            }


            WriteCookie("ListaElementosEditados", numeroControlCuadranteSQ);
        }


        public void SetSQ(string sq)
        {
            WriteCookie("SQActual", sq);
        }

        public void SetProject(int projectId)
        {
            ProyectoCache project = UserScope.MisProyectos.SingleOrDefault(p => p.ID == projectId);


            if (project == null)
            {
                throw new SecurityException(string.Format("The requested projectId {0} is not available for the looged user {1}", projectId, SessionFacade.Username));
            }


            WriteCookie("projectId", projectId.ToString(CultureInfo.CurrentCulture));
        }

        public void SetProjectEdit(int projectId)
        {
            ProyectoCache project = UserScope.MisProyectos.SingleOrDefault(p => p.ID == projectId);


            if (project == null)
            {
                throw new SecurityException(string.Format("The requested projectId {0} is not available for the looged user {1}", projectId, SessionFacade.Username));
            }


            WriteCookie("projectIdEditar", projectId.ToString(CultureInfo.CurrentCulture));
        }




        public void SetControlNumber(OrdenTrabajoSpool controlNumber)
        {
            NameValueCollection nvc = new NameValueCollection
            {
                {"controlNumberId", controlNumber.OrdenTrabajoSpoolID.ToString(CultureInfo.CurrentCulture)},
                {"controlNumber", controlNumber.NumeroControl},
                {"spoolId", controlNumber.SpoolID.ToString(CultureInfo.CurrentCulture)}
            };


            WriteCookie("controlNumber", nvc);
        }


        public PatioCache GetCurrentYard()
        {
            int yardId = GetIdFromCookie("yardId");


            if (yardId <= 0)
            {
                throw new InvalidDataException("The context has not been set with a value for yardId");
            }


            PatioCache yard = UserScope.MisPatios.SingleOrDefault(y => y.ID == yardId);


            if (yard == null)
            {
                throw new SecurityException(string.Format("The requested yardId {0} is not available for the looged user {1}", yardId, SessionFacade.Username));
            }


            return yard;
        }




        public ProyectoCache GetCurrentProject()
        {
            int projectId = GetIdFromCookie("projectId");


            if (projectId < 0)
            {
                throw new InvalidDataException("The context has not been set with a value for projectId");

            }


            ProyectoCache project = UserScope.MisProyectos.SingleOrDefault(y => y.ID == projectId);


            if (project == null)
            {
                throw new SecurityException(string.Format("The requested projectId {0} is not available for the looged user {1}", projectId, SessionFacade.Username));
            }


            return project;
        }

        public ProyectoCache GetCurrentProjectSQ()
        {
            int projectId = GetIdFromCookie("projectId");
            ProyectoCache project = UserScope.MisProyectos.SingleOrDefault(y => y.ID == projectId);
            if (project == null)
            {
                return null;
            }
            return project;
        }

        public ProyectoCache GetCurrentProjectSQEditar()
        {
            int projectId = GetIdFromCookie("projectIdEditar");
            ProyectoCache project = UserScope.MisProyectos.SingleOrDefault(y => y.ID == projectId);
            if (project == null)
            {
                return null;
            }
            return project;
        }


        public ControlNumberCache GetCurrentControlNumber()
        {
            NameValueCollection nvc = GetCookieValues("controlNumber");


            if (nvc != null)
            {
                return new ControlNumberCache(nvc);
            }

            throw new InvalidDataException("The context has not been set with a value for control number");
        }




        public string GetCurrentNumbersControl()
        {


            string spools = GetFromCookie("spools");




            return spools;
        }
        public string GetCurrentNCSQ()
        {
            string spools = GetFromCookie("ListaElementos");
            return spools;
        }

        public string GetCurrentNCSQEditar()
        {
            string spools = GetFromCookie("ListaElementosEditados");
            return spools;
        }

        public string GetSQ()
        {
            string sq = GetFromCookie("SQActual");
            return sq;
        }



    }
}

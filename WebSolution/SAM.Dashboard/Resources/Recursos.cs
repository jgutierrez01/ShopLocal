using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace SAM.Dashboard.Resources
{
    public class Recursos
    {

        public Recursos()
        {
        }

        public Recursos(CultureInfo culture)
        {
            MainPage.Culture = culture;
        }

        public string txtProyecto
        {
            get { return MainPage.txtProyecto_Text; }
        }

        public string txtPatio
        {
            get { return MainPage.txtPatio_Text; }
        }

        public string txtVentana
        {
            get { return MainPage.txtVentana_Text; }
        }

        public string Proyecto
        {
            get { return MainPage.Proyecto_Text; }
        }

        public string Ingenieria
        {
            get { return MainPage.Ingenieria_Text; }
        }

        public string Materiales
        {
            get { return MainPage.Materiales_Text; }
        }

        public string Produccion
        {
            get { return MainPage.Produccion_Text; }
        }
        
        public string Calidad
        {
            get { return MainPage.Calidad_Text; }
        }

        public string Pintura
        {
            get { return MainPage.Pintura_Text; }
        }

        public string Comercial
        {
            get { return MainPage.Comercial_Text; }
        }

        public string Reportes
        {
            get { return MainPage.Reportes_Text; }
        }

        public string Mostrar
        {
            get { return MainPage.btnMostrar_Text; }
        }

        public string GuardarVista
        {
            get { return MainPage.btnGuardar_Text; }
        }

        public string errPatio
        {
            get { return MainPage.errPatio_Text;  }
        }

        public string VistaGuardada
        {
            get
            {
                return MainPage.txtVistaGuardada_Text;
            }
        }

        public string VistaDefault
        {
            get { return MainPage.btnDefault_Text; }
        }
    }
}

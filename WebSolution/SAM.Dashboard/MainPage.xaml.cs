using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SAM.Dashboard.Resources;
using SAM.Web.RIAServices;
using System.ServiceModel.DomainServices.Client;
using SAM.Entities;
using SAM.Dashboard.Utils;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;
using System.Threading;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Resources;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Windows.Browser;

namespace SAM.Dashboard
{
    public partial class MainPage : UserControl
    {



        private DashboardContext _dbContext = new DashboardContext();
        private Usuario _usuario;
        private Recursos _recursos = new Recursos();
        private bool _mainLoad = true;


        public MainPage()
        {
           InitializeComponent();
            DataContext = new Recursos();


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            EntityQuery<Usuario> usuarioQuery = _dbContext.ObtenUsuarioQuery();

            _dbContext.Load(usuarioQuery, (x) =>
            {
                _usuario = x.Entities.FirstOrDefault();

                cargaCombos();


            }, null);

        }

        /// <summary>
        /// Metodo que carga el contenido de los paneles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dock_ElementLoaded(object sender, LayoutSerializationEventArgs e)
        {
            if (e.AffectedElement.GetType().Name == "RadPane")
            {
                //((RadPane)e.AffectedElement).Content = new Control1();
                //((RadPane)e.AffectedElement).UpdateLayout();
            }
        }

        private void Dock_ElementLoading(object sender, LayoutSerializationLoadingEventArgs e)
        {

            switch (e.AffectedElementSerializationTag)
            {
                case "Proyecto":
                    e.SetAffectedElement(MapeaPanel("Proyecto", _recursos.Proyecto));
                    break;
                case "Ingenieria":
                    e.SetAffectedElement(MapeaPanel("Ingenieria", _recursos.Ingenieria));
                    break;
                case "Produccion":
                    e.SetAffectedElement(MapeaPanel("Produccion", _recursos.Produccion));
                    break;
                case "Materiales":
                    e.SetAffectedElement(MapeaPanel("Materiales", _recursos.Materiales));
                    break;
                case "Calidad":
                    e.SetAffectedElement(MapeaPanel("Calidad", _recursos.Calidad));
                    break;
                case "Pintura":
                    e.SetAffectedElement(MapeaPanel("Pintura", _recursos.Pintura));
                    break;
                case "Comercial":
                    e.SetAffectedElement(MapeaPanel("Comercial", _recursos.Comercial));
                    break;
                case "Reportes":
                    e.SetAffectedElement(MapeaPanel("Reportes", _recursos.Reportes));
                    break;
            }

        }

        private RadPane MapeaPanel(string nombre, string recurso)
        {
            RadPane panel = new RadPane();

            panel.SetValue(RadDocking.SerializationTagProperty, nombre);

            panel.Name = nombre;
            panel.Header = recurso;
            panel.Title = recurso;

            return panel;
        }

        /// <summary>
        /// Método que carga la información de los combos.
        /// </summary>
        private void cargaCombos()
        {
            cbPatio.ItemsSource = _dbContext.Patios;

            EntityQuery<Patio> patioQuery = _dbContext.ObtenPatioQuery();
            _dbContext.Load(patioQuery, (x) =>
            {
                EntityQuery<Proyecto> proyectoQuery = _dbContext.ObtenProyectosQuery();
                _dbContext.Load(proyectoQuery, (y) =>
                {
                    if (_dbContext.Proyectos.Count == 1)
                    {

                        Proyecto proy = _dbContext.Proyectos.Single();

                        cbPatio.SelectedValue = proy.PatioID;
                        cargaInformacion();
                        cbVentanas.IsEnabled = true;
                        btnGuardar.IsEnabled = true;
                        btnDefault.IsEnabled = true;
                    }
                }, null);

            }, null);

        }

        /// <summary>
        /// Método que carga el layout del usuario
        /// </summary>
        private void cargaInformacion()
        {
            if (_usuario.VistaDashboard != null)
            {
                XDocument xml = XDocument.Parse(_usuario.VistaDashboard);
                byte[] xmlByte = Encoding.Unicode.GetBytes(xml.ToString());
                MemoryStream isoStream = new MemoryStream(xmlByte);
                mainDock.LoadLayout(isoStream);
                verificaPaneles();
            }
            else
            {
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += HttpsCompleted;
                wc.DownloadStringAsync(new Uri("/ArchivosAuxiliares/Default.xml", UriKind.Relative));
            }


        }

        private void verificaPaneles()
        {

            if (panelVisible("Proyecto", _recursos.Proyecto))
            {
                chkProyecto.IsChecked = true;
            }
            if (panelVisible("Ingenieria", _recursos.Ingenieria))
            {
                chkIngenieria.IsChecked = true;
            }
            if (panelVisible("Materiales", _recursos.Materiales))
            {
                chkMateriales.IsChecked = true;
            }
            if (panelVisible("Produccion", _recursos.Produccion))
            {
                chkProduccion.IsChecked = true;
            }
            if (panelVisible("Calidad", _recursos.Calidad))
            {
                chkCalidad.IsChecked = true;
            }
            if (panelVisible("Pintura", _recursos.Pintura))
            {
                chkPintura.IsChecked = true;
            }
            if (panelVisible("Comercial", _recursos.Comercial))
            {
                chkComercial.IsChecked = true;
            }
            if (panelVisible("Reportes", _recursos.Reportes))
            {
                chkReportes.IsChecked = true;
            }

            _mainLoad = false;
        }

        private bool panelVisible(string nombre, string recurso)
        {
            RadPane panel = null;
            if (mainDock.Panes.Where(p => p.Name == nombre).Count() > 0)
            {
                panel = mainDock.Panes.First(p => p.Name == nombre);
            }
            if (panel != null)
            {
                panel.Header = recurso;
                panel.Title = recurso;
                return !panel.IsHidden;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo que carga el layout default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HttpsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);
                byte[] xmlByte = Encoding.Unicode.GetBytes(xdoc.ToString());
                MemoryStream isoStream = new MemoryStream(xmlByte);
                mainDock.LoadLayout(isoStream);
                verificaPaneles();
            }
        }

        #region Eventos
        private void cbPatio_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbPatio.SelectedValue.SafeIntParse() > 0)
            {
                txtError.Visibility = Visibility.Collapsed;
                imgError.Visibility = Visibility.Collapsed;
            }

            cbProyecto.ItemsSource = _dbContext.Proyectos;
            EntityQuery<Proyecto> proyectoQuery = _dbContext.ObtenProyectosPorPatioQuery(cbPatio.SelectedValue.SafeIntParse());
            _dbContext.Proyectos.Clear();
            _dbContext.Load(proyectoQuery, x =>
            {
                if (cbProyecto.Items.Count == 1)
                {
                    cbProyecto.SelectedIndex = 0;
                }
            }, null
            );

        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cbo = sender as CheckBox;
            string recurso = string.Empty;

            switch (cbo.Tag.ToString())
            {
                case "Proyecto":
                    recurso = _recursos.Proyecto;
                    break;
                case "Ingenieria":
                    recurso = _recursos.Ingenieria;
                    break;
                case "Produccion":
                    recurso = _recursos.Produccion;
                    break;
                case "Materiales":
                    recurso = _recursos.Materiales;
                    break;
                case "Calidad":
                    recurso = _recursos.Calidad;
                    break;
                case "Pintura":
                    recurso = _recursos.Pintura;
                    break;
                case "Comercial":
                    recurso = _recursos.Comercial;
                    break;
                case "Reportes":
                    recurso = _recursos.Reportes;
                    break;
            }

            LoadPanel(cbo.Tag.ToString(), recurso);
        }

        private void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cbo = sender as CheckBox;
            RemovePanel(cbo.Tag.ToString());
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string xml;

            using (MemoryStream isoStream = new MemoryStream())
            {
                mainDock.SaveLayout(isoStream);
                isoStream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(isoStream);
                xml = reader.ReadToEnd();
            }

            ((IEditableObject)_usuario).BeginEdit();

            _usuario.VistaDashboard = xml;
            _usuario.UsuarioModifica = _usuario.UserId;
            _usuario.FechaModificacion = DateTime.Now;
            ((IEditableObject)_usuario).EndEdit();

            _dbContext.SubmitChanges();

            ctrMensaje.iniciaAnimacion();

        }

        private void btnMostrar_Click(object sender, RoutedEventArgs e)
        {
            if (cbPatio.SelectedValue.SafeIntParse() < 1)
            {
                txtError.Visibility = Visibility.Visible;
                imgError.Visibility = Visibility.Visible;
            }
            else
            {
                if (_mainLoad)
                {
                    cargaInformacion();
                    cbVentanas.IsEnabled = true;
                    btnGuardar.IsEnabled = true;
                    btnDefault.IsEnabled = true;
                }
            }
        }

        private void btnVistaDefault_Click(object sender, RoutedEventArgs e)
        {
            ((IEditableObject)_usuario).BeginEdit();

            _usuario.VistaDashboard = null;
            _usuario.UsuarioModifica = _usuario.UserId;
            _usuario.FechaModificacion = DateTime.Now;
            ((IEditableObject)_usuario).EndEdit();

            _dbContext.SubmitChanges(x => {
                HtmlPage.Window.Navigate(new Uri("/Dashboard/DashDefault.aspx", UriKind.Relative));
            },null);

            
        }


        #endregion

        #region Load Ventanas

        /// <summary>
        /// Método que genera un nuevo panel y lo carga en el dock principal
        /// </summary>
        /// <param name="nombre">Nombre del panel</param>
        /// <param name="recurso">Título del header del panel</param>
        private void LoadPanel(string nombre, string recurso)
        {
            if (!_mainLoad)
            {

                RadPane panel = null;
                if (mainDock.Panes.Where(p => p.Name == nombre).Count() > 0)
                {
                    panel = mainDock.Panes.First(p => p.Name == nombre);
                }
                if (panel != null)
                {
                    panel.IsHidden = false;
                    panel.MakeFloatingDockable();
                }
            }
        }

        /// <summary>
        /// Método que remueve el panel del dock principal
        /// </summary>
        /// <param name="nombre">Nombre del panel a remover</param>
        private void RemovePanel(string nombre)
        {
            RadPane panel = null;
            if (mainDock.Panes.Where(p => p.Name == nombre).Count() > 0)
            {
                panel = mainDock.Panes.First(p => p.Name == nombre);
            }
            if (panel != null)
            {
                panel.IsHidden = true;
            }
        }



        #endregion



    }
}

using System;
using System.IO; 
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using SAM.Web.Produccion.App_LocalResources;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Muestra un popup con las alternativas de impresión para una orden de trabajo en particular.
    /// </summary>
    public partial class PopupHistoricoOdt : SamPaginaPopup
    {

        /// <summary>
        /// Número de orden de trabajo
        /// </summary>
        private string NumeroOdt
        {
            get
            {
                return ViewState["NumeroOdt"].ToString();
            }
            set
            {
                ViewState["NumeroOdt"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(EntityID.Value))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando imprimir una ODT {1} para la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }
                //Obtener el número de la ODT y guardar en ViewState
                NumeroOdt = OrdenTrabajoBO.Instance
                                          .Obtener(EntityID.Value)
                                          .NumeroOrden;
                litTitulo.Text = string.Format(MensajesPopupHistoricoOdt.Titulo_HistoricoOdt, NumeroOdt);
            }

            getODTFiles(NumeroOdt); 

        }

        private void getODTFiles(string numeroODT)
        {

            TableRow tableRow;
            TableCell rowCellDate;
            TableCell rowCellVersion;
            TableCell rowCellLink;
            ImageButton printHistoricoODT; 

            DirectoryInfo directoryInfo = new DirectoryInfo(ConfigurationManager.AppSettings["Sam.Produccion.ODTFilesDirectory"]);
            List<FileInfo> listedFiles = Enumerable.ToList(directoryInfo.GetFiles(string.Format("ODT_{0}_*.{1}", numeroODT,"PDF")));

            var selectedFiles = from file in listedFiles orderby file.CreationTime select file;
            foreach (FileInfo file in selectedFiles) 
            {
                string[] splittedFileName = file.Name.ToString().Split('_');
                try
                {
                    tableRow = new TableRow();
                    tableRow.CssClass = "repFila";
                    rowCellDate = new TableCell();
                    rowCellVersion = new TableCell();
                    rowCellLink = new TableCell();
                    printHistoricoODT = new ImageButton();
                    rowCellDate.Text = splittedFileName[3];
                    tableRow.Cells.Add(rowCellDate);
                    rowCellVersion.Text = splittedFileName[2];
                    tableRow.Cells.Add(rowCellVersion);
                    printHistoricoODT.ID = file.Name.ToString();
                    printHistoricoODT.ImageUrl = "~/Imagenes/Iconos/imprimirReporte.png";
                    printHistoricoODT.Click += new ImageClickEventHandler(printHistoricoODT_ImageClick);
                    rowCellLink.Controls.Add(printHistoricoODT);
                    tableRow.Cells.Add(rowCellLink);
                    tblListaArchivos.Rows.Add(tableRow);
                }
                catch (Exception exeption)
                {
                    exeption.Message.ToString(); 
                }
            }
        
        }

        protected void printHistoricoODT_ImageClick(object sender, ImageClickEventArgs e)
        {
            ImageButton printHistoricoODT = (ImageButton)sender;
            Validate();
            if (IsValid)
            {
                FileStream clickedFile = null; 
                try
                {
                    clickedFile = new FileStream(ConfigurationManager.AppSettings["Sam.Produccion.ODTFilesDirectory"] + "\\" + printHistoricoODT.ID.ToString(), FileMode.Open);
                    int fileLength = Convert.ToInt32(clickedFile.Length);
                    Byte[] binaryFile = new Byte[fileLength];
                    clickedFile.Read(binaryFile, 0, fileLength);
                    UtileriasReportes.EnviaReporteComoPdf(this, binaryFile, string.Format("ODT #{0}.pdf", NumeroOdt));
                }
                catch (Exception exeption)
                {
                    exeption.Message.ToString();
                }
                finally
                {
                    clickedFile.Dispose();
                }
            }
        }



        /// <summary>
        /// Generar el PDF compuesto de los distintos reportes seleccionados y enviar ese PDF como el response de la
        /// página para que el browser lo abra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// Validar que se haya mandado imprimir al menos uno de los 5 reportes posibles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
    }
}
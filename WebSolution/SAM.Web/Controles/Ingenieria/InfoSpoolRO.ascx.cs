using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Ingenieria;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;

namespace SAM.Web.Controles.Ingenieria
{
    public partial class InfoSpoolRO : ControlHomologacion
    {
        protected override void Map()
        {
            litRevisionArchivo.Text = RevisionArchivo;
            litRevisionBD.Text = RevisionBD;
            MapeaArchivo(SpoolBD, SpoolEnArchivo);
            MapeaBD(SpoolBD, SpoolEnArchivo);
        }

        protected void MapeaBD(Entities.Spool spool, SpoolPendiente spoolEnArchivo)
        {   
            const string chk = @"<input type=""checkbox"" {0} onclick=""return false"" onkeydown=""return false"" />";
            string cedula = InterpreteDatos.CalculaCedula(spoolEnArchivo);
            
            litDibujoBD.Text = construyeTD(spool.Dibujo, spoolEnArchivo.Dibujo);
            litCedulaBD.Text = construyeTD(spool.Cedula, cedula);
            litNombreBD.Text = construyeTD(spool.Nombre, spoolEnArchivo.Nombre);
            litPdiBD.Text = construyeTD(spool.Pdis.GetValueOrDefault(0).ToString("0.00"),
                                    spoolEnArchivo.Pdis.GetValueOrDefault(0).ToString("0.00"));
            litPesoBD.Text = construyeTD(spool.Peso.GetValueOrDefault(0).ToString("0.00"),
                                    spoolEnArchivo.Peso.GetValueOrDefault(0).ToString("0.#0"));
            litAreaBD.Text = construyeTD((Math.Floor(spool.Area.GetValueOrDefault(0)*100)/100).ToString("0.00"),
                                    (Math.Floor(spoolEnArchivo.Area.GetValueOrDefault(0)*100)/100).ToString("0.00"));
            litEspecificacionBD.Text = construyeTD(spool.Especificacion,spoolEnArchivo.Especificacion);
            litPndBD.Text = construyeTD(spool.PorcentajePnd, spoolEnArchivo.PorcentajePnd);
            litPwhtBD.Text = construyeTD(
                                    string.Format(chk, spool.RequierePwht ? @"checked=""checked""" : string.Empty),
                                    string.Format(chk, spoolEnArchivo.RequierePwht ? @"checked=""checked""" : string.Empty));

            //calculado de juntas
            litDiametroBD.Text = construyeTD(spool.DiametroPlano.GetValueOrDefault().DiameterFormat(), spoolEnArchivo.DiametroPlano.GetValueOrDefault(0).DiameterFormat() );
            litRevCteBD.Text = construyeTD(spool.RevisionCliente, spoolEnArchivo.RevisionCliente);
            litRevStBD.Text = construyeTD(spool.Revision, spoolEnArchivo.Revision);

            
        }

        protected void MapeaArchivo(Entities.Spool spool, SpoolPendiente spoolEnArchivo)
        {
            const string chk = @"<input type=""checkbox"" {0} onclick=""return false"" onkeydown=""return false"" />";
            string cedula = InterpreteDatos.CalculaCedula(spoolEnArchivo);
            litDibujoAR.Text = construyeTD(spoolEnArchivo.Dibujo, spool.Dibujo);
            litCedulaAR.Text = construyeTD(cedula, spool.Cedula);
            litNombreAR.Text = construyeTD(spoolEnArchivo.Nombre, spool.Nombre);
            litPdiAR.Text = construyeTD(spoolEnArchivo.Pdis.GetValueOrDefault(0).ToString("0.00"),
                                    spool.Pdis.GetValueOrDefault(0).ToString("0.00"));
            litPesoAR.Text = construyeTD(spoolEnArchivo.Peso.GetValueOrDefault(0).ToString("0.00"),
                                    spool.Peso.GetValueOrDefault(0).ToString("0.00"));
            litAreaAR.Text = construyeTD((Math.Floor(spoolEnArchivo.Area.SafeDecimalParse()*100)/100).ToString("0.00"),
                                    (Math.Floor(spool.Area.GetValueOrDefault(0)*100)/100).ToString("0.00"));
            litEspecificacionAR.Text = construyeTD(spoolEnArchivo.Especificacion, spool.Especificacion);
            litPndAR.Text = construyeTD(spoolEnArchivo.PorcentajePnd, spool.PorcentajePnd);
            litPwhtAR.Text =
                construyeTD(
                    string.Format(chk, spoolEnArchivo.RequierePwht ? @"checked=""checked""" : string.Empty),
                    string.Format(chk, spool.RequierePwht ? @"checked=""checked""" : string.Empty));
            //calculado de juntas
            litDiametroAR.Text = construyeTD(spoolEnArchivo.DiametroPlano.GetValueOrDefault(0).DiameterFormat(), spool.DiametroPlano.GetValueOrDefault().DiameterFormat());
            litRevCteAR.Text = construyeTD(spoolEnArchivo.RevisionCliente, spool.RevisionCliente);
            litRevStAR.Text = construyeTD(spoolEnArchivo.Revision, spool.Revision);
        }
    }
}
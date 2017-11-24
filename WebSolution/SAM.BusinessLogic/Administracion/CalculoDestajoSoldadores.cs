using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Destajos;
using SAM.Entities;
using System.Xml.Serialization;
using Mimo.Framework.Common;
using System.Xml;
using System.IO;

namespace SAM.BusinessLogic.Administracion
{
    public class CalculoDestajoSoldadores
    {
        /// <summary>
        /// Variable de instancia donde se guarda la lista de soldadura procesada
        /// </summary>
        private List<DestajoSoldaduraGrupo> _lstGrupoSoldadura = new List<DestajoSoldaduraGrupo>();

        /// <summary>
        /// Variable de instancia con el xml serializado que contiene la información del destajo para soldadura
        /// </summary>
        private string _xml = string.Empty;

        /// <summary>
        /// La magia para el cálculo de un destajo ocurre en este método.
        /// Se iteran todos los soldadores encontrados y posteriormente se buscan cuales fueron las juntas que soldaron.
        /// Se hace el cálculo de prorrateos, cuadros y totales en base a las reglas de negocio establecidas.
        /// 
        /// Al finalizar el método se llena la propiedad Xml con el string serializado con el resultado del cálculo.
        /// Este Xml es el que espera el SP para generar un nuevo periodo de destajo.
        /// </summary>
        /// <param name="diasFestivos">Número de días festivos del periodo</param>
        /// <param name="lstSoldadores">Soldadores activos</param>
        /// <param name="lstCostoRaiz">Lista con los costos de procesos raíz</param>
        /// <param name="lstCostoRelleno">Lista con los costos de procesos relleno</param>
        /// <param name="lstConfiguracion">Lista con la configuración del proyecto (tiene la información de valores para cuadros)</param>
        /// <param name="lstSoldadura">Lista de las juntas soldadas que se deben considerar en el destajo</param>
        /// <param name="diametros">Todos los diámetros existentes</param>
        /// <param name="cedulas">Todas las cédulas existentes</param>
        public void CalculaDestajoSoldadores(int diasFestivos, List<Soldador> lstSoldadores, List<CostoProcesoRaiz> lstCostoRaiz, List<CostoProcesoRelleno> lstCostoRelleno, List<ProyectoConfiguracion> lstConfiguracion, List<DestajoSoldaduraDetalle> lstSoldadura, Dictionary<decimal, int> diametros, Dictionary<string, int> cedulas)
        {
            //Variable anónima intencional para dejarle la bronca del hashing a .NET
            var dicCostoRaiz = lstCostoRaiz.ToDictionary(x => new { x.DiametroID, x.TipoJuntaID, x.ProyectoID, x.FamiliaAceroID, x.CedulaID, x.ProcesoRaizID }, y => y.Costo);
            //Variable anónima intencional para dejarle la bronca del hashing a .NET
            var dicCostoRelleno = lstCostoRelleno.ToDictionary(x => new { x.DiametroID, x.TipoJuntaID, x.ProyectoID, x.FamiliaAceroID, x.CedulaID, x.ProcesoRellenoID }, y => y.Costo);

            int idGrupo = -1;
            int idDetalle = -1;
            IEnumerable<DestajoSoldaduraDetalle> lst;
            decimal cuadroRaiz, cuadroRelleno, cuadro, costoUnitarioRaiz, costoUnitarioRelleno;
            bool costoRaizEncontrado = false, costoRellenoEncontrado = false;
            

            foreach (Soldador soldador in lstSoldadores)
            {
                //Obtener sólo las juntas soldadas por esta persona
                lst = lstSoldadura.Where(x => x.SoldadorID == soldador.SoldadorID);

                //Obtener costos de cuadro y relleno
                cuadroRaiz = obtenerValorCuadroRaiz(lstConfiguracion, lst);
                cuadroRelleno = obtenerValorCuadroRelleno(lstConfiguracion, lst);

                //Determinar cual es el cuadro a utilizar
                cuadro = Math.Max(cuadroRaiz, cuadroRelleno);

                #region Generar objeto padre

                DestajoSoldaduraGrupo solGrp = new DestajoSoldaduraGrupo
                {
                    Aprobado = false,
                    CantidadDiasFestivos = diasFestivos,
                    CostoDiaFestivo = cuadro / 5.5m,
                    ReferenciaCuadro = cuadro,
                    SoldadorID = soldador.SoldadorID,
                    TotalAjuste = 0,
                    TotalCuadro = 0,
                    TotalDestajoRaiz = 0,
                    TotalDestajoRelleno = 0,
                    TotalDiasFestivos = cuadro / 5.5m * diasFestivos,
                    TotalOtros = 0,
                    ID = idGrupo
                };

                #endregion

                //iterar por junta workstatusid para luego agrupar raiz y relleno
                IEnumerable<int> lstIds = lst.Select(x => (int)x.JuntaWorkstatusID).Distinct();

                #region Iterar las juntas únicas soldadas por esta persona en particular

                foreach (int jwId in lstIds)
                {
                    #region Generar el objeto de detalle

                    DestajoSoldaduraDetalle sd = new DestajoSoldaduraDetalle
                    {
                        JuntaWorkstatusID = jwId,
                        SoldadorID = soldador.SoldadorID,
                        Ajuste = 0,
                        ProrrateoCuadro = 0,
                        ProrrateoDiasFestivos = 0,
                        ProrrateoOtros = 0,
                        ID = idDetalle--
                    };

                    #endregion

                    //Obtener la junta que se soldó como raíz por esta persona en caso que aplique, solo debe ser una o ninguna
                    var jtaRaiz = lst.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz && x.JuntaWorkstatusID == jwId).SingleOrDefault();

                    if (jtaRaiz != null)
                    {
                        costoRaizEncontrado = false;

                        #region Buscar el costo en el diccionario

                        costoUnitarioRaiz = 0.0m;

                        if (diametros.ContainsKey(jtaRaiz.Diametro) && cedulas.ContainsKey(jtaRaiz.Cedula))
                        {
                            //También es un tipo anónimo para buscar el costo de la junta
                            var llaveRaiz = new
                            {
                                DiametroID = diametros[jtaRaiz.Diametro],
                                TipoJuntaID = jtaRaiz.TipoJuntaID,
                                ProyectoID = jtaRaiz.ProyectoID,
                                FamiliaAceroID = jtaRaiz.FamiliaAceroID,
                                CedulaID = cedulas[jtaRaiz.Cedula],
                                ProcesoRaizID = jtaRaiz.ProcesoRaizID
                            };

                            if (dicCostoRaiz.ContainsKey(llaveRaiz))
                            {
                                costoUnitarioRaiz = dicCostoRaiz[llaveRaiz];
                                costoRaizEncontrado = true;
                            }
                        }

                        #endregion

                        sd.Diametro = jtaRaiz.Diametro;
                        sd.CostoUnitarioRaiz = costoUnitarioRaiz;
                        sd.EsDePeriodoAnterior = jtaRaiz.EsDePeriodoAnterior;
                        //la misma junta soldada por otros y por mi
                        sd.NumeroFondeadores = (byte)lstSoldadura.Count(x => x.JuntaWorkstatusID == jwId && x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz);
                        sd.ProyectoID = jtaRaiz.ProyectoID;
                        sd.RaizDividida = sd.NumeroFondeadores > 1;
                        sd.DestajoRaiz = costoUnitarioRaiz / sd.NumeroFondeadores;
                        sd.CostoRaizVacio = !costoRaizEncontrado;
                    }
                    else
                    {
                        sd.CostoUnitarioRaiz = 0;
                        sd.CostoRaizVacio = false;
                        sd.NumeroFondeadores = 0;
                        sd.RaizDividida = false;
                        sd.DestajoRaiz = 0;
                    }

                    //Obtener la junta que se soldó como relleno por esta persona en caso que aplique, solo debe ser una o ninguna
                    var jtaRelleno = lst.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno && x.JuntaWorkstatusID == jwId).SingleOrDefault();

                    if (jtaRelleno != null)
                    {
                        
                        #region Buscar el costo en el diccionario
                        costoRellenoEncontrado = false;
                        costoUnitarioRelleno = 0;

                        if (diametros.ContainsKey(jtaRelleno.Diametro) && cedulas.ContainsKey(jtaRelleno.Cedula))
                        {
                            //También es un tipo anónimo para buscar el costo de la junta
                            var llaveRelleno = new
                            {
                                DiametroID = diametros[jtaRelleno.Diametro],
                                TipoJuntaID = jtaRelleno.TipoJuntaID,
                                ProyectoID = jtaRelleno.ProyectoID,
                                FamiliaAceroID = jtaRelleno.FamiliaAceroID,
                                CedulaID = cedulas[jtaRelleno.Cedula],
                                ProcesoRellenoID = jtaRelleno.ProcesoRellenoID
                            };

                            if (dicCostoRelleno.ContainsKey(llaveRelleno))
                            {
                                costoUnitarioRelleno = dicCostoRelleno[llaveRelleno];
                                costoRellenoEncontrado = true;
                            }
                        }

                        #endregion

                        sd.Diametro = jtaRelleno.Diametro;
                        sd.CostoUnitarioRelleno = costoUnitarioRelleno;
                        sd.EsDePeriodoAnterior = jtaRelleno.EsDePeriodoAnterior;
                        //La misma junta soldada por otros y por mi
                        sd.NumeroRellenadores = (byte)lstSoldadura.Count(x => x.JuntaWorkstatusID == jwId && x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno);
                        sd.ProyectoID = jtaRelleno.ProyectoID;
                        sd.RellenoDividido = sd.NumeroRellenadores > 1;
                        sd.DestajoRelleno = costoUnitarioRelleno / sd.NumeroRellenadores;
                        sd.CostoRellenoVacio = !costoRellenoEncontrado;
                    }
                    else
                    {
                        sd.CostoUnitarioRelleno = 0;
                        sd.CostoRellenoVacio = false;
                        sd.NumeroRellenadores = 0;
                        sd.RellenoDividido = false;
                        sd.DestajoRelleno = 0;
                    }

                    sd.ProrrateoDiasFestivos = 0;
                    sd.IDPadre = solGrp.ID;
                    solGrp.AgregaDetalle(sd);
                }

                #endregion

                calculaProrrateosYTotalesSoldador(solGrp);
                idGrupo--;

                _lstGrupoSoldadura.Add(solGrp);
            }

            serializa();
        }

        /// <summary>
        /// Resultado de la serialización después de haber calculado el destajo. 
        /// Esta propiedad no regresa nada si no se manda llamara antes alguno de los métodos para calcular el destajo.
        /// </summary>
        public string Xml
        {
            get
            {
                return _xml;
            }
        }

        /// <summary>
        /// Serializa en un XML la lista con el cálculo de destajo para los soldadores.
        /// Se asume que este método se manda llamar ya cuando la lista tiene datos
        /// </summary>
        private void serializa()
        {
            StringWriter sw = new StringWriter();

            using(XmlTextWriter writer = new XmlTextWriter(sw))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DestajoSoldaduraGrupo>));
                serializer.Serialize(writer, _lstGrupoSoldadura);
                writer.Flush();
                writer.Close();
            }

            _xml = sw.ToString();
        }

        /// <summary>
        /// Ya que se le dio la primera vuelta al soldador y sus juntas es necesario volver a iterar las juntas
        /// del destajo como grupo para poder sumar y calcular los prorrateos y totales finales.
        /// 
        /// Este método lleva a cabo esos cálculos y al finalizar la entidad contiene los cálculos correctos.
        /// </summary>
        /// <param name="grp">Entidad con la infórmación del destajo y sus juntas</param>
        private void calculaProrrateosYTotalesSoldador(DestajoSoldaduraGrupo grp)
        {
            decimal diff = 0m;
            decimal totalPulgadas = 1.0m;

            //Sumar el total a pagar por destajo en caso que hayan juntas
            if (grp.Detalle.Count > 0)
            {
                totalPulgadas = grp.Detalle.Sum(x => x.Diametro);
                grp.TotalDestajoRaiz = grp.Detalle.Sum(x => x.DestajoRaiz);
                grp.TotalDestajoRelleno = grp.Detalle.Sum(x => x.DestajoRelleno);
            }

            //Calcular la diferencia entre el destajo y el cuadro sólo en caso que el cuadro sea mayor al destajo
            //Cuando el cuadro es mayor al destajo el soldador no "completa" su salario mínimo con lo soldado
            //por lo cual se le paga por cuadro
            if ( (grp.TotalDestajoRaiz + grp.TotalDestajoRelleno) < grp.ReferenciaCuadro )
            {
                diff = grp.ReferenciaCuadro - (grp.TotalDestajoRaiz + grp.TotalDestajoRelleno);
                grp.TotalCuadro = diff;
            }

            //Calcular el prorrateo del cuadro y recalcular el total de la junta
            grp.Detalle.ForEach(x =>
            {
                x.ProrrateoCuadro = diff * (x.Diametro / totalPulgadas);
                x.ProrrateoDiasFestivos = grp.TotalDiasFestivos * (x.Diametro / totalPulgadas);
                x.Total = x.DestajoRaiz + x.DestajoRelleno + x.ProrrateoDiasFestivos + x.ProrrateoCuadro;
            });

            //Recalcular el total del destajo
            grp.GranTotal = grp.TotalCuadro + grp.TotalDestajoRaiz + grp.TotalDestajoRelleno + grp.TotalDiasFestivos;
        }

        /// <summary>
        /// Regresa cual es el valor ($) del cuadro para el proceso raíz de la junta.  Cada proyecto tiene sus propios valores
        /// de cuadro por lo cual si un soldador soldó para varios proyectos puede existir más de un valor de cuadro
        /// en el caso anterior se debe utilizar el valor mayor.
        /// </summary>
        /// <param name="lstConfiguracion">Lista con la configuración de todos los proyectos de la BD</param>
        /// <param name="lstSoldadura">Lista con las juntas que se van a pagar en este destajo</param>
        /// <returns>Valor del cuadro correspondiente</returns>
        private decimal obtenerValorCuadroRaiz(List<ProyectoConfiguracion> lstConfiguracion, IEnumerable<DestajoSoldaduraDetalle> lstSoldadura)
        {
            decimal? cuadro;

            //Seleccionar solo dentro de los proyectos para los cuales hayan soldado juntas
            cuadro = lstConfiguracion.Where(p => lstSoldadura.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Raiz).Select(a => a.ProyectoID).Contains(p.ProyectoID))
                                     .Select(p => (decimal?)p.CuadroRaiz)
                                     .Max();

            return cuadro ?? 0;
        }

        /// <summary>
        /// Regresa cual es el valor ($) del cuadro para el proceso relleno de la junta.  Cada proyecto tiene sus propios valores
        /// de cuadro por lo cual si un soldador soldó para varios proyectos puede existir más de un valor de cuadro
        /// en el caso anterior se debe utilizar el valor mayor.
        /// </summary>
        /// <param name="lstConfiguracion">Lista con la configuración de todos los proyectos de la BD</param>
        /// <param name="lstSoldadura">Lista con las juntas que se van a pagar en este destajo</param>
        /// <returns>Valor del cuadro correspondiente</returns>
        private decimal obtenerValorCuadroRelleno(List<ProyectoConfiguracion> lstConfiguracion, IEnumerable<DestajoSoldaduraDetalle> lstSoldadura)
        {
            decimal? cuadro;

            cuadro = lstConfiguracion.Where(p => lstSoldadura.Where(x => x.TecnicaSoldadorID == (int)TecnicaSoldadorEnum.Relleno).Select(a => a.ProyectoID).Contains(p.ProyectoID))
                                     .Select(p => (decimal?)p.CuadroRelleno)
                                     .Max();

            return cuadro ?? 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Destajos;
using SAM.Entities;
using Mimo.Framework.Common;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SAM.BusinessLogic.Administracion
{
    public class CalculoDestajoTuberos
    {
        /// <summary>
        /// Variable de instancia donde se guarda la lista de armado procesada
        /// </summary>
        private List<DestajoArmadoGrupo> _lstArmado = new List<DestajoArmadoGrupo>();

        /// <summary>
        /// Variable de instancia con el xml serializado que contiene la información del destajo para armado
        /// </summary>
        private string _xml = string.Empty;

        /// <summary>
        /// La magia para el cálculo de un destajo ocurre en este método.
        /// Se iteran todos los tuberos encontrados y posteriormente se buscan cuales fueron las juntas que armaron.
        /// Se hace el cálculo de prorrateos, cuadros y totales en base a las reglas de negocio establecidas.
        /// 
        /// Al finalizar el método se llena la propiedad Xml con el string serializado con el resultado del cálculo.
        /// Este Xml es el que espera el SP para generar un nuevo periodo de destajo.
        /// </summary>
        /// <param name="diasFestivos">Número de días festivos del periodo</param>
        /// <param name="lstTuberos">Tuberos activos</param>
        /// <param name="lstCostoArmado">Lista con los costos de armado</param>
        /// <param name="lstConfiguracion">Lista con la configuración del proyecto (tiene la información de valores para cuadros)</param>
        /// <param name="lstArmado">Lista de las juntas armadas que se deben considerar en el destajo</param>
        /// <param name="diametros">Todos los diámetros existentes</param>
        /// <param name="cedulas">Todas las cédulas existentes</param>
        public void CalculaDestajoTuberos(int diasFestivos, List<Tubero> lstTuberos, List<CostoArmado> lstCostoArmado, List<ProyectoConfiguracion> lstConfiguracion, List<DestajoArmadoDetalle> lstArmado, Dictionary<decimal, int> diametros, Dictionary<string, int> cedulas)
        {
            //Variable anónima intencional para dejarle la bronca del hashing a .NET
            var dicCostosArmado = lstCostoArmado.ToDictionary(x => new { x.DiametroID, x.TipoJuntaID, x.ProyectoID, x.FamiliaAceroID, x.CedulaID }, y => y.Costo);

            decimal costoUnitarioArmado;
            decimal cuadro;
            IEnumerable<DestajoArmadoDetalle> lst;
            int idGrupo = -1;
            int idDetalle = -1;
            bool costoEncontrado = false;

            //Iterar para cada uno de los tuberos que van a formar parte de este periodo de destajo
            foreach (Tubero tubero in lstTuberos)
            {
                idDetalle = -1;

                //Obtener los registros de armado únicamente del tubero en cuestión
                lst = lstArmado.Where(x => x.TuberoID == tubero.TuberoID);

                //El valor del cuadro para el tubero
                cuadro = obtenerValorCuadroTubero(lstConfiguracion, lst);

                #region Generar el objeto con el registro maestro para el tubero en cuestión

                DestajoArmadoGrupo dt = new DestajoArmadoGrupo
                {
                    Aprobado = false,
                    CantidadDiasFestivos = diasFestivos,
                    CostoDiaFestivo = cuadro / 5.5m, //Inicialmente el costo del día festivo asume que los periodos son de 5.5 días laborales
                    ReferenciaCuadro = cuadro,
                    TotalCuadro = 0,
                    GranTotal = 0,
                    TotalDestajo = 0,
                    TotalDiasFestivos = cuadro / 5.5m * diasFestivos,
                    TotalOtros = 0,
                    TotalAjuste = 0,
                    TuberoID = tubero.TuberoID,
                    ID = idGrupo
                };

                #endregion

                #region Detalle de cada junta

                //Iterar cada junta armada por el tubero
                foreach (DestajoArmadoDetalle dtj in lst)
                {
                    #region Buscar en el diccionario el costo de la junta
                    costoUnitarioArmado = 0.0m;
                    costoEncontrado = false;

                    if (diametros.ContainsKey(dtj.Diametro) && cedulas.ContainsKey(dtj.Cedula))
                    {
                        //También es un tipo anónimo para buscar el costo de la junta
                        var llave = new
                        {
                            DiametroID = diametros[dtj.Diametro],
                            TipoJuntaID = dtj.TipoJuntaID,
                            ProyectoID = dtj.ProyectoID,
                            FamiliaAceroID = dtj.FamiliaAceroID,
                            CedulaID = cedulas[dtj.Cedula]
                        };

                        //Obtener el costo de la junta
                        if (dicCostosArmado.ContainsKey(llave))
                        {
                            costoUnitarioArmado = dicCostosArmado[llave];
                            costoEncontrado = true;
                        }
                    }

                    #endregion

                    #region Calcular propiedades faltantes

                    //Costo unitario y destajo de momento son lo mismo
                    dtj.Ajuste = 0;
                    dtj.CostoUnitario = costoUnitarioArmado;
                    dtj.Destajo = costoUnitarioArmado;
                    dtj.EsDePeriodoAnterior = dtj.EsDePeriodoAnterior;
                    dtj.ProrrateoDiasFestivos = 0;
                    dtj.ProrrateoOtros = 0;
                    dtj.ProrrateoCuadro = 0;
                    dtj.ID = idDetalle--;
                    dtj.IDPadre = idGrupo;
                    dtj.CostoDestajoVacio = !costoEncontrado;
                    dt.AgregaDetalle(dtj);
                    #endregion
                }

                #endregion

                calculaProrrateosYTotalesTubero(dt);

                idGrupo--;

                _lstArmado.Add(dt);
            }

            serializa();
        }

        /// <summary>
        /// Sigue la misma lógica que el método para varios tuberos solo que este lo hace para un solo tubero.
        /// </summary>
        /// <param name="diasFestivos">Número de días festivos del periodo</param>
        /// <param name="tuberoID">ID del tubero para el cual se está calculando el destajo</param>
        /// <param name="lstTuberos">Tuberos activos</param>
        /// <param name="lstCostoArmado">Lista con los costos de armado</param>
        /// <param name="lstConfiguracion">Lista con la configuración del proyecto (tiene la información de valores para cuadros)</param>
        /// <param name="lstArmado">Lista de las juntas armadas que se deben considerar en el destajo</param>
        /// <param name="diametros">Todos los diámetros existentes</param>
        /// <param name="cedulas">Todas las cédulas existentes</param>
        public void CalculaDestajoParaUnTubero(int diasFestivos, int tuberoID, List<CostoArmado> lstCostoArmado, List<ProyectoConfiguracion> lstConfiguracion, List<DestajoArmadoDetalle> lstArmado, Dictionary<decimal, int> diametros, Dictionary<string, int> cedulas)
        {
            //Variable anónima intencional para dejarle la bronca del hashing a .NET
            var dicCostosArmado = lstCostoArmado.ToDictionary(x => new { x.DiametroID, x.TipoJuntaID, x.ProyectoID, x.FamiliaAceroID, x.CedulaID }, y => y.Costo);

            decimal costoUnitarioArmado;
            decimal cuadro;
            int idGrupo = -1;
            int idDetalle = -1;
            bool costoDestajoEncontrado = false;

            //El valor del cuadro para el tubero
            cuadro = obtenerValorCuadroTubero(lstConfiguracion, lstArmado);

            #region Generar el objeto con el registro maestro para el tubero en cuestión

            DestajoArmadoGrupo dt = new DestajoArmadoGrupo
            {
                Aprobado = false,
                CantidadDiasFestivos = diasFestivos,
                CostoDiaFestivo = cuadro / 5.5m,
                ReferenciaCuadro = cuadro,
                TotalCuadro = 0,
                GranTotal = 0,
                TotalDestajo = 0,
                TotalDiasFestivos = cuadro / 5.5m * diasFestivos,
                TotalOtros = 0,
                TotalAjuste = 0,
                TuberoID = tuberoID,
                ID = idGrupo
            };

            #endregion

            #region Detalle de cada junta

            foreach (DestajoArmadoDetalle dtj in lstArmado)
            {
                #region Buscar en el diccionario el costo de la junta
                costoUnitarioArmado = 0.0m;
                costoDestajoEncontrado = false;

                if (diametros.ContainsKey(dtj.Diametro) && cedulas.ContainsKey(dtj.Cedula))
                {
                    //También es un tipo anónimo para buscar el costo de la junta
                    var llave = new
                    {
                        DiametroID = diametros[dtj.Diametro],
                        TipoJuntaID = dtj.TipoJuntaID,
                        ProyectoID = dtj.ProyectoID,
                        FamiliaAceroID = dtj.FamiliaAceroID,
                        CedulaID = cedulas[dtj.Cedula]
                    };

                    //Obtener el costo de la junta
                    if (dicCostosArmado.ContainsKey(llave))
                    {
                        costoUnitarioArmado = dicCostosArmado[llave];
                        costoDestajoEncontrado = true;
                    }
                }

                #endregion

                #region Calcular propiedades faltantes

                //Costo unitario y destajo de momento son lo mismo
                dtj.Ajuste = 0;
                dtj.CostoUnitario = costoUnitarioArmado;
                dtj.Destajo = costoUnitarioArmado;
                dtj.EsDePeriodoAnterior = dtj.EsDePeriodoAnterior;
                dtj.ProrrateoDiasFestivos = 0;
                dtj.ProrrateoOtros = 0;
                dtj.ProrrateoCuadro = 0;
                dtj.ID = idDetalle--;
                dtj.IDPadre = idGrupo;
                dtj.CostoDestajoVacio = !costoDestajoEncontrado;
                dt.AgregaDetalle(dtj);
                #endregion
            }

            #endregion

            calculaProrrateosYTotalesTubero(dt);

            _lstArmado.Add(dt);

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
        /// Serializa en un XML la lista con el cálculo de destajo para los tuberos.
        /// Se asume que este método se manda llamar ya cuando la lista tiene datos
        /// </summary>
        private void serializa()
        {
            StringWriter sw = new StringWriter();

            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<DestajoArmadoGrupo>));
                serializer.Serialize(writer, _lstArmado);
                writer.Flush();
                writer.Close();
            }

            _xml = sw.ToString();
        }

        /// <summary>
        /// Ya que se le dio la primera vuelta al tubero y sus juntas es necesario volver a iterar las juntas
        /// del destajo como grupo para poder sumar y calcular los prorrateos y totales finales.
        /// 
        /// Este método lleva a cabo esos cálculos y al finalizar la entidad contiene los cálculos correctos.
        /// </summary>
        /// <param name="dt">Entidad con la infórmación del destajo y sus juntas</param>
        private void calculaProrrateosYTotalesTubero(DestajoArmadoGrupo dt)
        {
            decimal diff = 0m;
            decimal totalPulgadas = 1.0m;

            //Sumar el total a pagar por destajo en caso que hayan juntas
            if (dt.Detalle.Count > 0)
            {
                totalPulgadas = dt.Detalle.Sum(x => x.Diametro);
                dt.TotalDestajo = dt.Detalle.Sum(x => x.Destajo);
            }

            //Calcular la diferencia entre el destajo y el cuadro sólo en caso que el cuadro sea mayor al destajo
            //Cuando el cuadro es mayor al destajo el tubero no "completa" su salario mínimo con lo armado
            //por lo cual se le paga por cuadro
            if (dt.TotalDestajo < dt.ReferenciaCuadro)
            {
                diff = dt.ReferenciaCuadro - dt.TotalDestajo;
                dt.TotalCuadro = diff;
            }

            //Calcular el prorrateo del cuadro y recalcular el total de la junta
            dt.Detalle.ForEach(x =>
            {
                x.ProrrateoCuadro = diff * (x.Diametro / totalPulgadas);
                x.ProrrateoDiasFestivos = dt.TotalDiasFestivos * (x.Diametro / totalPulgadas);
                x.Total = x.Destajo + x.ProrrateoDiasFestivos + x.ProrrateoCuadro;
            });

            //Recalcular el total del destajo
            dt.GranTotal = dt.TotalCuadro + dt.TotalDestajo + dt.TotalDiasFestivos;
        }

        /// <summary>
        /// Regresa cual es el valor ($) del cuadro para el tubero.  Cada proyecto tiene sus propios valores
        /// de cuadro por lo cual si un tubero armó para varios proyectos puede existir más de un valor de cuadro
        /// en el caso anterior se debe utilizar el valor mayor.
        /// </summary>
        /// <param name="lstConfiguracion">Lista con la configuración de todos los proyectos de la BD</param>
        /// <param name="lstArmado">Lista con las juntas que se van a pagar en este destajo</param>
        /// <returns>Valor del cuadro correspondiente</returns>
        private decimal obtenerValorCuadroTubero(List<ProyectoConfiguracion> lstConfiguracion, IEnumerable<DestajoArmadoDetalle> lstArmado)
        {
            decimal? cuadro;

            //Seleccionar solo dentro de los proyectos para los cuales haya armado juntas
            cuadro = lstConfiguracion.Where(p => lstArmado.Select(a => a.ProyectoID).Contains(p.ProyectoID))
                                     .Select(p => (decimal?)p.CuadroTubero)
                                     .Max();

            return cuadro ?? 0;
        }
    }
}

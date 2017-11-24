using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Cache;
using SAM.Entities;
using Mimo.Framework.Common;
using System.Threading;
using UltimoProceso = SAM.Entities.UltimoProceso;

namespace SAM.BusinessObjects.Utilerias
{
    public static class EntityConverter
    {
        private static string cultura
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        public static AceroCache ToAceroCache(Acero acero)
        {
            return new AceroCache
            {
                ID = acero.AceroID,
                Nombre = acero.Nomenclatura,
                FamiliaAceroID = acero.FamiliaAceroID,
                FamiliaAceroNombre = acero.FamiliaAcero.Nombre,
                FamiliaMaterialNombre = acero.FamiliaAcero.FamiliaMaterial.Nombre,
                VerificadoPorCalidad = acero.VerificadoPorCalidad
            };
        }

        public static DefectoCache ToDefectoCache(Defecto def)
        {
            return new DefectoCache
            {
                ID = def.DefectoID,
                Nombre = (cultura == LanguageHelper.INGLES ? def.NombreIngles : def.Nombre),
                TipoPruebaID = def.TipoPruebaID
            };
        }

        public static FabricanteCache ToFabricanteCache(Fabricante fab)
        {
            return new FabricanteCache
            {
                ID = fab.FabricanteID,
                Nombre = fab.Nombre,
                Telefono = fab.Telefono,
                Direccion = fab.Direccion,
                Descripcion = fab.Descripcion,
                ContactoID = fab.ContactoID,
                Contacto = new ContactoStruct
                {
                    ApPaterno = fab.Contacto.ApPaterno,
                    ApMaterno = fab.Contacto.ApMaterno,
                    CorreoElectronico = fab.Contacto.CorreoElectronico,
                    Nombre = fab.Contacto.Nombre,
                    TelefonoCelular = fab.Contacto.TelefonoCelular,
                    TelefonoOficina = fab.Contacto.TelefonoOficina,
                    TelefonoParticular = fab.Contacto.TelefonoParticular
                }
            };
        }

        public static FamAceroCache ToFamAceroCache(FamiliaAcero fam)
        {
            return new FamAceroCache
            {
                ID = fam.FamiliaAceroID,
                Nombre = fam.Nombre,
                FamiliaMaterialID = fam.FamiliaMaterialID,
                FamiliaMaterialNombre = fam.FamiliaMaterial.Nombre,
                Descripcion = fam.Descripcion
            };
        }

        public static FamMaterialCache ToFamMaterialCache(FamiliaMaterial fam)
        {
            return new FamMaterialCache
            {
                ID = fam.FamiliaMaterialID,
                Nombre = fam.Nombre
            };
        }

        public static PatioCache ToPatioCache(Patio patio)
        {
            return new PatioCache
            {
                ID = patio.PatioID,
                Nombre = patio.Nombre,
                Propietario = patio.Propietario,
                Talleres = (from ta in patio.Taller
                            select new TallerCache
                            {
                                ID = ta.TallerID,
                                Nombre = ta.Nombre
                            }).ToList()
            };
        }

        public static TallerCache ToTallerCache(Taller taller)
        {
            return new TallerCache
            {                
                ID = taller.TallerID,
                Nombre = taller.Nombre//,                 
                /*Talleres = (from ta in taller.Taller
                            select new TallerCache
                            {
                                ID = ta.TallerID,
                                Nombre = ta.Nombre
                            }).ToList()*/
            };
        }

        public static PerfilCache ToPerfilCache(Perfil perfil)
        {
            return new PerfilCache
            {
                ID = perfil.PerfilID,
                Nombre = (cultura == LanguageHelper.INGLES ? perfil.NombreIngles : perfil.Nombre),
                Permisos = (from pe in perfil.PerfilPermiso select pe.PermisoID).ToList()
            };
        }

        public static TipoMaterialCache ToTipoMaterialCache(TipoMaterial tipoMaterial)
        {
            return new TipoMaterialCache
            {
                ID = tipoMaterial.TipoMaterialID,
                Nombre = (cultura == LanguageHelper.INGLES ? tipoMaterial.NombreIngles : tipoMaterial.Nombre),
            };
        }

        public static PermisoCache ToPermisoCache(Permiso permiso)
        {
            return new PermisoCache
            {
                ID = permiso.PermisoID,
                Nombre = (cultura == LanguageHelper.INGLES ? permiso.NombreIngles : permiso.Nombre),
                ModuloID = permiso.ModuloID,
                Paginas = (from p in permiso.Pagina select p.Url).ToList()
            };
        }

        public static ProcesoRaizCache ToProcesoRaizCache(ProcesoRaiz pRaiz)
        {
            return new ProcesoRaizCache
            {
                Codigo = pRaiz.Codigo,
                Nombre = (cultura == LanguageHelper.INGLES ? pRaiz.NombreIngles : pRaiz.Nombre),
                ID = pRaiz.ProcesoRaizID
            };
        }

        public static ProcesoRellenoCache ToProcesoRellenoCache(ProcesoRelleno pRelleno)
        {
            return new ProcesoRellenoCache
            {
                ID = pRelleno.ProcesoRellenoID,
                Codigo = pRelleno.Codigo,
                Nombre = (cultura == LanguageHelper.INGLES ? pRelleno.NombreIngles : pRelleno.Nombre)
            };
        }

        public static ProveedorCache ToProveedorCache(Proveedor prov)
        {
            return new ProveedorCache
            {
                ID = prov.ProveedorID,
                Nombre = prov.Nombre,
                Telefono = prov.Telefono,
                Direccion = prov.Direccion,
                Descripcion = prov.Descripcion,
                ContactoID = prov.ContactoID,
                Contacto = new ContactoStruct
                {
                    ApPaterno = prov.Contacto.ApPaterno,
                    ApMaterno = prov.Contacto.ApMaterno,
                    CorreoElectronico = prov.Contacto.CorreoElectronico,
                    Nombre = prov.Contacto.Nombre,
                    TelefonoCelular = prov.Contacto.TelefonoCelular,
                    TelefonoOficina = prov.Contacto.TelefonoOficina,
                    TelefonoParticular = prov.Contacto.TelefonoParticular
                }
            };        
        }

        public static ProyectoCache ToProyectoCache(Proyecto proy)
        {
            ProyectoCache pc =
            new ProyectoCache
            {
                ClienteID = proy.ClienteID,
                ContactoID = proy.ContactoID,
                ColorID = proy.ColorID,
                Descripcion = proy.Descripcion,
                FechaInicio = proy.FechaInicio,
                ID = proy.ProyectoID,
                Nombre = proy.Nombre,
                PatioID = proy.PatioID,
                PrefijoNumeroUnico = proy.ProyectoConfiguracion.PrefijoNumeroUnico,
                PrefijoOdt = proy.ProyectoConfiguracion.PrefijoOrdenTrabajo,
                DigitosOdt = proy.ProyectoConfiguracion.DigitosOrdenTrabajo,
                NombreCliente = proy.Cliente.Nombre,
                NombreColor = (cultura == LanguageHelper.INGLES ? proy.Color.NombreIngles : proy.Color.Nombre),
                Estatus = TraductorEnumeraciones.TextoActivoInactivo(proy.Activo),
                Activo = proy.Activo,
                HexadecimalColor = proy.Color.CodigoHexadecimal,
                NombrePatio = proy.Patio.Nombre,
                ColumnasNomenclatura = proy.ProyectoNomenclaturaSpool != null ? proy.ProyectoNomenclaturaSpool.CantidadSegmentosSpool : 0,
                Nomenclatura = new List<NomenclaturaStruct>()
            };

            for (int i = 0; i < pc.ColumnasNomenclatura; i++)
            {
                pc.Nomenclatura.Add(new NomenclaturaStruct
                {
                    NombreColumna = ReflectionUtils.GetStringValue(proy.ProyectoNomenclaturaSpool, "SegmentoSpool" + (i + 1)),
                    Orden = i + 1
                });
            }

            return pc;
        }

        public static TransportistaCache ToTransportistaCache(Transportista trans)
        {
            return new TransportistaCache
            {
                ID = trans.TransportistaID,
                Nombre = trans.Nombre,
                Telefono = trans.Telefono,
                Direccion = trans.Direccion,
                Descripcion = trans.Descripcion,
                ContactoID = trans.ContactoID,
                Contacto = new ContactoStruct
                {
                    ApPaterno = trans.Contacto.ApPaterno,
                    ApMaterno = trans.Contacto.ApMaterno,
                    CorreoElectronico = trans.Contacto.CorreoElectronico,
                    Nombre = trans.Contacto.Nombre,
                    TelefonoCelular = trans.Contacto.TelefonoCelular,
                    TelefonoOficina = trans.Contacto.TelefonoOficina,
                    TelefonoParticular = trans.Contacto.TelefonoParticular
                }
            };
        }

        public static WpsCache ToWpsCache(Wps wps)
        {
            return new WpsCache
            {
                EspesorRaizMaximo = wps.EspesorRaizMaximo,
                EspesorRellenoMaximo = wps.EspesorRellenoMaximo,
                FamiliaAcero1 = wps.FamiliaAcero.Nombre,
                FamiliaAcero2 = wps.FamiliaAcero1.Nombre,
                ID = wps.WpsID,
                MaterialBase2ID = wps.FamiliaAcero1.FamiliaAceroID,
                MaterialBaseID = wps.FamiliaAcero.FamiliaAceroID,
                Nombre = wps.Nombre,
                ProcesoRaizID = wps.ProcesoRaizID,
                ProcesoRaizNombre = wps.ProcesoRaiz.Nombre,
                ProcesoRellenoID = wps.ProcesoRellenoID,
                ProcesoRellenoNombre = wps.ProcesoRelleno.Nombre,
                RequierePwht = wps.RequierePwht,
                RequierePreheat = wps.RequierePreheat
            };
        }

        public static ColorCache ToColorCache(Color color)
        {
            return new ColorCache
            {
                ID = color.ColorID,
                Nombre = (cultura == LanguageHelper.INGLES ? color.NombreIngles : color.Nombre),
                CodigoHexadecimal = color.CodigoHexadecimal
            };
        }

        public static TipoJuntaCache ToTipoJuntaCache(TipoJunta tj)
        {
            return new TipoJuntaCache
            {
                ID = tj.TipoJuntaID,
                Nombre = tj.Codigo,
                NombreJunta = tj.Nombre
            };
        }

        public static TipoMovimientoCache ToTipoMovimientoCache(TipoMovimiento tm)
        {
            return new TipoMovimientoCache
            {
                ID = tm.TipoMovimientoID,
                Nombre = (cultura == LanguageHelper.INGLES ? tm.NombreIngles : tm.Nombre),
                EsEntrada = tm.EsEntrada,
                EsTransferenciaProcesos = tm.EsTransferenciaProcesos,
                ApareceEnSaldos = tm.ApareceEnSaldos,
                DisponibleMovimientosUI = tm.DisponibleMovimientosUI
            };
        }

        public static TipoCorteCache ToTipoCorteCache(TipoCorte tc)
        {
            return new TipoCorteCache
            {
                ID = tc.TipoCorteID,
                Nombre = tc.Codigo,
            };
        
        }

        public static TipoPruebaCache ToTipoPruebaCache(TipoPrueba tp)
        {
            return new TipoPruebaCache
            {
                ID = tp.TipoPruebaID,
                Nombre = (cultura == LanguageHelper.INGLES ? tp.NombreIngles : tp.Nombre),
                Categoria = tp.Categoria
            };
        }

        public static TipoPruebaSpoolCache ToTipoPruebaSpoolCache(TipoPruebaSpool tp)
        {
            return new TipoPruebaSpoolCache
            {
                ID = tp.TipoPruebaSpoolID,
                Nombre = (cultura == LanguageHelper.INGLES ? tp.NombreIngles : tp.Nombre),
                Categoria = tp.Categoria
            };
        }

        public static FabAreaCache ToFabAreaCache(FabArea fa)
        {
            return new FabAreaCache
            {
                ID = fa.FabAreaID,
                Nombre = fa.Codigo
            };
        }

        public static ClienteCache ToClienteCache(Cliente cliente)
        {
            return new ClienteCache
            {
                ID = cliente.ClienteID,
                Nombre = cliente.Nombre
            };
        }

       

        public static DiametroCache ToDiametroCache(Diametro diametro)
        {
            return new DiametroCache
                       {
                           ID = diametro.DiametroID,
                           Nombre = diametro.Valor.ToString(),
                           Valor =  diametro.Valor
                       };
        }

        public static KgTeoricoCache ToKgTeoricoCache(KgTeorico kg)
        {
            return new KgTeoricoCache
            {
                ID = kg.KgTeoricoID,
                Nombre = kg.Valor.ToString(),
                Valor = kg.Valor,
                CedulaID = kg.CedulaID,
                DiametroID = kg.DiametroID
            };
        }

        public static CedulaCache ToCedulaCache(Cedula cedula)
        {
            return new CedulaCache
                       {
                           ID = cedula.CedulaID,
                           Nombre = cedula.Codigo
                       };
        }

        public static EspesorCache ToEspesorCache(Espesor espesor)
        {
            return new EspesorCache
                       {
                           ID = espesor.EspesorID,
                           Nombre = espesor.Valor.ToString(),
                           Valor = espesor.Valor,
                           CedulaID = espesor.CedulaID,
                           DiametroID = espesor.DiametroID
                       };
        }

        public static UltimoProcesoCache ToUltimoProcesoCache(UltimoProceso ultimoProceso)
        {
            return new UltimoProcesoCache
                       {
                           ID = ultimoProceso.UltimoProcesoID,
                           Nombre = (cultura == LanguageHelper.INGLES ? ultimoProceso.NombreIngles : ultimoProceso.Nombre)
                       };
        }

        public static MaquinaCache ToMaquinaCache(Maquina maquina)
        {
            return new MaquinaCache
            {
                ID = maquina.MaquinaID,
                Nombre = maquina.Nombre,
                PatioNombre = maquina.Patio.Nombre,
                PatioID = maquina.PatioID
            };
        }

        public static CortadorCache ToCortadorCache(Cortador cortador)
        {
            return new CortadorCache
            {
                ID = cortador.CortadorID,
                Nombre = cortador.Nombre,
                //PatioNombre = cortador.Patio.Nombre,
                PatioID = cortador.PatioID,
                TallerID = cortador.TallerID
            };
        }

        public static DespachadorCache ToDespachadorCache(Despachador despachador)
        {
            return new DespachadorCache
            {
                ID = despachador.DespachadorID,
                Nombre = despachador.Nombre,
                //PatioNombre = cortador.Patio.Nombre,
                PatioID = despachador.PatioID,
                TallerID = despachador.TallerID
            };
        }

        public static TipoReporteDimensionalCache ToTipoReporteDimensionalCache(TipoReporteDimensional tipoReporte)
        {
            return new TipoReporteDimensionalCache
            {
                ID = tipoReporte.TipoReporteDimensionalID,
                Nombre = (cultura == LanguageHelper.INGLES ? tipoReporte.NombreIngles : tipoReporte.Nombre)
            };
        }

        public static ConceptoEstimacionCache ToConceptoEstimacionCache(ConceptoEstimacion concepto)
        {
            return new ConceptoEstimacionCache
            {
                ID = concepto.ConceptoEstimacionID,
                Nombre = (cultura == LanguageHelper.INGLES ? concepto.NombreIngles : concepto.Nombre),
                AplicaParaJunta = concepto.AplicaParaJunta,
                AplicaParaSpool = concepto.AplicaParaSpool
            };
        }

        public static ProyectoReporteCache ToProyectoReporteCache(ProyectoReporte pr)
        {
            return new ProyectoReporteCache
            {
                ID = pr.ProyectoReporteID,
                Nombre = (cultura == LanguageHelper.INGLES ? pr.RutaIngles : pr.RutaEspaniol),
                ProyectoID = pr.ProyectoID,
                TipoReporte = (TipoReporteProyectoEnum)pr.TipoReporteProyectoID
            };
        }

        public static TipoReporteProyectoCache ToTipoReporteProyectoCache(TipoReporteProyecto trp)
        {
            return new TipoReporteProyectoCache
            {
                ID = trp.TipoReporteProyectoID,
                Nombre = (cultura == LanguageHelper.INGLES ? trp.NombreIngles : trp.Nombre)
            };
        }

        public static CampoSeguimientoSpoolCache ToCampoSeguimientoSpool(CampoSeguimientoSpool css)
        {
            return new CampoSeguimientoSpoolCache
            {
                ID = css.CampoSeguimientoSpoolID,
                Nombre = (cultura == LanguageHelper.INGLES ? css.NombreIngles : css.Nombre),
                CssColumnaUI = css.CssColumnaUI,
                CampoSeguimientoSpoolID = css.CampoSeguimientoSpoolID,
                ModuloSeguimientoSpoolID = css.ModuloSeguimientoSpoolID,
                DataFormat = css.DataFormat,
                NombreColumnaSp = css.NombreColumnaSp,
                NombreControlUI = css.NombreControlUI,
                OrdenUI = css.OrdenUI,
                AnchoUI = css.AnchoUI
             };
        }

        public static CampoSeguimientoJuntaCache ToCampoSeguimientoJunta(CampoSeguimientoJunta csj)
        {
            return new CampoSeguimientoJuntaCache
            {
                ID = csj.CampoSeguimientoJuntaID,
                Nombre = (cultura == LanguageHelper.INGLES ? csj.NombreIngles : csj.Nombre),
                CssColumnaUI = csj.CssColumnaUI,
                CampoSeguimientoJuntaID = csj.CampoSeguimientoJuntaID,
                ModuloSeguimientoJuntaID = csj.ModuloSeguimientoJuntaID,
                DataFormat = csj.DataFormat,
                NombreColumnaSp = csj.NombreColumnaSp,
                NombreControlUI = csj.NombreControlUI,
                OrdenUI = csj.OrdenUI,
                AnchoUI = csj.AnchoUI,
                TipoDeDato = csj.TipoDeDato
            };
        }


        public static ModuloSeguimientoSpoolCache ToModuloSeguimientoSpool(ModuloSeguimientoSpool mss)
        {
            return new ModuloSeguimientoSpoolCache
            {
                ID = mss.ModuloSeguimientoSpoolID,
                Nombre = (cultura == LanguageHelper.INGLES ? mss.NombreIngles : mss.Nombre),
                NombreTemplateColumn = mss.NombreTemplateColumn,
                ModuloSeguimientoSpoolID = mss.ModuloSeguimientoSpoolID,
                OrdenUI = mss.OrdenUI
            };
        }


        public static ModuloSeguimientoJuntaCache ToModuloSeguimientoJunta(ModuloSeguimientoJunta msj)
        {
            return new ModuloSeguimientoJuntaCache 
            {
                ID = msj.ModuloSeguimientoJuntaID,
                Nombre = (cultura == LanguageHelper.INGLES ? msj.NombreIngles : msj.Nombre),
                NombreTemplateColumn = msj.NombreTemplateColumn,
                ModuloSeguimientoJuntaID= msj.ModuloSeguimientoJuntaID,
                OrdenUI = msj.OrdenUI
            };
        }

        public static CategoriaPendienteCache ToCategoriaPendiente(CategoriaPendiente cp)
        {
            return new CategoriaPendienteCache
            {
                ID = cp.CategoriaPendienteID,
                Nombre = (cultura == LanguageHelper.INGLES ? cp.NombreIngles : cp.Nombre)
            };
        }

        public static PaginaCache ToPaginaCache(Pagina pagina)
        {
            return new PaginaCache
            {
                ID = pagina.PaginaID,
                Nombre = pagina.Url
            };
        }
    }
}

using System.Linq;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using SAM.Web.Shop.Models;

namespace SAM.Web.Shop.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControlNumberMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spool"></param>
        /// <param name="controlNumber"></param>
        /// <returns></returns>
        public static ControlNumberModel FromSpoolAndControlNumber(DetSpool spool, OrdenTrabajoSpool controlNumber)
        {
            ControlNumberModel model = new ControlNumberModel
            {
                ControlNumber = controlNumber.NumeroControl,
                ControlNumberId = controlNumber.OrdenTrabajoSpoolID,
                Spool = spool.Nombre,
                SpoolId = spool.SpoolID,
                Drawing = spool.Dibujo,
                Materials = 
                   (from material in spool.Materiales
                    select new MaterialModel
                    {
                        DiameterOne = material.Diametro1,
                        DiameterTwo = material.Diametro2,
                        ItemCodeName = material.CodigoItemCode,
                        Label = material.Etiqueta,
                        Quantity = material.Cantidad
                    }).ToArray(),
                Joints = 
                   (from joint in spool.Juntas
                    select new JointModel
                    {
                        Diameter = joint.Diametro,
                        Label = joint.Etiqueta,
                        Location = joint.Localizacion,
                        FabAreaName = joint.FabArea,
                        TypeName = joint.TipoJunta
                    }).ToArray(),
                Cuts = 
                   (from cut in spool.Cortes
                    select new CutModel
                    {
                        Diameter = cut.Diametro,
                        ItemCodeName = cut.CodigoItemCode,
                        Length = cut.Cantidad,
                        Segment = cut.EtiquetaSegmento,
                        Label = cut.EtiquetaMaterial
                    }).ToArray()
            };

            return model;
        }
    }
}
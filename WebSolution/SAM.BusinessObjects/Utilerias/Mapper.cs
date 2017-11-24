using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Personalizadas;
using SAM.Entities;
using SAM.Entities.Cache;

namespace SAM.BusinessObjects.Utilerias
{
    public static class Mapper
    {
        public static DetSpool MapearDesdeSpool(Spool sp)
        {
            CacheCatalogos instance = CacheCatalogos.Instance;

            Dictionary<int, string> tiposJunta = instance.ObtenerTiposJunta().ToDictionary(x=> x.ID, y => y.Nombre);
            Dictionary<int, string> tiposCorte = instance.ObtenerTiposCorte().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> fabAreas = instance.ObtenerFabAreas().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> familiasAcero = instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            return new DetSpool(sp, tiposJunta, tiposCorte, fabAreas, familiasAcero);
        }

        public static DetSpoolHold MapearDesdeSpoolHolds(Spool sp)
        {
            return new DetSpoolHold(sp);
        }

        public static DetSpoolHistorico MapearDesdeSpoolHistorico(SpoolHistorico sp)
        {
            CacheCatalogos instance = CacheCatalogos.Instance;

            Dictionary<int, string> tiposJunta = instance.ObtenerTiposJunta().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> tiposCorte = instance.ObtenerTiposCorte().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> fabAreas = instance.ObtenerFabAreas().ToDictionary(x => x.ID, y => y.Nombre);
            Dictionary<int, string> familiasAcero = instance.ObtenerFamiliasAcero().ToDictionary(x => x.ID, y => y.Nombre);

            return new DetSpoolHistorico(sp, tiposJunta, tiposCorte, fabAreas, familiasAcero);
        }
    }
}

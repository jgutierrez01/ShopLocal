using SAM.Entities.Busqueda;


namespace SAM.Web.Shop.Models
{
    public class SearchSpoolResultsModel
    {
        public string Spool { get; set; }
        public PagedResult<NumeroControlBusqueda> Results { get; set; }
    }
}

using System.Collections.Specialized;

namespace SAM.Web.Shop.Models
{
    public class ControlNumberCache
    {
        public ControlNumberCache() { }

        public ControlNumberCache(NameValueCollection nvc)
        {
            ControlNumberId = int.Parse(nvc.Get("controlNumberId"));
            ControlNumber = nvc.Get("controlNumber");
            SpoolId = int.Parse(nvc.Get("spoolId"));
        }

        public int ControlNumberId { get; set; }
        public int SpoolId { get; set; }
        public string ControlNumber { get; set; }
    }
}
[assembly: WebActivator.PreApplicationStartMethod(typeof(SAM.Web.Shop.App_Start.Combres), "PreStart")]
namespace SAM.Web.Shop.App_Start {
	using System.Web.Routing;
	using global::Combres;
	
    public static class Combres {
        public static void PreStart() {
            RouteTable.Routes.AddCombresRoute("Combres");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.Agente
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            EliminarArchivos.Eliminar();
        }
    }
}

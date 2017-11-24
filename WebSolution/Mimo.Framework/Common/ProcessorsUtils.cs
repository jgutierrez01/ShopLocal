using System.Linq;
using System.Management;

namespace Mimo.Framework.Common
{
    public static class ProcessorsUtils
    {
        private static readonly int _cores;
        static ProcessorsUtils()
        {
            _cores = new ManagementObjectSearcher("Select * from Win32_Processor").Get().Cast<ManagementBaseObject>().Sum(
                    item => int.Parse(item["NumberOfCores"].ToString()));
        }

        /// <summary>
        /// Regresa el numero de nucleos o cores que posee la maquina
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfCoreProcessors()
        {
            return _cores;
        }
    }
}

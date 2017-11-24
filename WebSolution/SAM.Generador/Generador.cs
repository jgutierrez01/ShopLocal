using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.Generador
{
    public class Generador
    {
        static void Main(string[] args)
        {
            int opcion = 0;
            Console.WriteLine("Generador masivo de datos");
            Console.WriteLine();

            while (opcion != 50)
            {
                Console.WriteLine("Introduzca una opción");
                Console.WriteLine("---------------------");
                Console.WriteLine("1. Generar odts");
                Console.WriteLine("2. Generar cortes");
                Console.WriteLine("3. Generar despachos");
                Console.WriteLine("4. Generar armados");
                Console.WriteLine("5. Generar soldaduras");
                Console.WriteLine("6. Generar inspecciones visuales");
                Console.WriteLine("7. Generar liberaciones dimensionales");
                Console.WriteLine("8. Requisiciones de pruebas");
                Console.WriteLine("9. Pruebas Pnd");
                Console.WriteLine("10. Pruebas TT");
                Console.WriteLine("11. Requistar Pintura");
                Console.WriteLine("12. Pintar");
                Console.WriteLine("50. Salir");
                Console.WriteLine("---------------------");

                Console.WriteLine();

                KeyboardUtils.ImprimeInline("Seleccione una opción: ");
                opcion = KeyboardUtils.LeeEntero();

                EjecutaSeleccionado(opcion);

                Console.Clear();
            }
        }

        private static void EjecutaSeleccionado(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    new GeneradorOdts().Inicia();
                    break;
                case 2:
                    new GeneradorCortes().Inicia();
                    break;
                case 3:
                    new GeneradorDespachos().Inicia();
                    break;
                case 4:
                    new GeneradorArmado().Inicia();
                    break;
                case 5:
                    new GeneradorSoldadura().Inicia();
                    break;
                case 6:
                    new GeneradorInspeccionVisual().Inicia();
                    break;
                case 7:
                    new GeneradorLiberacionDimensional().Inicia();
                    break;
                case 8:
                    new GeneradorRequisicionesDePruebas().Inicia();
                    break;
                case 9:
                    new GeneradorPruebasPdn().Inicia();
                    break;
                case 10:
                    new GeneradorPruebasTT().Inicia();
                    break;
                case 11:
                    new GeneradorRequisicionesPintura().Inicia();
                    break;
                case 12:
                    new GeneradorPintura().Inicia();
                    break;
                case 50:
                    Console.WriteLine();
                    Console.WriteLine("GRACIAS....");
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Opción inválida");
                    Console.ReadLine();
                    break;
            }
        }
    }
}


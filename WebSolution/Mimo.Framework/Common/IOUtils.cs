using System.Collections.Generic;
using System.IO;

namespace Mimo.Framework.Common
{
    public static class IOUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetBytesFromBinaryStream(Stream s, int length)
        {
            byte [] result = new byte[length];
            int read = 0;

            using (BinaryReader reader = new BinaryReader(s))
            {
                do
                {
                    read += reader.Read(result, read, length - read);
                }
                while (read < length);
            }
           
            return result;
        }

        public static Stream GetStreamFromBytes(byte [] content)
        {
            MemoryStream ms = new MemoryStream(content);
            return ms;
        }


        public static string[] GetLinesFromTextStream(Stream s)
        {
            string line = null;
            string[] lines = null;
            List<string> lstLine = new List<string>();

            using ( TextReader sr = new StreamReader(s, System.Text.Encoding.Default) )
            {
                line = sr.ReadLine();

                while ( line != null )
                {
                    lstLine.Add(line);
                    line = sr.ReadLine();
                }

                lines = lstLine.ToArray();
                sr.Close();
            }

            return lines;
        }

        /// <summary>
        /// Creea una copia del stream original y regresa un clone con posicion 0
        /// </summary>
        /// <param name="source">El stream que va ser copiado</param>
        /// <returns>la copia del stream original</returns>
        public static Stream CloneStreamAndReturnToStart(Stream source, int length)
        {
            Stream clone = new MemoryStream();
            CloneStream(source, clone, length);
            clone.Seek(0, SeekOrigin.Begin);
            return clone;
        }

        /// <summary>
        /// Copia el contenido del stream a otro. Note que el stream de salida estara a la izquierda la ultima 
        /// posicion y por lo que se requiere que se haga un seek antes de usarlo.
        /// </summary>
        /// <param name="input">El stream a copiar</param>
        /// <param name="output">El stream de salida</param>
        private static void CloneStream(Stream input, Stream output, int length)
        {
            byte[] bytes = new byte[length];
            int numBytes;
            while ((numBytes = input.Read(bytes, 0, length)) > 0)
            {
                output.Write(bytes, 0, numBytes);
            }
        }

    }
}

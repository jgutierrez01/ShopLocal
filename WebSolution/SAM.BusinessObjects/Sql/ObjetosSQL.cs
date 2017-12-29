using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SAM.BusinessObjects.Sql
{
    public class ObjetosSQL
    {
        /// <summary>
        /// Cadena Coneccion a la BD
        /// </summary>
        /// <returns></returns>
        protected SqlConnection Conexion()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString);
        }
       
       
        /// <summary>
        /// Retorna un coleccion de Tablas con la información de BD
        /// </summary>
        /// <param name="Stord">Nombre del Stord a ejecutar</param>
        /// <param name="TablaSube">objeto DataTable que envía al stord</param>
        /// <param name="NombreTabla">nombre del parametro de tabla</param>
        /// <param name="Parametros">Parametros que requiere el stord</param>
        /// <returns>Coleccion de tablas</returns>
        public DataSet Coleccion(string Stord, DataTable TablaSube, String NombreTabla, string[,] Parametros = null)
        {
            DataSet ds = new DataSet();
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                if (Parametros != null)
                    for (int i = Numeros.CERO; i < Parametros.Length / Numeros.DOS; i++)
                        cmd.Parameters.AddWithValue(Parametros[i, Numeros.CERO].ToString(), Parametros[i, Numeros.UNO].ToString());
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Connection.Open();
                        da.Fill(ds);
                        cmd.Connection.Close();
                    }
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
                return ds;
            }


        }
       
        /// <summary>
        /// Ejecuta un stord en la BD
        /// </summary>
        /// <param name="Stord">Nombre del Stord a ejecutar</param>
        /// <param name="TablaSube">objeto DataTable que envía al stord</param>
        /// <param name="NombreTabla">nombre del parametro de tabla</param>
        /// <param name="Parametros">Parametros que requiere el stord</param>
        /// <returns>Objeto DatatTable con la coleccion de datos</returns>
        public int Ejecuta(string Stord, DataTable TablaSube, String NombreTabla, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                
                cmd.Parameters.AddWithValue("@Usuario", Parametros[0, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@Inspector", Parametros[1, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@ProyectoID", Parametros[2, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@SQBuscar", Parametros[3, Numeros.UNO].ToString());
                cmd.Parameters.Add("@UltimoSQInternoEncontrado", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    int contractID = Convert.ToInt32(cmd.Parameters["@UltimoSQInternoEncontrado"].Value);
                    cmd.Connection.Close();
                    return contractID;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }

        public string EjecutaRetornaString(string Stord, DataTable TablaSube, String NombreTabla, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {

                cmd.Parameters.AddWithValue("@Usuario", Parametros[0, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@Inspector", Parametros[1, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@ProyectoID", Parametros[2, Numeros.UNO].ToString());
                cmd.Parameters.AddWithValue("@SQBuscar", Parametros[3, Numeros.UNO].ToString());
                cmd.Parameters.Add("@UltimoSQInternoEncontrado", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    string contractID = cmd.Parameters["@UltimoSQInternoEncontrado"].Value.ToString();
                    cmd.Connection.Close();
                    return contractID;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }

        public int EjecutaInsertUpdate(string Stord, DataTable TablaSube, String NombreTabla, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                int lastRow = 0;
                if (Parametros != null)
                    for (int i = Numeros.CERO; i < Parametros.Length / Numeros.DOS; i++)
                        cmd.Parameters.AddWithValue(Parametros[i, Numeros.CERO].ToString(), Parametros[i, Numeros.UNO].ToString());
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    lastRow = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Connection.Close();
                    return lastRow;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }

        public int EjecutaInsertUpdate(string Stord, DataTable TablaSube, String NombreTabla, DataTable TablaSube2, String NombreTabla2, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                int lastRow = 0;
                if (Parametros != null)
                    for (int i = Numeros.CERO; i < Parametros.Length / Numeros.DOS; i++)
                        cmd.Parameters.AddWithValue(Parametros[i, Numeros.CERO].ToString(), Parametros[i, Numeros.UNO].ToString());
                cmd.Parameters.Add(new SqlParameter(NombreTabla, TablaSube));
                cmd.Parameters.Add(new SqlParameter(NombreTabla2, TablaSube2));
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    lastRow = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Connection.Close();
                    return lastRow;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
        public int EjecutaInsertUpdate(string Stord, string[,] Parametros = null)
        {
            using (SqlCommand cmd = new SqlCommand(Stord, Conexion()))
            {
                int lastRow = 0;
                if (Parametros != null)
                    for (int i = Numeros.CERO; i < Parametros.Length / Numeros.DOS; i++)
                        cmd.Parameters.AddWithValue(Parametros[i, Numeros.CERO].ToString(), Parametros[i, Numeros.UNO].ToString());                
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    lastRow = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return lastRow;
                }
                catch (Exception e)
                {
                    cmd.Connection.Close();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
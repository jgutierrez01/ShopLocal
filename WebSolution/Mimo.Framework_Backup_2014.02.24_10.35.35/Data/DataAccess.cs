using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace Mimo.Framework.Data
{
    /// <summary>
    /// Summary description for DataAccess.
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command.
        /// </summary>
        /// <param name="command">the IdbCommand to be prepared</param>
        /// <param name="connection">a valid IDbConnection, on which to execute this command</param>
        /// <param name="transaction">a valid IDbTransaction, or 'null'</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of IDbDataParameters to be associated with the command or 'null' if no parameters are required</param>
        private static void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            //if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //associate the connection with the command
            command.Connection = connection;

            //set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            //if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //set the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }

        private static void AttachParameters(IDbCommand command, IDataParameter[] commandParameters)
        {
            foreach (IDbDataParameter p in commandParameters)
            {
                //check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        public static DataSet ExecuteDataSet(   IDbConnection connection, 
                                                CommandType commandType, 
                                                string commandText, 
                                                DataSet dataSet, 
                                                params IDbDataParameter [] parameters)
        {
            return ExecuteDataset(  connection,
                                    null, 
                                    commandType, 
                                    commandText, 
                                    dataSet, 
                                    (string[]) null,
                                    parameters);
        }

        public static DataSet ExecuteDataset(   IDbConnection connection,
                                                CommandType commandType,
                                                string commandText,
                                                DataSet dataSet,
                                                string dataTableName,
                                                params IDbDataParameter[] parameters)
        {
            return ExecuteDataset(  connection, 
                                    null, 
                                    commandType, 
                                    commandText, 
                                    dataSet, 
                                    (dataTableName != null ? new[] { dataTableName } : null), 
                                    parameters);
        }

        public static DataSet ExecuteDataset(   IDbConnection connection,
                                                CommandType commandType,
                                                string commandText,
                                                DataSet dataSet,
                                                string[] dataTableNames,
                                                params IDbDataParameter[] parameters)
        {
            return ExecuteDataset(  connection, 
                                    null, 
                                    commandType, 
                                    commandText, 
                                    dataSet,
                                    dataTableNames, 
                                    parameters);
        }

        public static DataSet ExecuteDataset(   IDbConnection connection,
                                                IDbTransaction transaction,
                                                CommandType commandType,
                                                string commandText,
                                                DataSet dataSet,
                                                string dataTableName,
                                                params IDbDataParameter[] parameters)
        {
            return ExecuteDataset(  connection, 
                                    transaction, 
                                    commandType, 
                                    commandText, 
                                    dataSet,
                                    (dataTableName != null ? new[] { dataTableName } : null), 
                                    parameters);
        }


        /// <summary>
        /// This method receives as parameter the name of the instruction to execute (could be a stored proc 
        /// or a dynamic query, the name of the stronly typed dataset, the name of the 
        /// table to fill, the connection, transaction and the command parameters
        /// </summary>

        public static DataSet ExecuteDataset(IDbConnection connection,
                                             IDbTransaction transaction,
                                             CommandType commandType,
                                             string commandText,
                                             DataSet dataSet,
                                             string[] dataTableNames,
                                             params IDbDataParameter[] parameters)
        {
            //create a command and prepare it for execution
            IDbCommand command = DataAccessFactory.CreateCommand();

            //open the connection if required
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //link the command and the connection
            command.Connection = connection;

            //assign transaction
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //assign command text
            command.CommandText = commandText;

            //assign the command type
            command.CommandType = commandType;

            //assign connection's time out to command
            command.CommandTimeout = connection.ConnectionTimeout;



            //attach the command parameters if they are provided
            if (parameters != null)
            {
                assignParameters(command, parameters);
            }

            //create the DataAdapter 
            IDbDataAdapter dataAdapter = DataAccessFactory.CreateDataAdapter();
            dataAdapter.SelectCommand = command;

            // create the dataset according to the type requested by the user
            //DataSet dataSet = (DataSet) Assembly.GetExecutingAssembly().CreateInstance(ConfigurationManager.AppSettings["BusinessEntityPath"] + dataSetName);

            // Define the table mapping for the current request

            if (dataTableNames != null && dataTableNames.Length > 0)
            {

                dataAdapter.TableMappings.Add("Table", dataTableNames[0]);

                for (int i = 1; i < dataTableNames.Length; i++)
                {
                    dataAdapter.TableMappings.Add("Table" + i, dataTableNames[i]);
                }
            }

            //fill the DataSet
            dataAdapter.Fill(dataSet);

            // detach the SqlParameters from the command object, so they can be used again.			
            command.Parameters.Clear();

            //return the dataset
            return dataSet;
        }


       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionKey"></param>
        /// <param name="table"></param>
        /// <param name="commandText"></param>
        /// <param name="transaction"></param>
        public static void ExecuteNonQuery( IDbConnection connection,
                                            string connectionKey,
                                            DataTable table,
                                            string commandText,
                                            IDbTransaction transaction)
        {

            // Check if the current database provider contains the return extra parameter or not.
            // by default, it is considered by that the provider contains the return extra parameter
            bool containsReturnParameter = !(ConfigurationManager.AppSettings["ContainsReturnParameter"] != null &&
                                             ConfigurationManager.AppSettings["ContainsReturnParameter"].ToUpper() == "FALSE");

            IDbDataAdapter objAdapter = DataAccessFactory.CreateDataAdapter();

            if (table.Rows.Count != 0)
            {

                objAdapter.InsertCommand = DataAccessFactory.CreateCommand();
                objAdapter.UpdateCommand = DataAccessFactory.CreateCommand();

                // Prepare command
                PrepareCommand(objAdapter.InsertCommand, connection, transaction, CommandType.StoredProcedure, commandText, null);
                PrepareCommand(objAdapter.UpdateCommand, connection, transaction, CommandType.StoredProcedure, commandText, null);

                IDbDataParameter[] parametersInsert = GetSpParameterSet(connectionKey, commandText, !containsReturnParameter);

                foreach (IDbDataParameter parameterInsert in parametersInsert)
                {

                    // NOtify parameter that values will be obtained 
                    // from the given datacolumn

                    // Remove @ from parameter name
                    parameterInsert.SourceColumn = parameterInsert.ParameterName.Remove(0, 1);

                    // Assign parameter to command
                    objAdapter.InsertCommand.Parameters.Add(parameterInsert);
                }

                // Build parameters based on the field attributes 

                IDbDataParameter[] parametersUpdate = GetSpParameterSet(connectionKey, commandText, !containsReturnParameter);

                foreach (IDbDataParameter parameterUpdate in parametersUpdate)
                {

                    // NOtify parameter that values will be obtained 
                    // from the given datacolumn

                    // Remove @ from parameter name
                    parameterUpdate.SourceColumn = parameterUpdate.ParameterName.Remove(0, 1);

                    // Assign parameter to commands
                    objAdapter.UpdateCommand.Parameters.Add(parameterUpdate);
                }

                // Update information
                ((DbDataAdapter)objAdapter).Update(table);
            }
        }

        /// <summary>
        /// This method executes the given insert command
        /// on all the rows where rowstate = added
        /// </summary>
        public static void ExecuteInsert(IDbConnection connection,
                                         DataTable table,
                                         CommandType commandType,
                                         string commandText,
                                         IDbTransaction transaction,
                                         params IDbDataParameter[] parameters)
        {

            IDbDataAdapter objAdapter = DataAccessFactory.CreateDataAdapter();

            if (table.Rows.Count != 0)
            {

                objAdapter.InsertCommand = DataAccessFactory.CreateCommand();

                // Prepare command
                PrepareCommand(objAdapter.InsertCommand, connection, transaction, commandType, commandText, parameters);

                // Set UpdateRowSource and Register RowUpdatedEventHandler
                objAdapter.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;

                ((SqlDataAdapter)objAdapter).RowUpdated += myHandler;

                // Update information
                ((DbDataAdapter)objAdapter).Update(table);

            }
        }

        public static void myHandler(object adapter, SqlRowUpdatedEventArgs e)
        {
            // Don't call AcceptChanges
            e.Status = UpdateStatus.SkipCurrentRow;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectionKey"></param>
        /// <param name="table"></param>
        /// <param name="saveCommandText"></param>
        /// <param name="deleteCommandText"></param>
        /// <param name="transaction"></param>
        public static void ExecuteNonQuery( IDbConnection connection,
                                            string connectionKey,
                                            DataTable table,
                                            string saveCommandText,
                                            string deleteCommandText,
                                            IDbTransaction transaction)
        {
            IDbDataAdapter objAdapter = DataAccessFactory.CreateDataAdapter();

            if (table.Rows.Count != 0)
            {

                // Create new command
                objAdapter.InsertCommand = DataAccessFactory.CreateCommand();
                objAdapter.UpdateCommand = DataAccessFactory.CreateCommand();
                objAdapter.DeleteCommand = DataAccessFactory.CreateCommand();

                // Prepare command
                PrepareCommand(objAdapter.InsertCommand, connection, transaction, CommandType.StoredProcedure, saveCommandText, null);
                PrepareCommand(objAdapter.UpdateCommand, connection, transaction, CommandType.StoredProcedure, saveCommandText, null);
                PrepareCommand(objAdapter.DeleteCommand, connection, transaction, CommandType.StoredProcedure, deleteCommandText, null);

                // Build parameters based on the field attributes 

                IDbDataParameter[] parametersInsert = GetSpParameterSet(connectionKey, saveCommandText, true);

                foreach (IDbDataParameter parameterInsert in parametersInsert)
                {

                    // NOtify parameter that values will be obtained 
                    // from the given datacolumn

                    // Remove @ from parameter name
                    parameterInsert.SourceColumn = parameterInsert.ParameterName.Remove(0, 1);

                    // Assign parameter to command
                    objAdapter.InsertCommand.Parameters.Add(parameterInsert);
                }

                // Build parameters based on the field attributes 

                IDbDataParameter[] parametersUpdate = GetSpParameterSet(connectionKey, saveCommandText, true);

                foreach (IDbDataParameter parameterUpdate in parametersUpdate)
                {

                    // NOtify parameter that values will be obtained 
                    // from the given datacolumn

                    // Remove @ from parameter name
                    parameterUpdate.SourceColumn = parameterUpdate.ParameterName.Remove(0, 1);

                    // Assign parameter to commands
                    objAdapter.UpdateCommand.Parameters.Add(parameterUpdate);
                }

                // Build parameters based on the field attributes 

                IDbDataParameter[] parametersDelete = GetSpParameterSet(connectionKey, deleteCommandText, true);

                foreach (IDbDataParameter parameterDelete in parametersDelete)
                {

                    // NOtify parameter that values will be obtained 
                    // from the given datacolumn

                    // Remove @ from parameter name
                    parameterDelete.SourceColumn = parameterDelete.ParameterName.Remove(0, 1);

                    // Assign parameter to commands
                    objAdapter.DeleteCommand.Parameters.Add(parameterDelete);
                }

                // Update information
                ((DbDataAdapter)objAdapter).Update(table);
            }
        }


        /// <summary>
        /// This method receives as parameter the name of the instruction to execute (could be a stored proc 
        /// or a dynamic query, the connection, transaction and the command parameters
        /// </summary>
        public static void ExecuteNonQuery(IDbConnection connection,
                                           CommandType commandType,
                                           string commandText,
                                           IDbTransaction transaction,
                                           params IDbDataParameter[] parameters)
        {

            IDbCommand command = DataAccessFactory.CreateCommand();

            //open the connection if required
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //link the command and the connection
            command.Connection = connection;

            //assign command text
            command.CommandText = commandText;

            //assign connection's time out to command
            command.CommandTimeout = connection.ConnectionTimeout;

            //if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //assign the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (parameters != null)
            {
                assignParameters(command, parameters);
            }

            command.ExecuteNonQuery();

        }


        private static void assignParameters(IDbCommand command, IDbDataParameter[] parameters)
        {
            foreach (IDbDataParameter parameter in parameters)
            {
                // convert null values to db nulls
                if ((parameter.Direction != ParameterDirection.ReturnValue) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }

                command.Parameters.Add(parameter);
            }
        }

        public static object ExecuteScalar(IDbConnection connection,
                                           IDbTransaction transaction,
                                           CommandType commandType,
                                           string commandText,
                                           params IDbDataParameter[] parameters)
        {
            object result;

            //create a command and prepare it for execution
            IDbCommand command = DataAccessFactory.CreateCommand();

            //open the connection if required
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            //link the command and the connection
            command.Connection = connection;

            //assign command text
            command.CommandText = commandText;

            //assign connection's time out to command
            command.CommandTimeout = connection.ConnectionTimeout;

            //link the command to the transaction
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            //assign the command type
            command.CommandType = commandType;

            //attach the command parameters if they are provided
            if (parameters != null)
            {
                assignParameters(command, parameters);
            }

            result = command.ExecuteScalar();

            // detach the SqlParameters from the command object, so they can be used again.			
            command.Parameters.Clear();

            //return the dataset
            return result;
        }


        public static object ExecuteScalar(IDbConnection connection,
                                           CommandType commandType,
                                           string commandText,
                                           params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(connection, null, commandType, commandText, parameters);
        }



        #region -------------------- ParameterCache -----------------------------

        public static IDbDataParameter[] GetSpParameterSet(string connectionKey, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionKey + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            IDbDataParameter[] cachedParameters;

            cachedParameters = (IDbDataParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters = (IDbDataParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionKey, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        public static IDbDataParameter[] GetSpParameterSet(string connectionKey, string spName)
        {
            // Check if the current database provider contains the return extra parameter or not.
            // by default, it is considered by that the provider contains the return extra parameter
            bool containsReturnParameter = !(ConfigurationManager.AppSettings["ContainsReturnParameter"] != null &&
                                             ConfigurationManager.AppSettings["ContainsReturnParameter"].ToUpper() == "FALSE");

            return GetSpParameterSet(connectionKey, spName, !containsReturnParameter);
        }


        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelperParameterCache()".

        private static readonly Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connectionKey">a valid connection key name for a SqlConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
        /// <returns></returns>
        private static IDbDataParameter[] DiscoverSpParameterSet(string connectionKey, string spName, bool includeReturnValueParameter)
        {
            using (IDbConnection cn = DataAccessFactory.CreateConnection(connectionKey))
            {
                using (IDbCommand cmd = DataAccessFactory.CreateCommand())
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = cn;
                    cmd.CommandText = spName;

                    object commandBuilder = DataAccessFactory.CreateCommandBuilder();

                    try
                    {
                        commandBuilder.GetType().InvokeMember("DeriveParameters",
                                                              BindingFlags.Public | BindingFlags.InvokeMethod |
                                                              BindingFlags.Static, null, null, new object[] { cmd });
                    }catch (Exception ex)
                    {
                        string error = ex.InnerException.Message;
                    }
                    //SqlCommandBuilder.DeriveParameters(cmd);

                    if (!includeReturnValueParameter)
                    {
                        cmd.Parameters.RemoveAt(0);
                    }

                    IDbDataParameter[] discoveredParameters = new IDbDataParameter[cmd.Parameters.Count];

                    cmd.Parameters.CopyTo(discoveredParameters, 0);

                    return discoveredParameters;
                }
            }
        }

        //deep copy of cached SqlParameter array
        private static IDbDataParameter[] CloneParameters(IDbDataParameter[] originalParameters)
        {
            IDbDataParameter[] clonedParameters = new IDbDataParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (IDbDataParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }


        #endregion
    }
}
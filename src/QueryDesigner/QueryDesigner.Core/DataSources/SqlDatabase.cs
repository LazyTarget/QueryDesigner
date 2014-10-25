using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace QueryDesigner.Core
{
    public delegate void DatabaseError(object sender, Exception errorInfo);

    
    public class SqlDatabase : IDataSource<SqlQuery>
    {
        public SqlDatabase(string connString)
            : this(connString, false)
        {

        }


        public SqlDatabase(string connString, bool connect)
        {
            ConnectionString = connString;
            AutoDisconnect = true;
            EnableStatistics = false;
            dbLock = new object();

            if (connect)
            {
                Connect();
            }
        }


        private SqlConnection dbConnection = null;
        private object dbLock;


        //public event DatabaseError OnDatabaseError = null;



        public bool AutoDisconnect { get; set; }

        public bool EnableStatistics { get; set; }

        public string ConnectionString { get; set; }



        public ConnectionState State
        {
            get
            {
                if (dbConnection != null)
                    return dbConnection.State;
                return ConnectionState.Closed;
            }
        }


        public IDictionary GetStatistics()
        {
            if (EnableStatistics)
                if (dbConnection != null)
                    return dbConnection.RetrieveStatistics();
            return null;
        }



        /// <summary>
        /// Establish a connection to the database
        /// </summary>
        public bool Connect()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                return true;
            }

            dbConnection = new SqlConnection(ConnectionString);
            dbConnection.StatisticsEnabled = EnableStatistics;
            dbConnection.Open();
            return true;
        }

        /// <summary>
        /// Establish a connection to the database
        /// </summary>
        public bool TryConnect()
        {
            try
            {
                var res = Connect();
                return res;
            }
            catch (SqlException sqlex)
            {
                return false;
            }
        }


        /// <summary>
        /// Disconnects the connection to the database if connected
        /// </summary>
        public void Disconnect()
        {
            lock (dbLock)
            {
                if (dbConnection != null)
                {
                    try
                    {
                        dbConnection.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        dbConnection.Dispose();
                        dbConnection = null;
                    }
                }
            }
        }


        public void Dispose()
        {
            Disconnect();
        }



        /// <summary>
        /// Extracts the name of the table in the sql string.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        private string ExtractTableName(string sql)
        {
            if (sql.StartsWith("insert into", StringComparison.InvariantCultureIgnoreCase))
            {
                int stopPos = sql.IndexOf("(", StringComparison.OrdinalIgnoreCase);
                if (stopPos > 0)
                {
                    const int start = 11;
                    return sql.Substring(start, (stopPos - start)).Trim(' ', ';');
                }
            }
            else if (sql.StartsWith("update", StringComparison.InvariantCultureIgnoreCase))
            {
                int stopPos = sql.IndexOf("set", StringComparison.OrdinalIgnoreCase);
                if (stopPos > 0)
                {
                    const int start = 6;
                    return sql.Substring(start, (stopPos - start)).Trim(' ', ';');
                }
            }
            else if (sql.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
            {
                int start = sql.IndexOf("from", StringComparison.OrdinalIgnoreCase) + 4; // add length for from word
                if (start > 4)
                {
                    int stopPos = start;
                    // Walk through in the sql string from the "from" keyword
                    for (int i = start; i < sql.Length; i++)
                    {
                        if (sql[i] != ' ') // Not a space, this is the start of the table name
                        {
                            start = i;
                            if (sql[i] != '[') // Not a spaced table name
                            {
                                stopPos = sql.IndexOf(" ", start + 1, StringComparison.Ordinal);
                                if (stopPos > start)
                                {
                                    // We have a hit
                                    return sql.Substring(start, (stopPos - start)).Trim();
                                }

                                if (stopPos == -1) // End of string will give -1
                                {
                                    return sql.Substring(start).Trim(' ', ';');
                                }
                            }
                            else
                            {
                                // Look for next occurance of a ']' token
                                stopPos = sql.IndexOf("]", start, StringComparison.Ordinal);
                                if (stopPos > start)
                                {
                                    // We have a hit
                                    return sql.Substring(start, (stopPos - start)).Trim(' ', ';');
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }






        /// <summary>
        /// Used to get data from the database
        /// </summary>
        /// <param name="query">The DB-query</param>
        /// <returns>A System.Data.Dataset</returns>
        public DataSet GetDataset(string query, params DbParameter[] sqlParams)
        {
            string tableName = ExtractTableName(query);
            var ds = GetDataset(query, (tableName != "" ? tableName : "DefaultDataMember"), sqlParams);
            return ds;
        }


        /// <summary>
        /// Used to get data from the database
        /// </summary>
        /// <param name="query">The DB-query</param>
        /// <param name="dataMember">Name of the datamember</param>
        /// <returns>A System.Data.Dataset</returns>
        public DataSet GetDataset(string query, string dataMember, params DbParameter[] sqlParams)
        {
            try
            {
                DataSet ds = null;
                lock (dbLock)
                {
                    if (!Connect())
                    {
                        return null;
                    }

                    ds = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, this.dbConnection))
                    {
                        adapter.SelectCommand.Parameters.AddRange(sqlParams);
                        adapter.Fill(ds, dataMember);
                    }
                }
                return ds;
            }
            catch (SqlException sqlex)
            {
                //if (OnDatabaseError != null)
                //    OnDatabaseError(this, sqlex);
                //return null;
                throw;
            }
            finally
            {
                if (this.AutoDisconnect)
                    Disconnect();
            }
        }



        public IQueryResult Execute(SqlQuery query)
        {
            if (query == null)
                return null;
            
            var sql = query.Sql;
            var parameters = query.Parameters.ToArray();

            var dataSet = GetDataset(sql, parameters);
            var tables = DataTable.FromDataSet(dataSet);

            var res = new QueryResult(query, tables);
            return res;
        }

    }
}

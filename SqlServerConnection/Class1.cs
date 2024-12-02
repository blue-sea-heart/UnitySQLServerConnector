using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SqlServerConnection
{
    public class SqlConnectionClass
    {
        private string _server;
        private string _port;
        private string _database;
        private string _uid;
        private string _pwd;
        private string _connectionString;

        public string ErrorMessage { get; private set; }
        
        public SqlConnectionClass(string server, string port, string database, string uid, string pwd)
        {
            //初始化连接字符串
            _server = server;
            _port = port;
            _database = database;
            _uid = uid;
            _pwd = pwd;
            _connectionString = $"Server={_server},{_port};Database={_database};Uid={_uid};Pwd={_pwd};";
            
            ErrorMessage = string.Empty;
        }

        private SqlConnection OpenConnection()
        {
            //创建连接
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            //创建连接,异步
            SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public void CloseConnection(SqlConnection connection)
        {
            //关闭连接
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string,string> paras)
        {
            ErrorMessage = string.Empty;
            DataTable dataTable = new DataTable();
            try
            {
                var parameters = CreateSqlParameters(paras);

                await using (SqlConnection connection = await OpenConnectionAsync())
                {
                    await using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage=$"Query failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage=$"Query failed: Unknown error - {ex.Message}";
            }

            return dataTable;
        }

        private SqlParameter[] CreateSqlParameters(Dictionary<string, string> paras)
        {
            List<SqlParameter> ilistStr = new List<SqlParameter>();
                
            foreach (var para in paras)
            {
                SqlParameter temp=new SqlParameter();
                temp.ParameterName = para.Key;
                temp.Value = para.Value;
                ilistStr.Add(temp);
            }
            SqlParameter[] parameters=ilistStr.ToArray();
            return parameters;
        }

        public async Task<string> ExecuteNonQueryAsync(string query, Dictionary<string,string> paras )
        {
            ErrorMessage = string.Empty;
            try
            {
                var parameters = CreateSqlParameters(paras);

                await using (SqlConnection connection = await OpenConnectionAsync())
                {
                    await using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int affectedRows = await command.ExecuteNonQueryAsync();
                        return $"Execution successful, affected rows: {affectedRows}";
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = $"Execution failed: {ex.Message}";
                return $"Execution failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Execution failed: Unknown error - {ex.Message}";
                return $"Execution failed: Unknown error - {ex.Message}";
            }
        }
        public DataTable ExecuteQuery(string query,out string errorMessage ,Dictionary<string,string> paras)
        {
            DataTable dataTable = new DataTable();
            errorMessage = string.Empty;
            try
            {
                var parameters = CreateSqlParameters(paras);

                SqlConnection connection = OpenConnection();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                CloseConnection(connection);
            }
            catch (SqlException ex)
            {
                errorMessage = $"Query failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                errorMessage = $"Query failed: Unknown error - {ex.Message}";
            }
            return dataTable;
        }
        public string ExecuteNonQuery(string query, Dictionary<string,string> paras)
        {
            try
            {
                var parameters = CreateSqlParameters(paras);

                SqlConnection connection = OpenConnection();
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    int affectedRows = command.ExecuteNonQuery();
                    CloseConnection(connection);
                    return $"Execution successful, affected rows: {affectedRows}";
                }
            }
            catch (SqlException ex)
            {
                return $"Execution failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Execution failed: Unknown error - {ex.Message}";
            }
        }
    }
}
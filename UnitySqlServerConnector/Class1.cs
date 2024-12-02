using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace UnitySqlServerConnector
{
    public class SqlConnectionClass
    {
        
        private string _server;
        private string _port;
        private string _database;
        private string _uid;
        private string _pwd;
        private string _connectionString;// 连接字符串

        public string ErrorMessage { get; private set; }// 错误信息
        
        // 初始化连接字符串
        public SqlConnectionClass(string connectionString)
        {
            _connectionString = connectionString;
            ErrorMessage = string.Empty;
        }
        
        // 使用提供的参数构造连接字符串
        public SqlConnectionClass(string server, string port, string database, string uid, string pwd)
        {
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
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            catch (SqlException ex)
            {
                ErrorMessage = $"连接失败：{ex.Message}";
                throw;
            }
        }

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return connection;
            }
            catch (SqlException ex)
            {
                ErrorMessage = $"连接失败：{ex.Message}";
                throw;
            }
        }

        public void CloseConnection(SqlConnection connection)
        {
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

                using (SqlConnection connection = await OpenConnectionAsync())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
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
                ErrorMessage = $"查询失败：{ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"查询失败：未知错误 - {ex.Message}";
            }
            
            return dataTable;
        }

        private SqlParameter[] CreateSqlParameters(Dictionary<string, string> paras)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
                
            foreach (var para in paras)
            {
                SqlParameter temp = new SqlParameter
                {
                    ParameterName = para.Key,
                    Value = para.Value
                };
                parameterList.Add(temp);
            }
            return parameterList.ToArray();
        }

        public async Task<string> ExecuteNonQueryAsync(string query, Dictionary<string,string> paras)
        {
            ErrorMessage = string.Empty;
            try
            {
                var parameters = CreateSqlParameters(paras);

                using (SqlConnection connection = await OpenConnectionAsync())
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int affectedRows = await command.ExecuteNonQueryAsync();
                        return $"执行成功，影响行数：{affectedRows}";
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = $"执行失败：{ex.Message}";
                return $"执行失败：{ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"执行失败：未知错误 - {ex.Message}";
                return $"执行失败：未知错误 - {ex.Message}";
            }
        }

        public DataTable ExecuteQuery(string query, out string errorMessage, Dictionary<string,string> paras)
        {
            DataTable dataTable = new DataTable();
            errorMessage = string.Empty;
            try
            {
                SqlParameter[] parameters = null;

                if (paras != null)
                {
                    parameters = CreateSqlParameters(paras);    
                }

                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            catch (SqlException ex)
            {
                errorMessage = $"查询失败：{ex.Message}";
            }
            catch (Exception ex)
            {
                errorMessage = $"查询失败：未知错误 - {ex.Message}";
            }
            return dataTable;
        }

        public string ExecuteNonQuery(string query, Dictionary<string,string> paras)
        {
            try
            {
                var parameters = CreateSqlParameters(paras);

                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    int affectedRows = command.ExecuteNonQuery();
                    return $"执行成功，影响行数：{affectedRows}";
                }
            }
            catch (SqlException ex)
            {
                return $"执行失败：{ex.Message}";
            }
            catch (Exception ex)
            {
                return $"执行失败：未知错误 - {ex.Message}";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace UnitySqlServerConnector
{
    public class SqlConnectionClass
    {
        private readonly string _connectionString;//连接字符串
        public string ErrorMessage { get; private set; }//错误信息

        /// <summary>
        /// 初始化 SqlConnectionClass 的新实例。
        /// </summary>
        /// <param name="connectionString">SQL Server 数据库的连接字符串。</param>
        /// <exception cref="ArgumentNullException">当 connectionString 为 null 或空时抛出。</exception>
        public SqlConnectionClass(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))//检查连接字符串
            {
                throw new ArgumentNullException(nameof(connectionString), "连接字符串不能为 null 或空。");//如果连接字符串为空,抛出异常
            }
            _connectionString = connectionString;//保存连接字符串
        }

        /// <summary>
        /// 异步打开与 SQL Server 的连接。
        /// </summary>
        /// <returns>一个打开的 SqlConnection。</returns>
        private async Task<SqlConnection> OpenConnectionAsync()//异步打开连接
        {
            try
            {
                var connection = new SqlConnection(_connectionString);//创建连接
                await connection.OpenAsync();//打开连接
                return connection;//返回连接
            }
            catch (SqlException ex)//如果连接失败
            {
                ErrorMessage = $"连接失败：{ex.Message}";//保存错误信息
                throw new SqlConnectionException("打开连接失败", ex);//抛出异常
            }
        }

        /// <summary>
        /// 异步执行查询并将结果返回为 DataTable。
        /// </summary>
        /// <param name="query">要执行的 SQL 查询。</param>
        /// <param name="parameters">SQL 查询的参数。</param>
        /// <returns>包含查询结果的 DataTable。</returns>
        public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object> parameters = null)//异步执行查询
        {
            ErrorMessage = string.Empty;//清空错误信息
            var dataTable = new DataTable();//创建数据表

            try
            {
                using (var connection = await OpenConnectionAsync())//打开连接
                using (var command = new SqlCommand(query, connection))//创建命令
                {
                    if (parameters != null)//如果有参数
                    {
                        foreach (var param in parameters)//遍历参数
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);//添加参数,如果值为空对象，设置为 DBNull
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))//创建适配器
                    {
                        await Task.Run(() => adapter.Fill(dataTable));//异步填充数据表
                    }
                }
            }
            catch (SqlException ex)//sql异常
            {
                ErrorMessage = $"查询失败：{ex.Message}";
                throw new SqlQueryException("查询执行失败", ex);
            }
            catch (Exception ex)//其他异常
            {
                ErrorMessage = $"查询失败：未知错误 - {ex.Message}";
                throw new SqlQueryException("查询执行期间发生未知错误", ex);
            }

            return dataTable;//返回数据表
        }

        /// <summary>
        /// 异步执行非查询 SQL 命令。
        /// </summary>
        /// <param name="query">要执行的 SQL 命令。</param>
        /// <param name="parameters">SQL 命令的参数。</param>
        /// <returns>受影响的行数。</returns>
        public async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters = null)//异步执行非查询
        {
            ErrorMessage = string.Empty;//清空错误信息

            try
            {
                using (var connection = await OpenConnectionAsync())
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    return await command.ExecuteNonQueryAsync();//返回受影响的行数
                }
            }
            catch (SqlException ex)//sql异常
            {
                ErrorMessage = $"执行失败：{ex.Message}";
                throw new SqlExecutionException("非查询执行失败", ex);
            }
            catch (Exception ex)//其他异常
            {
                ErrorMessage = $"执行失败：未知错误 - {ex.Message}";
                throw new SqlExecutionException("非查询执行期间发生未知错误", ex);
            }
        }
    }

    /// <summary>
    /// 表示 SQL 连接过程中发生的异常。
    /// </summary>
    public class SqlConnectionException : Exception
    {
        public SqlConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 表示 SQL 查询执行过程中发生的异常。
    /// </summary>
    public class SqlQueryException : Exception
    {
        public SqlQueryException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// 表示 SQL 非查询执行过程中发生的异常。
    /// </summary>
    public class SqlExecutionException : Exception
    {
        public SqlExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
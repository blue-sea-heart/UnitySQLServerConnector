# SqlServerConnection

SqlServerConnection 是一个用于 C# 项目的 SQL Server 数据库连接库。它提供了一个简单而强大的接口，用于执行 SQL 查询和非查询操作。

## 主要特性

- 异步数据库操作支持
- 参数化查询，防止 SQL 注入
- 错误处理和日志记录
- 支持事务操作

## 使用方法

### 初始化连接

```csharp
string connectionString = "Your connection string here";
var sqlConnection = new SqlConnectionClass(connectionString);
执行查询
csharp
CopyInsert
string query = "SELECT * FROM Users WHERE Age > @Age";
var parameters = new Dictionary<string, string>
{
    { "@Age", "18" }
};

DataTable result = await sqlConnection.ExecuteQueryAsync(query, parameters);
执行非查询操作
csharp
CopyInsert
string command = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
var parameters = new Dictionary<string, string>
{
    { "@Name", "John Doe" },
    { "@Age", "25" }
};

string result = await sqlConnection.ExecuteNonQueryAsync(command, parameters);
类说明
SqlConnectionClass
主要的连接类，包含以下方法：

OpenConnection(): 打开数据库连接
OpenConnectionAsync(): 异步打开数据库连接
CloseConnection(): 关闭数据库连接
ExecuteQueryAsync(): 异步执行查询操作
ExecuteNonQueryAsync(): 异步执行非查询操作
ExecuteQuery(): 执行查询操作
ExecuteNonQuery(): 执行非查询操作
注意事项
请确保在使用前正确设置连接字符串
建议使用异步方法以提高性能
使用参数化查询来防止 SQL 注入攻击
贡献
欢迎提交 Issues 和 Pull Requests 来帮助改进这个项目！

许可证
MIT License

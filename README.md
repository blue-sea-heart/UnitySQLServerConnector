# UnitySQLServerConnector

功能特性
简单易用：通过构造函数直接初始化连接或传入连接字符串。
错误处理：内置异常捕获机制，确保任何数据库操作失败时能够获取到具体的错误信息。
异步支持：提供异步方法OpenConnectionAsync, ExecuteQueryAsync, 和 ExecuteNonQueryAsync，以提高应用程序性能。
参数化查询：支持带有参数的SQL查询，有助于防止SQL注入攻击。
使用示例
初始化：

// 使用连接字符串初始化
var db = new SqlConnectionClass("your_connection_string");

// 或者使用服务器、端口等详细信息初始化
var db = new SqlConnectionClass("server", "port", "database", "username", "password");

执行查询：

try
{
    var result = await db.ExecuteQueryAsync("SELECT * FROM YourTable WHERE Column1=@Value1", new Dictionary<string, string>{{"@Value1", "some_value"}});
    // 处理结果
}
catch (Exception ex)
{
    Console.WriteLine(db.ErrorMessage);
}

执行非查询：

string outcome = await db.ExecuteNonQueryAsync("INSERT INTO YourTable (Column1) VALUES (@Value1)", new Dictionary<string, string>{{"@Value1", "some_value"}});
Console.WriteLine(outcome); // 输出影响行数或错误消息
注意事项
确保你的Unity项目已经配置好.NET Standard 2.0或更高版本的支持。
在生产环境中，请务必安全地存储和处理数据库凭据。
对于复杂的查询或大量数据处理，考虑使用更高级的数据访问技术或ORM框架。

Prerequisites and Setup
To ensure smooth operation of the UnitySQLServerConnector, certain SQL Server settings need to be configured correctly. Here are the steps to set up your SQL Server:

Enable TCP/IP Protocol:

Open SQL Server Configuration Manager.
Navigate to SQL Server Network Configuration -> Protocols for [Your SQL Server Instance].
Right-click on TCP/IP and select Enable.
Set Authentication Mode:

Open SQL Server Management Studio (SSMS).
Right-click on your server instance in Object Explorer and select Properties.
Go to the Security page.
Under Server authentication, select SQL Server and Windows Authentication mode.
Click OK and restart your SQL Server service.
Configure 'sa' Login:

In SSMS, expand Security -> Logins.
Right-click on sa and select Properties.
Under the General page, set a strong password.
Under the Status page, ensure Login is set to Enabled.
Allow Remote Connections:

In SSMS, right-click on your server instance and select Properties.
Go to the Connections page.
Check Allow remote connections to this server.
Click OK and ensure your firewall settings allow SQL Server connections.
Firewall Settings:

Ensure that the TCP port you are using (default is 1433) is open in the firewall.
You might need to create an inbound rule in the Windows Firewall settings to allow connections on this port.
Contributing
Contributions are welcome! Please fork this repository and submit a pull request for any features, bug fixes, or enhancements.

License
This project is licensed under the MIT License.

# UnitySQLServerConnector

## Overview
UnitySQLServerConnector is a Unity plugin designed to simplify the process of connecting to and interacting with SQL Server databases from within your Unity projects. It provides both synchronous and asynchronous methods for executing queries and non-query commands.

## Features
- Seamless integration with SQL Server databases.
- Support for both synchronous and asynchronous database operations.
- Error handling and reporting.
- Easy-to-use API for executing SQL commands and queries.

## Installation

### 1. Download and Import the Plugin
1. Clone or download the repository:
   ```sh
   git clone https://github.com/your-username/UnitySQLServerConnector.git
Copy the SqlServerConnection.dll file from the Assets/Plugins folder into your Unity project's Assets/Plugins folder.
2. Add Example Script
Copy the SQLServerSample.cs script from the Assets/Scripts folder into your Unity project's Assets/Scripts folder.

Configuration
Open the plugin settings in Unity.
Enter your SQL Server connection details (server address, port, database name, username, and password).
Usage Examples
Unity Script Example
Here is an example Unity script demonstrating how to use the UnitySQLServerConnector plugin:



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
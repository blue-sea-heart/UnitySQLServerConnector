using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using SqlServerConnection;

public class SQLServerSample : MonoBehaviour
{
    [SerializeField] private string server;
    [SerializeField] private string port;
    [SerializeField] private string database;
    [SerializeField] private string uid;
    [SerializeField] private string pwd;
    [SerializeField] private string selectQuery;
    [SerializeField] private string nonQuery;
    [SerializeField] private string field;
    // Start is called before the first frame update
    async void Start()
    {
        SqlConnectionClass sqlConnectionClass = new SqlConnectionClass(server, port, database, uid, pwd);

        // Example parameters
        Dictionary<string, string> selectParams = new Dictionary<string, string>
        {
            { "@param1", "value1" },
            // Add more parameters as needed
        };

        // Execute select query with parameters
        DataTable dataTable = await sqlConnectionClass.ExecuteQueryAsync(selectQuery, selectParams);
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                Debug.Log(row[field].ToString());
            }
        }
        else
        {
            Debug.LogError("No data returned or query failed.");
        }

        Dictionary<string, string> nonQueryParams = new Dictionary<string, string>
        {
            { "@param1", "value1" },
            // Add more parameters as needed
        };

        // Execute non-query with parameters
        string nonQueryResult = await sqlConnectionClass.ExecuteNonQueryAsync(nonQuery, nonQueryParams);
        Debug.Log(nonQueryResult);    }

 
}

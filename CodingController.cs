using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Lawang.Coding_Tracker;

public class CodingController
{
    private readonly string _connectionString;
    public CodingController(string connectionString)
    {
        _connectionString = connectionString; 
    }

    public IDbConnection GetConnection()
    {
        using IDbConnection connection = new SqliteConnection(_connectionString);
        return connection;
    }

}

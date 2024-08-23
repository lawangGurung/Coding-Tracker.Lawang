using System;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Lawang.Coding_Tracker;

public class DatabaseManager
{
    private readonly string _connectionString;
    public DatabaseManager(string connectionString)
    {
       _connectionString = connectionString; 
    }
    public void CreateTable()
    {
        try
        {
            var createSQL =
                @"CREATE TABLE IF NOT EXISTS CodingSessions(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    StartTime TEXT,
                    EndTime TEXT,
                    Date TEXT
                )";
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Execute(createSQL);
            };

        }
        catch (SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
    }

    public void SeedDataIntoTable()
    {
        try
        {
            var insertSQL = 
                @"INSERT INTO CodingSessions (StartTime, EndTime, Date)
                    VALUES('10:00 AM', '02:30 PM', '22/08/2024'),
                          ('02:00 PM', '04:40 PM', '23/08/2024'),
                          ('05:00 PM', '09:00 PM', '24/08/2023'),
                          ('05:00 AM', '10:00 AM', '14/08/2024'),
                          ('03:00 PM', '10:00 PM', '01/08/2024')";
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            connection.Execute(insertSQL);
            connection.Close();
        }
        catch(SqliteException ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}");
            Console.ReadLine();
        }
    }
}

using System;
using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Lawang.Coding_Tracker;

public class CodingController
{
    private readonly string _connectionString;
    public CodingController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Post(CodingSession codingSession)
    {
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            string insertSQL = 
                @"INSERT INTO CodingSessions (StartTime, EndTime, Date)
                VALUES(@startTime, @endTime, @Date)";

            var parameter = new {
                StartTime = codingSession.StartTime.ToShortTimeString(),
                EndTime = codingSession.EndTime.ToShortTimeString(),
                Duration = codingSession.Duration.ToString(),
                Date = codingSession.Date.ToShortDateString()};

            int affectedRow = connection.Execute(insertSQL,parameter);

            return affectedRow;
        }
        catch(SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return -1;

    }



}

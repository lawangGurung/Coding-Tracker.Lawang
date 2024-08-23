using System;
using System.Configuration;
using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Lawang.Coding_Tracker;

public class CodingController
{
    private readonly string _connectionString;
    public CodingController()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
    }

    public int Post(CodingSession codingSession)
    {
        try
        {
            using IDbConnection connection = new SqliteConnection(_connectionString);
            string insertSQL = 
                @"INSERT INTO CodingSessions (StartTime, EndTime, Duration, Date)
                VALUES(@startTime, @endTime, @Duration, @Date)";

            var parameter = new {
                @startTime = codingSession.StartTime.ToString("hh:mm tt"),
                @endTime = codingSession.EndTime.ToString("hh:mm tt"),
                @Duration = codingSession.Duration.ToString(),
                @Date = codingSession.Date.ToString("dd/MM/yyyy")};

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

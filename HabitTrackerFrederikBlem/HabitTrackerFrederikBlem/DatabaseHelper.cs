using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

public class DatabaseHelper
{
    const string connectionString = "Data Source=habittracker.db";
    public static void CreateTableIfNotExists(string givenConnectionString = connectionString, string tableName = "drinking_water")
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER)";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static List<DrinkingWater> GetAllRecords(string givenConnectionString = connectionString, string tableName = "drinking_water")
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = @$"SELECT * FROM {tableName}";

            List<DrinkingWater> tableData = new();

            using (SqliteDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var record = new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.CurrentCulture),
                            Quantity = reader.GetInt32(2)
                        };
                        tableData.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine("No rows of records found.");
                }

                connection.Close();
            }

            return tableData;
        }
    }

    public static bool InsertRecord(string date, int quantity, string givenConnectionString = connectionString, string tableName = "drinking_water")
    { 
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = $"INSERT INTO '{tableName}'(Date, Quantity) VALUES ('{date}', {quantity})";
            
            int rowsAffected = insertCmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }            
        }
    }

    public static bool DeleteRecord(int id, string givenConnectionString = connectionString, string tableName = "drinking_water")
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = $"DELETE FROM '{tableName}' WHERE Id = {id}";
            int rowsAffected = deleteCmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }            
        }
    }

    public static bool UpdateRecord(int id, string date, int quantity, string givenConnectionString = connectionString, string tableName = "drinking_water")
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE '{tableName}' SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
            int rowsAffected = updateCmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }            
        }
    }
}

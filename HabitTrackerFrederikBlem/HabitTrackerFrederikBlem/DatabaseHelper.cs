using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

public class DatabaseHelper
{
    const string connectionString = "Data Source=habittracker.db";

    public static bool CreateDatabase(string givenConnectionString = connectionString)
    {
        try
        {
            using (var connection = new SqliteConnection(givenConnectionString))
            {
                connection.Open();
                connection.Close();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
            return false;
        }
    }

    public static bool CreateTableIfNotExists(string tableName = "tracking_habits", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity FLOAT, Unit TEXT)";
                tableCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the table: {ex.Message}");
                return false;
            }
        }
    }

    public static bool DropTable(string tableName = "tracking_habits", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var dropCmd = connection.CreateCommand();
                dropCmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                dropCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while dropping the table: {ex.Message}");
                return false;
            }
        }
    }

    public static List<Habit> GetAllRecordsFromDatabase(string givenConnectionString = connectionString)
    {
        var allRecords = new List<Habit>();

        string[] tableNames = DatabaseHelper.GetTableNames(givenConnectionString);

        foreach (string tableName in tableNames) 
        { 
            var tableRecords = GetAllRecordsFromTable(tableName, givenConnectionString);
            allRecords.AddRange(tableRecords);
        }

        return allRecords;
    }

    public static List<Habit> GetAllRecordsFromTable(string tableName = "tracking_habits", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @$"SELECT * FROM {tableName}";

                List<Habit> tableData = new();

                using (SqliteDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var record = new Habit
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.CurrentCulture),
                                Quantity = reader.GetDouble(2),
                                Unit = reader.GetString(3)
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving records: {ex.Message}");
                return new List<Habit>();
            }


        }
    }

    public static bool InsertRecord(string date, double quantity, string quantityUnit, string tableName = "tracking_habits", string givenConnectionString = connectionString)
    { 
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO '{tableName}'(Date, Quantity, Unit) VALUES (@date, @quantity, @quantityUnit)";
                insertCmd.Parameters.AddWithValue("@date", date);
                insertCmd.Parameters.AddWithValue("@quantity", quantity);
                insertCmd.Parameters.AddWithValue("@quantityUnit", quantityUnit);

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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the record: {ex.Message}");
                return false;
            }
        }
    }

    public static bool DeleteRecord(int id, string tableName = "tracking_habits", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
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
             catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the record: {ex.Message}");
                return false;
            }
        }
    }

    public static bool UpdateRecord(int id, string date, double quantity, string quantityUnit, string tableName = "tracking_habits", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE '{tableName}' SET Date = @date, Quantity = @quantity, Unit = @quantityUnit WHERE Id = @id";
            updateCmd.Parameters.AddWithValue("@date", date);
            updateCmd.Parameters.AddWithValue("@quantity", quantity);
            updateCmd.Parameters.AddWithValue("@quantityUnit", quantityUnit);
            updateCmd.Parameters.AddWithValue("@id", id);

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

    public static string[] GetTableNames(string givenConnectionString = connectionString, bool writeToConsole = false)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            List<string> tableNames = new();
            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if( reader.GetString(0) != "sqlite_sequence")
                        {
                            tableNames.Add(reader.GetString(0));
                        }                            
                    }
                }
                else if (writeToConsole)
                {
                    Console.WriteLine("No tables found in the database.");
                }
                connection.Close();
            }
            return tableNames.ToArray();
        }
    }
}

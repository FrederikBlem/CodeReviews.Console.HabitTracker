using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

public class DatabaseHelper
{
    const string connectionString = "Data Source=habittracker.db";

    public static bool CreateTableIfNotExists(string tableName = "drinking_water", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER, QuantityUnit TEXT)";
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

    public static bool DropTable(string tableName = "drinking_water", string givenConnectionString = connectionString)
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

    public static List<Habit> GetAllRecords(string tableName = "drinking_water", string givenConnectionString = connectionString)
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
                                Quantity = reader.GetInt32(2),
                                QuantityUnit = reader.GetString(3)
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

    public static bool InsertRecord(string date, int quantity, string quantityUnit, string tableName = "drinking_water", string givenConnectionString = connectionString)
    { 
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            try
            {
                connection.Open();
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $"INSERT INTO '{tableName}'(Date, Quantity, QuantityUnit) VALUES (@date, @quantity, @quantityUnit)";
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

    public static bool DeleteRecord(int id, string tableName = "drinking_water", string givenConnectionString = connectionString)
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

    public static bool UpdateRecord(int id, string date, int quantity, string quantityUnit, string tableName = "drinking_water", string givenConnectionString = connectionString)
    {
        using (var connection = new SqliteConnection(givenConnectionString))
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE '{tableName}' SET Date = @date, Quantity = @quantity, QuantityUnit = @quantityUnit WHERE Id = @id";
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

    public static string[] GetTableNames(string givenConnectionString = connectionString)
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
                else
                {
                    Console.WriteLine("No tables found in the database.");
                }
                connection.Close();
            }
            return tableNames.ToArray();
        }
    }
}

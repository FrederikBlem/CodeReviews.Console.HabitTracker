using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.Tests;

[TestFixture, Order(1)]
public class TestsOfMenuInputMethods
{
    [Test, Order (1)]
    [TestCase("25-12-2023", ExpectedResult = "25-12-2023")]
    [TestCase("01-01-2024", ExpectedResult = "01-01-2024")]
    [TestCase("2022-07-15", ExpectedResult = "15-07-2022")]
    public string GivenADateStringInputShouldReturnFormattedDateString(string dateInput)
    {
        var input = new StringReader(dateInput + "\n");
        Console.SetIn(input);

        return Menu.GetDateInput();
    }

    [Test, Order(2)]
    [TestCase("q", ExpectedResult = "q")]
    public string GivenQAsDateInputShouldReturnQ(string dateInput)
    {
        var input = new StringReader(dateInput + "\n");
        Console.SetIn(input);
        return Menu.GetDateInput();
    }

    [Test, Order(3)]
    [TestCase("1", ExpectedResult = 1)]
    [TestCase("10", ExpectedResult = 10)]
    [TestCase("2147483647", ExpectedResult = 2147483647)]
    public int GivenAPositiveNumberStringInputShouldReturnIntegerValue(string numberInput)
    {
        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);

        return Menu.GetNumberInput(message: default);
    }

    [Test, Order(4)]
    [TestCase("q", ExpectedResult = -1)]
    public int GivenQAsInputShouldReturnMinusOne(string numberInput)
    {
        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);
        return Menu.GetNumberInput(message: default);
    }

    [Test, Order(5)]
    [TestCase("1", ExpectedResult = 1)]
    [TestCase("10", ExpectedResult = 10)]
    [TestCase("2147483647", ExpectedResult = 2147483647)]
    public int GivenAPositiveNumberStringInputWithMessageShouldReturnIntegerValue(string numberInput)
    {
        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);
        return Menu.GetNumberInput("Please enter a number: ");
    }

    [Test, Order(6)]
    [TestCase("delete", "1", ExpectedResult = 1)]
    [TestCase("update", "2", ExpectedResult = 2)]
    [TestCase("implode", "3", ExpectedResult = 3)]
    [TestCase("pineapple on pizza", "3", ExpectedResult = 3)]
    [TestCase("", "4", ExpectedResult = 4)]

    public int GivenActionStringAndAPositiveNumberStringInputShouldReturnIntegerIdValueToManipulate(string action, string numberToManipulate)
    {
        var givenRecords = new List<Habit>
        {
            new Habit { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new Habit { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new Habit { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new Habit { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
        };

        var input = new StringReader(numberToManipulate + "\n");
        Console.SetIn(input);

        return Menu.GetIdToManipulateInput(action, givenRecords);
    }

    [Test, Order(7)]
    [TestCase("delete", "q", ExpectedResult = -1)]
    [TestCase("update", "q", ExpectedResult = -1)]
    [TestCase("explode", "q", ExpectedResult = -1)]
    [TestCase("salty liquorice", "q", ExpectedResult = -1)]
    [TestCase("", "q", ExpectedResult = -1)]
    public int GivenActionStringAndQAsInputShouldReturnMinusOne(string actionToDisplay, string numberToManipulate)
    {
        var givenRecords = new List<Habit>
        {
            new Habit { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new Habit { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new Habit { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new Habit { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
        };

        var input = new StringReader(numberToManipulate + "\n");
        Console.SetIn(input);

        return Menu.GetIdToManipulateInput(actionToDisplay, givenRecords);
    }
}

[TestFixture, Order(2)]
public class TestsOfMenuDisplayMethods
{
    [Test, Order(1)]
    public void ShouldDisplayMainMenuInConsole()
    {
        string expectedOutput = $@"MAIN MENU
What would you like to do?
Type q to Close Application.
Type 1 to View All Records.
Type 2 to Insert Record.
Type 3 to Delete Record.
Type 4 to Update Record.
----------------------------------------
Select an option: ";

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Menu.DisplayMainMenu();
            string result = sw.ToString();
            Assert.That(expectedOutput, Is.EqualTo(result));
        }
    }

    [Test, Order(2)]
    public void GivenListOf4DrinkingWaterRecordsShouldDisplayRecordsInConsole()
    {
        var givenRecords = new List<Habit>
        {
            new Habit { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new Habit { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new Habit { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new Habit { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
        };

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("----------------------------------------");
        foreach (var record in givenRecords)
        {
            sb.AppendLine($"ID: {record.Id} | Date: {record.Date.ToString("dd-MM-yyyy")} | Quantity: {record.Quantity}");
        }
        sb.AppendLine("----------------------------------------");

        string expectedOutput = sb.ToString().Trim();

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Menu.DisplayRecords(givenRecords);

            string result = sw.ToString().Trim();

            Assert.That(expectedOutput, Is.EqualTo(result));
        }
    }

    [Test, Order(3)]
    public void GivenEmptyListOfDrinkingWaterRecordsShouldDisplayOnlyLinesInConsole()
    {
        var givenRecords = new List<Habit>();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("----------------------------------------");
        sb.AppendLine("----------------------------------------");

        string expectedOutput = sb.ToString().Trim();

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Menu.DisplayRecords(givenRecords);

            string result = sw.ToString().Trim();

            Assert.That(expectedOutput, Is.EqualTo(result));
        }
    }
}

[TestFixture, Order(3)]
public class TestsOfDatabaseHelperMethods
{
    private const string testConnectionString = "Data Source=habittracker_test.db";

    [SetUp]
    public void Setup()
    {
        DatabaseHelper.CreateTableIfNotExists(givenConnectionString: testConnectionString);
    }

    [TearDown]
    public void TearDown()
    {
        SqliteConnection.ClearAllPools();

        string[] databaseFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "habittracker_test*.db");
        foreach (var databaseFile in databaseFiles)
        {
            File.Delete(databaseFile);
        }
    }

    [Test, Order(1)]
    [TestCase("drinking_water")]
    [TestCase("consume_water")]
    public void GivenCreateTableIfNotExistsShouldCreateTableInDatabase(string givenTestTableName, string givenTestConnectionString = testConnectionString)
    {
        DatabaseHelper.CreateTableIfNotExists(givenTestTableName, givenTestConnectionString);

        using (var connection = new SqliteConnection(givenTestConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"SELECT name FROM sqlite_master WHERE type='table' AND name='{givenTestTableName}';";
            var result = tableCmd.ExecuteScalar();
            connection.Close();
            Assert.That(result, Is.EqualTo(givenTestTableName));
        }
    }

    [Test, Order(2)]
    [TestCase("25-12-2023", 500, "laps")]
    [TestCase("01-01-2024", 750, "tablespoons")]
    [TestCase("15-07-2022", 2, "liters")]
    [TestCase("31-10-2023", 4, "cups", "consume_water", "Data Source=habittracker_test2024.db")]
    [TestCase("12-02-2024", 350, "sips", "daily_hydration", "Data Source=habittracker_test2024.db")]
    [TestCase("04-04-2024", 3, "bottles", "daily_water_intake")]
    [TestCase("20-08-2023", 1600, "ml", "daily_kcal_intake", "Data Source=habittracker_test2025.db")]
    public void InsertRecordShouldAddOneRecordToDatabase(string testDateString, int testQuantity, string testQuantityUnit, string givenTestTableName = "drinking_water", string givenTestConnectionString = testConnectionString)
    {
        DatabaseHelper.CreateTableIfNotExists(givenTestTableName, givenTestConnectionString);                   

        var testDate = DateTime.Parse(testDateString);

        using (var sw = new StringWriter()) // Without this, console output from DatabaseHelper would interfere with test results
        {
            Console.SetOut(sw);

            bool wasInserted = DatabaseHelper.InsertRecord(testDateString, testQuantity, testQuantityUnit, givenTestTableName, givenTestConnectionString);

            var records = DatabaseHelper.GetAllRecords(givenTestTableName, givenTestConnectionString);

            Assert.Multiple(() =>
            {
                Assert.That(wasInserted, Is.True);
                Assert.That(records, Has.Count.EqualTo(1));
                Assert.That(records[0].Date, Is.EqualTo(testDate));
                Assert.That(records[0].Quantity, Is.EqualTo(testQuantity));
                Assert.That(records[0].QuantityUnit, Is.EqualTo(testQuantityUnit));
                Assert.That(records[0].Id, Is.GreaterThan(0));                
            });
        }
    }

    [Test, Order(3)]
    public void GetAllRecordsShouldReturnAllRecordsFromDatabase()
    {
        var testRecords = new List<(string dateString, int quantity, string quantityUnit)>
        {
            ("25-12-2023", 500, "ml"),
            ("01-01-2024", 1, "l"),
            ("15-07-2022", 1000, "m")
        };

        foreach (var (dateString, quantity, quantityUnit) in testRecords)
        {
            DatabaseHelper.InsertRecord(dateString, quantity, quantityUnit, givenConnectionString: testConnectionString);
        }

        var records = DatabaseHelper.GetAllRecords(givenConnectionString: testConnectionString);

        Assert.Multiple(() =>
        {
            Assert.That(records, Has.Count.EqualTo(testRecords.Count));
            for (int i = 0; i < testRecords.Count; i++)
            {
                var expectedDate = DateTime.Parse(testRecords[i].dateString);
                var expectedQuantity = testRecords[i].quantity;
                Assert.That(records[i].Date, Is.EqualTo(expectedDate));
                Assert.That(records[i].Quantity, Is.EqualTo(expectedQuantity));
                Assert.That(records[i].QuantityUnit, Is.EqualTo(testRecords[i].quantityUnit));
                Assert.That(records[i].Id, Is.GreaterThan(0));
            }
        });
    }

    [Test, Order(4)]
    [TestCase("drinking_water", 1)]
    [TestCase("drinking_water", 2)]
    [TestCase("drinking_water", 3)]
    public void DeleteRecordShouldRemoveRecordFromDatabaseTable(string testTableName, int testIdToDelete)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, "ml", testTableName, givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 750, "teaspoons", givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 1000, "sips", givenConnectionString: testConnectionString);

        var recordsBeforeDeletion = DatabaseHelper.GetAllRecords(testTableName, givenConnectionString: testConnectionString);

        int idToDelete = recordsBeforeDeletion[testIdToDelete - 1].Id;

        bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete, testTableName, givenConnectionString: testConnectionString);

        var recordsAfterDeletion = DatabaseHelper.GetAllRecords(givenConnectionString: testConnectionString);

        Assert.Multiple(() =>
        {
            Assert.That(wasDeleted, Is.True);
            Assert.That(recordsAfterDeletion, Has.Count.EqualTo(recordsBeforeDeletion.Count - 1));
            Assert.That(recordsAfterDeletion.Any(r => r.Id == idToDelete), Is.False);
        });
    }
    [Test]
    [TestCase("non_existent_table", 1)]
    [TestCase("another_missing_table", 2)]
    public void DeleteRecordShouldReturnFalseWhenTableDoesNotExist(string testTableName, int testIdToDelete)
    {
        using (var sw = new StringWriter()) // Without this, console output from DatabaseHelper would interfere with test results
        {
            Console.SetOut(sw);

            bool wasDeleted = DatabaseHelper.DeleteRecord(testIdToDelete, testTableName, givenConnectionString: testConnectionString);

            Assert.That(wasDeleted, Is.False);
            Assert.That(sw.ToString(), Does.Contain($"An error occurred while deleting the record: SQLite Error 1: 'no such table: {testTableName}'."));
        }        
    }

    [Test, Order(5)]
    [TestCase(1, "31-12-2023", 1000, "ml")]
    [TestCase(2, "15-08-2024", 2, "l")]
    [TestCase(3, "01-01-2025", 2, "teaspoons")]
    public void UpdateRecordShouldModifyRecordInDatabase(int testIdToUpdate, string newDateString, int newQuantity, string newQuantityUnit)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, "ml", givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 1, "l", givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 3, "teaspoons", givenConnectionString: testConnectionString);

        var recordsBeforeUpdate = DatabaseHelper.GetAllRecords(givenConnectionString: testConnectionString);

        int idToUpdate = recordsBeforeUpdate[testIdToUpdate - 1].Id;

        bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newDateString, newQuantity, newQuantityUnit, givenConnectionString: testConnectionString);

        var recordsAfterUpdate = DatabaseHelper.GetAllRecords(givenConnectionString: testConnectionString);

        var updatedRecord = recordsAfterUpdate.First(r => r.Id == idToUpdate);

        Assert.Multiple(() =>
        {
            Assert.That(wasUpdated, Is.True);
            Assert.That(updatedRecord.Date, Is.EqualTo(DateTime.Parse(newDateString)));
            Assert.That(updatedRecord.Quantity, Is.EqualTo(newQuantity));
            Assert.That(updatedRecord.QuantityUnit, Is.EqualTo(newQuantityUnit));
        });
    }

    [Test, Order(6)]
    [TestCase(new string[] { "table_one", "table_two", "table_three" }, testConnectionString)]
    [TestCase(new string[] { "alpha", "beta", "gamma", "delta" }, "Data Source=habittracker_test2024.db")]
    [TestCase(new string[] { "first_table", "second_table" }, "Data Source=habittracker_test2025.db")]
    [TestCase(new string[] { "table_a", "table_b", "table_c", "table_d", "table_e" }, testConnectionString)]

    public void GetTableNamesShouldReturnAllTableNamesInDatabase(string[] testTableNames, string givenTestConnectionString = testConnectionString)
    {
        foreach (var tableName in testTableNames)
        {
            DatabaseHelper.CreateTableIfNotExists(tableName, givenTestConnectionString);
        }

        var tableNames = DatabaseHelper.GetTableNames(givenTestConnectionString);

        Assert.Multiple(() =>
        {
            Assert.That(tableNames, Is.Not.Null);
            Assert.That(tableNames.Length, Is.GreaterThanOrEqualTo(3));

            foreach (var tableName in testTableNames)
            {
                Assert.That(tableNames, Does.Contain(tableName));
            }            
        });
    }
}

using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text;

namespace HabitTracker.Tests;

[TestFixture]
public class TestsOfMenuInputMethods
{
    [SetUp]
    public void Setup()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-GB");
    }

    [Test, Order (1)]
    [TestCase("25-12-2023", ExpectedResult = "25-12-2023")]
    [TestCase("01-01-2024", ExpectedResult = "01-01-2024")]
    [TestCase("2022-07-15", ExpectedResult = "15-07-2022")]
    public string GivenADateStringInputShouldReturnFormattedDateString(string dateInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(dateInput + "\n");
        Console.SetIn(input);

        using (output)
        {
            return Menu.GetDateInput();
        }
    }

    [Test]
    [TestCase("q", ExpectedResult = "q")]
    public string GivenQAsDateInputShouldReturnQ(string dateInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(dateInput + "\n");
        Console.SetIn(input);

        using (output)
        {
            return Menu.GetDateInput();
        }                
    }

    [Test]
    [TestCase("1", ExpectedResult = 1)]
    [TestCase("10", ExpectedResult = 10)]
    [TestCase("2147483647", ExpectedResult = 2147483647)]
    public double GivenAPositiveNumberStringInputGetNumberShouldReturnIntegerValue(string numberInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetNumberInput(message: default);
    }

    [Test]
    [TestCase("q", ExpectedResult = -1)]
    public double GivenQAsInputShouldReturnMinusOne(string numberInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);
        using (output)
        {
            return Menu.GetNumberInput(message: default);
        }        
    }

    [Test]
    [TestCase("1", ExpectedResult = 1)]
    [TestCase("10", ExpectedResult = 10)]
    [TestCase("2147483647", ExpectedResult = 2147483647)]
    public double GivenAPositiveNumberStringInputWithMessageShouldReturnIntegerValue(string numberInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(numberInput + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetNumberInput("Please enter a number: ");
    }

    [Test]
    [TestCase("delete", "1", ExpectedResult = 1)]
    [TestCase("update", "2", ExpectedResult = 2)]
    [TestCase("implode", "3", ExpectedResult = 3)]
    [TestCase("pineapple on pizza", "3", ExpectedResult = 3)]
    [TestCase("", "4", ExpectedResult = 4)]

    public int GivenActionStringAndAPositiveNumberStringInputShouldReturnIntegerIdValueToManipulate(string action, string numberToManipulate)
    {
        var givenRecords = new List<Habit>
        {
            new() { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new() { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new() { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new() { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
        };

        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(numberToManipulate + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetIdToManipulateInput(action, givenRecords);
    }

    [Test]
    [TestCase("delete", "q", ExpectedResult = -1)]
    [TestCase("update", "q", ExpectedResult = -1)]
    [TestCase("explode", "q", ExpectedResult = -1)]
    [TestCase("salty liquorice", "q", ExpectedResult = -1)]
    [TestCase("", "q", ExpectedResult = -1)]
    public int GivenActionStringAndQAsInputShouldReturnMinusOne(string actionToDisplay, string numberToManipulate)
    {
        var givenRecords = new List<Habit>
        {
            new() { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new() { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new() { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new() { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
        };

        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(numberToManipulate + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetIdToManipulateInput(actionToDisplay, givenRecords);
    }

    [Test]
    [TestCase("ml", ExpectedResult = "ml")]
    [TestCase("liters", ExpectedResult = "liters")]
    public string GivenAQuantityUnitStringInputShouldReturnSameStringValue(string quantityUnitInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(quantityUnitInput + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetQuantityUnitInput("test");
    }

    [Test]
    [TestCase("q", ExpectedResult = "q")]
    public string GivenQAsQuantityUnitInputShouldReturnQ(string quantityUnitInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(quantityUnitInput + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetQuantityUnitInput("test");
    }

    [Test]
    [TestCase("tracking_habits", ExpectedResult = "tracking_habits")]
    [TestCase("daily_hydration", ExpectedResult = "daily_hydration")]
    public string GivenAHabitNameStringInputShouldReturnSameStringValue(string habitNameInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(habitNameInput + "\n");
        Console.SetIn(input);

        using (output)
            return Menu.GetHabitNameInput();
    }

    [Test]
    [TestCase("q", ExpectedResult = "q")]
    public string GivenQAsHabitNameInputShouldReturnQ(string habitNameInput)
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var input = new StringReader(habitNameInput + "\n");
        Console.SetIn(input);

        using (output)
        {
            return Menu.GetHabitNameInput();
        }        
    }

    [Test]
    [TestCase(new string[] { "tracking_habits", "daily_hydration", "consume_water" }, "insert", "1")]
    [TestCase(new string[] { "reading", "exercise" }, "delete", "2")]
    [TestCase(new string[] { "meditation", "yoga", "journaling", "sleep_tracking" }, "update", "4")]
    [TestCase(new string[] { "habit_one", "habit_two" }, "remove", "1")]
    public void GivenHabitTableNamesAndActionIsValidShouldLetUserSelectHabitTableAndReturnItsIndex(string[] givenTestTableNames, string givenTestAction, string indexTestInput)
    {
        var input = new StringReader(indexTestInput + "\n");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        using (output)
        {
            int resultIndex = Menu.GetHabitSelectionInput(givenTestTableNames, givenTestAction);

            int expectedIndex = int.Parse(indexTestInput);

            switch (givenTestAction)
            {
                case "insert":
                    Assert.That(output.ToString(), Does.Contain("Type the number of the habit you want to insert a record for"));
                    break;
                case "delete":
                    Assert.That(output.ToString(), Does.Contain("Type the number of the habit you want to delete from or q to return to the main menu:"));
                    break;
                case "update":
                    Assert.That(output.ToString(), Does.Contain("Type the number of the habit you want to update a record for or q to return to the main menu:"));
                    break;
                case "remove":
                    Assert.That(output.ToString(), Does.Contain("Type the number of the habit you want to drop tracking."));
                    break;
            }
            Assert.That(resultIndex, Is.EqualTo(expectedIndex));
        }
    }

    [Test]
    [TestCase(new string[] { "tracking_habits", "daily_hydration", "consume_water" }, "insert", "q")]
    public void GivenHabitTableNamesAndActionIsValidAndQAsInputShouldReturnMinusOne(string[] givenTestTableNames, string givenTestAction, string indexTestInput)
    {
        var input = new StringReader(indexTestInput + "\n");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);
        using (output)
        {
            int resultIndex = Menu.GetHabitSelectionInput(givenTestTableNames, givenTestAction);

            Assert.Multiple(() =>
            {
                Assert.That(output.ToString(), Does.Contain("Type the number of the habit you want to insert a record for"));
                Assert.That(resultIndex, Is.EqualTo(-1));
            });            
        }
    }

    [Test]
    [TestCase("delete", "5", "Data Source=habittracker_test.db")]
    [TestCase("update", "0", "Data Source=habittracker_test2024.db")]
    [TestCase("insert", "-1", "Data Source=habittracker_test2025.db")]
    [TestCase("remove", "abc", "Data Source=habittracker_test.db")]
    public void GivenNoHabitTablesInDatabaseAndActionIsValidShouldReturnMinusOneWhenUserTriesToSelectHabitTable(string givenTestAction, string invalidIndexInput, string givenTestConnectionString)
    {
        DatabaseHelper.CreateDatabase(givenTestConnectionString);

        string[] tableNamesInDatabase = DatabaseHelper.GetTableNames(givenTestConnectionString);

        var input = new StringReader(invalidIndexInput + "\n");
        Console.SetIn(input);
        var output = new StringWriter();
        Console.SetOut(output);
        using (output)
        {
            int resultIndex = Menu.GetHabitSelectionInput(tableNamesInDatabase, givenTestAction);

            Assert.Multiple(() =>
            {
                Assert.That(output.ToString(), Does.Contain("No existing habits found in the database."));
                Assert.That(resultIndex, Is.EqualTo(-1));
            });            
        }
    }
}

[TestFixture]
public class TestsOfMenuDisplayMethods
{
    [Test]
    public void ShouldDisplayMainMenuInConsole()
    {
        var sb = new StringBuilder();
        sb.AppendLine("MAIN MENU");
        sb.AppendLine("----------------------------------------");
        sb.AppendLine("What would you like to do?");
        sb.AppendLine();
        sb.AppendLine("Type q to Close Application.");
        sb.AppendLine("Type 1 to View All Records.");
        sb.AppendLine("Type 2 to Insert Record.");
        sb.AppendLine("Type 3 to Delete Record.");
        sb.AppendLine("Type 4 to Update Record.");
        sb.AppendLine("Type 5 to Remove Tracking of a Habit (erasing all records of it).");
        sb.AppendLine("----------------------------------------");
        sb.Append("Select an option: ");

        string expectedOutput = sb.ToString().Trim();

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            Menu.DisplayMainMenu();
            string result = sw.ToString().Trim();

            Assert.That(expectedOutput, Is.EqualTo(result));
        }
    }

    [Test]
    public void GivenListOf4RecordsShouldDisplayRecordsInConsole()
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
            sb.AppendLine($"ID: {record.Id} | Date: {record.Date.ToString("dd-MM-yyyy")} | Quantity: {record.Quantity}".TrimEnd());
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

    [Test]
    public void GivenEmptyListOfRecordsShouldDisplayOnlyLinesInConsole()
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

    [Test]
    public void GivenNoTableNamesShouldReturnFalseWhenDisplayingAllTableRecordsInConsole()
    {
        var givenTableNames = Array.Empty<string>();
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            bool hasDisplayed = Menu.DisplayAllTableRecords(givenTableNames);
            string result = sw.ToString().Trim();
            Assert.Multiple(() =>
            {
                Assert.That(hasDisplayed, Is.False);
                Assert.That(result, Is.EqualTo(string.Empty));
            });
        }
    }

    [Test]
    [TestCase(new string[] { "drinking_water", "daily_hydration", "consume_water" }, "test")]
    [TestCase(new string[] { "table_one", "table_two" }, "test")]
    public void GivenTableNamesShouldReturnTrueWhenDisplayingAllTableRecordsInConsole(string[] givenTestTableNames, string placeHolderNUNITNecessary)
    {
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            bool hasDisplayed = Menu.DisplayAllTableRecords(givenTestTableNames);
            string result = sw.ToString().Trim();
            Assert.Multiple(() =>
            {
                Assert.That(hasDisplayed, Is.True);

                foreach (var tableName in givenTestTableNames)
                {
                    Assert.That(result, Does.Contain($"{tableName}"));
                }                
            });
        }
    }

    [Test]
    [TestCase("12-12-1212", "12", "twelves")]
    [TestCase("01-01-0001", "1", "ones")]
    [TestCase("12-03-4567", "8", "nines")]
    [TestCase("25-11-2025", "2", "calls")]
    public void GivenValidRecordDetailsInputShouldReturnDateQuantityAndQuantityUnit(string testDate, string testQuantity, string testQuantityUnit)
    {
        var input = new StringReader(testDate + "\n" + testQuantity + "\n" + testQuantityUnit + "\n");
        Console.SetIn(input);

        using (var output = new StringWriter())
        {
            Console.SetOut(output);

            var resultRecordDetails = Menu.GetRecordDetailsInput();

            Assert.Multiple(() =>
            {
                Assert.That(resultRecordDetails.date, Is.EqualTo(testDate));
                Assert.That(resultRecordDetails.quantity.ToString(), Is.EqualTo(testQuantity));
                Assert.That(resultRecordDetails.quantityUnit, Is.EqualTo(testQuantityUnit));
            });
        }
    }
}

[TestFixture]
public class TestsOfDatabaseHelperMethods
{
    private const string testConnectionString = "Data Source=habittracker_test.db";

    [SetUp]
    public void Setup()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-GB");

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

    [Test]
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

    [Test]
    [TestCase("25-12-2023", 500, "laps")]
    [TestCase("01-01-2024", 750, "tablespoons")]
    [TestCase("15-07-2022", 2, "liters")]
    [TestCase("31-10-2023", 4, "cups", "consume_water", "Data Source=habittracker_test2024.db")]
    [TestCase("12-02-2024", 350, "sips", "daily_hydration", "Data Source=habittracker_test2024.db")]
    [TestCase("04-04-2024", 3, "bottles", "daily_water_intake")]
    [TestCase("20-08-2023", 1600, "ml", "daily_kcal_intake", "Data Source=habittracker_test2025.db")]
    public void InsertRecordShouldAddOneRecordToDatabase(string testDateString, int testQuantity, string testQuantityUnit, string givenTestTableName = "tracking_habits", string givenTestConnectionString = testConnectionString)
    {
        DatabaseHelper.CreateTableIfNotExists(givenTestTableName, givenTestConnectionString);                   

        var testDate = DateTime.Parse(testDateString);

        using (var sw = new StringWriter()) // Without this, console output from DatabaseHelper would interfere with test results
        {
            Console.SetOut(sw);

            bool wasInserted = DatabaseHelper.InsertRecord(testDateString, testQuantity, testQuantityUnit, givenTestTableName, givenTestConnectionString);

            var records = DatabaseHelper.GetAllRecordsFromTable(givenTestTableName, givenTestConnectionString);

            Assert.Multiple(() =>
            {
                Assert.That(wasInserted, Is.True);
                Assert.That(records, Has.Count.EqualTo(1));
                Assert.That(records[0].Date, Is.EqualTo(testDate));
                Assert.That(records[0].Quantity, Is.EqualTo(testQuantity));
                Assert.That(records[0].Unit, Is.EqualTo(testQuantityUnit));
                Assert.That(records[0].Id, Is.GreaterThan(0));                
            });
        }
    }

    [Test]
    [TestCase(new string[] { "tracking_habits", "reading"}, new string[] { "25-12-2023", "01-01-2024", "15-07-2022" }, new double[] {500.123, 1, 1000}, new string[] {"ml", "l", "m"})]
    [TestCase( new string[] { "table_one", "table_two", "table_three"}, new string[] { "20-11-2023", "01-01-2021", "14-08-2022" }, new double[] { 50, 12.12, 1 }, new string[] { "nautical miles", "revolutions", "times" })]
    [TestCase(new string[] { "single_table" }, new string[] {"01-01-2001"}, new double[] { 1 }, new string[] { "singles" })]
    public void GetAllRecordsShouldReturnAllRecordsFromDatabase(string[] testTableNames, string[] dateStrings, double[] quantities, string[] quantityUnits)
    {
        StringWriter output = new();
        Console.SetOut(output);

        var testRecords = new List<(string dateString, double quantity, string quantityUnit)>();

        bool[] wasCreatedTables = new bool[testTableNames.Length];
        bool[] wasInsertedRecords = new bool[dateStrings.Length];

        for (int i = 0; i < dateStrings.Length; i++)
        {
            testRecords.Add((dateStrings[i], quantities[i], quantityUnits[i]));
        }

        for (int i = 0; i < testTableNames.Length; i++)
        {
            wasCreatedTables[i] = DatabaseHelper.CreateTableIfNotExists(testTableNames[i], givenConnectionString: testConnectionString);

            for (int j = 0; j < testRecords.Count; j++)
            {
                wasInsertedRecords[j] = DatabaseHelper.InsertRecord(testRecords[j].dateString, testRecords[j].quantity, testRecords[j].quantityUnit, testTableNames[i], givenConnectionString: testConnectionString);
            }
        }

        int totalExpectedRecords = testTableNames.Length * quantityUnits.Length;

        var totalRecords = DatabaseHelper.GetAllRecordsFromDatabase(givenConnectionString: testConnectionString);

        using (output)
        {
            Assert.Multiple(() =>
            {
                Assert.That(wasCreatedTables, Is.All.True);
                Assert.That(wasInsertedRecords, Is.All.True);

                Assert.That(totalRecords, Has.Count.EqualTo(totalExpectedRecords));

                for (int i = 0; i < testRecords.Count; i++)
                {
                    var expectedDate = DateTime.Parse(testRecords[i].dateString);
                    var expectedQuantity = testRecords[i].quantity;
                    Assert.That(totalRecords[i].Date, Is.EqualTo(expectedDate));
                    Assert.That(totalRecords[i].Quantity, Is.EqualTo(expectedQuantity));
                    Assert.That(totalRecords[i].Unit, Is.EqualTo(testRecords[i].quantityUnit));
                    Assert.That(totalRecords[i].Id, Is.GreaterThan(0));
                }
            });
        }
    }

    [Test]
    [TestCase("tracking_habits", 1)]
    [TestCase("tracking_habits", 2)]
    [TestCase("tracking_habits", 3)]
    public void DeleteRecordShouldRemoveRecordFromDatabaseTable(string testTableName, int testIdToDelete)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, "ml", testTableName, givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 750, "teaspoons", testTableName, givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 1000, "sips", testTableName, givenConnectionString: testConnectionString);

        var recordsBeforeDeletion = DatabaseHelper.GetAllRecordsFromTable(testTableName, givenConnectionString: testConnectionString);

        int idToDelete = recordsBeforeDeletion[testIdToDelete - 1].Id;

        bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete, testTableName, givenConnectionString: testConnectionString);

        var recordsAfterDeletion = DatabaseHelper.GetAllRecordsFromTable(givenConnectionString: testConnectionString);

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

            Assert.Multiple(() => 
            {
                Assert.That(wasDeleted, Is.False);
                Assert.That(sw.ToString(), Does.Contain($"An error occurred while deleting the record: SQLite Error 1: 'no such table: {testTableName}'."));
            });            
        }        
    }

    [Test]
    [TestCase(1, "31-12-2023", 1000, "ml")]
    [TestCase(2, "15-08-2024", 2, "l")]
    [TestCase(3, "01-01-2025", 2, "teaspoons")]
    public void UpdateRecordShouldModifyRecordInDatabase(int testIdToUpdate, string newDateString, int newQuantity, string newQuantityUnit)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, "ml", givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 1, "l", givenConnectionString: testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 3, "teaspoons", givenConnectionString: testConnectionString);

        var recordsBeforeUpdate = DatabaseHelper.GetAllRecordsFromTable(givenConnectionString: testConnectionString);

        int idToUpdate = recordsBeforeUpdate[testIdToUpdate - 1].Id;

        bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newDateString, newQuantity, newQuantityUnit, givenConnectionString: testConnectionString);

        var recordsAfterUpdate = DatabaseHelper.GetAllRecordsFromTable(givenConnectionString: testConnectionString);

        var updatedRecord = recordsAfterUpdate.First(r => r.Id == idToUpdate);

        Assert.Multiple(() =>
        {
            Assert.That(wasUpdated, Is.True);
            Assert.That(updatedRecord.Date, Is.EqualTo(DateTime.Parse(newDateString)));
            Assert.That(updatedRecord.Quantity, Is.EqualTo(newQuantity));
            Assert.That(updatedRecord.Unit, Is.EqualTo(newQuantityUnit));
        });
    }

    [Test]
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

        if(givenTestConnectionString.Equals(testConnectionString))
        {
            testTableNames = testTableNames.Append("tracking_habits").ToArray();
        }

        var tableNames = DatabaseHelper.GetTableNames(givenTestConnectionString);

        StringWriter consoleOutput = new();
        Console.SetOut(consoleOutput);
        using (consoleOutput)
        {
            foreach (var tableName in tableNames)
            {
                Console.WriteLine($"Created table: {tableName}");
            }
        }

        Assert.Multiple(() =>
        {
            Assert.That(tableNames, Is.Not.Null);
            Assert.That(tableNames, Has.Length.EqualTo(testTableNames.Length));

            foreach (var tableName in testTableNames)
            {
                Assert.That(tableNames, Does.Contain(tableName));
            }            
        });
    }

    [Test]
    [TestCase(new string[] { "table_one", "table_two", "table_three" }, "Data Source=habittracker_test.db")]
    [TestCase(new string[] { "alpha", "beta", "gamma", "delta" }, "Data Source=habittracker_test2024.db")]
    [TestCase(new string[] { "first_table", "second_table" }, "Data Source=habittracker_test2025.db")]
    [TestCase(new string[] { "table_a", "table_b", "table_c", "table_d", "table_e" }, "Data Source=habittracker_test.db")]
    public void GivenTablesInDatabaseShouldDisplayTableNamesInConsoleExcludingSqliteSequence(string[] testTableNames, string givenTestConnectionString)
    {
        foreach (var tableName in testTableNames)
        {
            DatabaseHelper.CreateTableIfNotExists(tableName, givenTestConnectionString);
            DatabaseHelper.InsertRecord("01-01-2024", 100, "ml", tableName, givenTestConnectionString);
        }

        if (givenTestConnectionString.Equals(testConnectionString))
        {
            testTableNames = testTableNames.Append("tracking_habits").ToArray();
        }

        var expectedTableNames = testTableNames;
        var resultTableNames = DatabaseHelper.GetTableNames(givenTestConnectionString);

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            bool hasDisplayed = Menu.DisplayTableNames(resultTableNames);
            string result = sw.ToString().Trim();

            Assert.Multiple(() =>
            {
                Assert.That(hasDisplayed, Is.True);
                Assert.That(resultTableNames, Has.Length.EqualTo(expectedTableNames.Length));

                foreach (var tableName in testTableNames)
                {
                    Assert.That(result, Does.Contain($"{tableName}"));
                }

                foreach (var resultTableName in resultTableNames)
                {
                    Assert.That(expectedTableNames, Does.Contain(resultTableName));
                }

                Assert.That(result, Does.Not.Contain("sqlite_sequence"));
            });
        }

        foreach (var tableName in testTableNames)
        {
            DatabaseHelper.DropTable(tableName, givenTestConnectionString);
        }
    }
}

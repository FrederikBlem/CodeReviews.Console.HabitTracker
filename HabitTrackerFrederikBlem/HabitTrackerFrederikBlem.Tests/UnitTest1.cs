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
        var givenRecords = new List<DrinkingWater>
        {
            new DrinkingWater { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new DrinkingWater { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new DrinkingWater { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new DrinkingWater { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
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
        var givenRecords = new List<DrinkingWater>
        {
            new DrinkingWater { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new DrinkingWater { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new DrinkingWater { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new DrinkingWater { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
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
        var givenRecords = new List<DrinkingWater>
        {
            new DrinkingWater { Id = 1, Date = new DateTime(2023, 12, 25), Quantity = 500 },
            new DrinkingWater { Id = 2, Date = new DateTime(2023, 12, 26), Quantity = 750 },
            new DrinkingWater { Id = 3, Date = new DateTime(2023, 12, 27), Quantity = 1000 },
            new DrinkingWater { Id = 4, Date = new DateTime(2024, 1, 1), Quantity = 250 }
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
        var givenRecords = new List<DrinkingWater>();

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
        DatabaseHelper.CreateTableIfNotExists(testConnectionString);
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
    [TestCase("Data Source=habittracker_test.db", "drinking_water")]
    [TestCase("Data Source=habittracker_test.db", "consume_water")]
    public void GivenCreateTableIfNotExistsShouldCreateTableInDatabase(string givenTestConnectionString, string givenTestTableName)
    {
        DatabaseHelper.CreateTableIfNotExists(givenTestConnectionString, givenTestTableName);

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
    [TestCase("25-12-2023", 500)]
    [TestCase("01-01-2024", 750)]
    [TestCase("15-07-2022", 1000)]
    [TestCase("31-10-2023", 250, "Data Source=habittracker_test2024.db", "consume_water")]
    [TestCase("12-02-2024", 350, "Data Source=habittracker_test2024.db", "daily_hydration")]
    [TestCase("04-04-2024", 600, "Data Source=habittracker_test.db", "daily_water_intake")]
    [TestCase("20-08-2023", 1600, "Data Source=habittracker_test2025.db", "daily_kcal_intake")]
    public void GivenInsertRecordShouldAddOneRecordToDatabase(string testDateString, int testQuantity, string givenTestConnectionString = "Data Source=habittracker_test.db", string givenTestTableName = "drinking_water")
    {
        DatabaseHelper.CreateTableIfNotExists(givenTestConnectionString, givenTestTableName);                   

        var testDate = DateTime.Parse(testDateString);

        using (var sw = new StringWriter()) // Without this, console output from DatabaseHelper would interfere with test results
        {
            Console.SetOut(sw);

            bool wasInserted = DatabaseHelper.InsertRecord(testDateString, testQuantity, givenTestConnectionString, givenTestTableName);

            var records = DatabaseHelper.GetAllRecords(givenTestConnectionString, givenTestTableName);

            
            Assert.Multiple(() =>
            {
                Assert.That(wasInserted, Is.True);
                Assert.That(records, Has.Count.EqualTo(1));
                Assert.That(records[0].Date, Is.EqualTo(testDate));
                Assert.That(records[0].Quantity, Is.EqualTo(testQuantity));
                Assert.That(records[0].Id, Is.GreaterThan(0));                
            });
        }
    }

    [Test, Order(3)]
    public void GivenGetAllRecordsShouldReturnAllRecordsFromDatabase()
    {
        var testRecords = new List<(string dateString, int quantity)>
        {
            ("25-12-2023", 500),
            ("01-01-2024", 750),
            ("15-07-2022", 1000)
        };

        foreach (var (dateString, quantity) in testRecords)
        {
            DatabaseHelper.InsertRecord(dateString, quantity, testConnectionString);
        }

        var records = DatabaseHelper.GetAllRecords(testConnectionString);

        Assert.Multiple(() =>
        {
            Assert.That(records, Has.Count.EqualTo(testRecords.Count));
            for (int i = 0; i < testRecords.Count; i++)
            {
                var expectedDate = DateTime.Parse(testRecords[i].dateString);
                var expectedQuantity = testRecords[i].quantity;
                Assert.That(records[i].Date, Is.EqualTo(expectedDate));
                Assert.That(records[i].Quantity, Is.EqualTo(expectedQuantity));
                Assert.That(records[i].Id, Is.GreaterThan(0));
            }
        });
    }

    [Test, Order(4)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void GivenDeleteRecordShouldRemoveRecordFromDatabase(int testIdToDelete)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 750, testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 1000, testConnectionString);

        var recordsBeforeDeletion = DatabaseHelper.GetAllRecords(testConnectionString);

        int idToDelete = recordsBeforeDeletion[testIdToDelete - 1].Id;

        bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete, testConnectionString);

        var recordsAfterDeletion = DatabaseHelper.GetAllRecords(testConnectionString);

        Assert.Multiple(() =>
        {
            Assert.That(wasDeleted, Is.True);
            Assert.That(recordsAfterDeletion, Has.Count.EqualTo(recordsBeforeDeletion.Count - 1));
            Assert.That(recordsAfterDeletion.Any(r => r.Id == idToDelete), Is.False);
        });
    }

    [Test, Order(5)]
    [TestCase(1, "31-12-2023", 1000)]
    [TestCase(2, "15-08-2024", 1500)]
    [TestCase(3, "01-01-2025", 2000)]
    public void GivenUpdateRecordShouldModifyRecordInDatabase(int testIdToUpdate, string newDateString, int newQuantity)
    {
        DatabaseHelper.InsertRecord("25-12-2023", 500, testConnectionString);
        DatabaseHelper.InsertRecord("01-01-2024", 750, testConnectionString);
        DatabaseHelper.InsertRecord("15-07-2022", 1000, testConnectionString);

        var recordsBeforeUpdate = DatabaseHelper.GetAllRecords(testConnectionString);

        int idToUpdate = recordsBeforeUpdate[testIdToUpdate - 1].Id;

        bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newDateString, newQuantity, testConnectionString);

        var recordsAfterUpdate = DatabaseHelper.GetAllRecords(testConnectionString);

        var updatedRecord = recordsAfterUpdate.First(r => r.Id == idToUpdate);

        Assert.Multiple(() =>
        {
            Assert.That(wasUpdated, Is.True);
            Assert.That(updatedRecord.Date, Is.EqualTo(DateTime.Parse(newDateString)));
            Assert.That(updatedRecord.Quantity, Is.EqualTo(newQuantity));
        });
    }
}

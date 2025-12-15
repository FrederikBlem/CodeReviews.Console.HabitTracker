using System.Text.RegularExpressions;

namespace HabitTracker;

public class Menu
{
    static readonly string lineSpacing = "----------------------------------------";
    static readonly string getQuantityUnitMessage = "Enter the unit of measurement.\n(e.g., liters, kilometers, bunches, pages, rotations. etc.) - It is recommended to avoid units of time.\nType q to return to main menu. ";
    static readonly string getQuantityMessage = "Enter the quantity. Type q to return to main menu. ";
    static string[]? tableNames;
    private static List<Habit> habitRecords = new();
    
    public static bool GetUserInputForMainMenu()
    {
        DisplayMainMenu();

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "q":
                Console.WriteLine("Closing Application...");
                return true;
            case "1":
                Console.Clear();
                tableNames = DatabaseHelper.GetTableNames();

                bool foundTables = DisplayAllTableRecords(tableNames);
                if (foundTables == false)
                {
                    Console.WriteLine("No habits found in the database.");
                }

                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
                return false;
            case "2":
                Console.Clear();

                var habitInsertResult = ArrangeInsertionOfARecord();

                if (habitInsertResult.wasInserted)
                {
                    Console.WriteLine("Record inserted successfully.");
                }
                else if (!habitInsertResult.wasInserted && habitInsertResult.tableName == "q")
                {
                    Console.WriteLine("No record was inserted. User Chose to exit to main menu.");
                }
                else
                {
                    Console.WriteLine("Failed to insert record into database.");
                }

                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                return false;
            case "3":
                Console.Clear();

                var habitDeleteResult = ArrangeDeletionOfARecord();

                if (habitDeleteResult.wasDeleted)
                {
                    Console.WriteLine($"Habit {habitDeleteResult.habitTable}'s record was {habitDeleteResult.actionString}d");
                }
                else if (!habitDeleteResult.wasDeleted && habitDeleteResult.habitTable == "q")
                {
                    Console.WriteLine("No record was deleted. User Chose to exit to main menu.");
                }
                else
                {
                    Console.WriteLine("Failed to delete record from database.");
                }

                return false;
            case "4":
                Console.Clear();

                var habitUpdateResult = ArrangeUpdateOfARecord();

                if (habitUpdateResult.wasUpdated)
                {
                    Console.WriteLine($"Habit {habitUpdateResult.tableName}'s record was {habitUpdateResult.actionString}d");
                }
                else if (!habitUpdateResult.wasUpdated && habitUpdateResult.tableName == "q")
                {
                    Console.WriteLine("No record was updated. User Chose to exit to main menu.");
                }
                else if (!habitUpdateResult.wasUpdated && habitUpdateResult.tableName == "0")
                {
                    Console.WriteLine("No records to display or update.");
                }
                else
                {
                    Console.WriteLine("Failed to update record in database.");
                }

                return false;
            case "5":
                Console.Clear();

                var habitTableDropResult = ArrangeDroppingHabitTable();

                if(habitTableDropResult.wasRemoved)
                {
                    Console.WriteLine($"Habit {habitTableDropResult.tableName} and all its records were successfully removed from the database.");
                }
                else if(!habitTableDropResult.wasRemoved && habitTableDropResult.tableName == "q")
                {
                    Console.WriteLine("No habit was removed from the database. User Chose to exit to main menu.");
                }
                else
                {
                    if (!habitTableDropResult.tableName.Equals("empty"))
                    {
                        Console.WriteLine($"Failed to {habitTableDropResult.actionString} habit {habitTableDropResult.tableName} from database.");
                    }
                }

                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                return false;
            default:
                Console.WriteLine("Invalid option. Please type a number from 0 to 5.");
                return false;
        }
    }

    public static (bool wasInserted, string tableName) ArrangeInsertionOfARecord()
    {
        tableNames = DatabaseHelper.GetTableNames();
        string habitName;

        int habitTableNumber = GetHabitSelectionInput(tableNames, "insert");

        switch (habitTableNumber)
        {
            case -1:
                Console.Clear();
                return (false, "q");
            case 0:
                habitName = GetHabitNameInput();
                if (habitName == "q")
                {
                    Console.Clear();
                    return (false, "q");
                }
                DatabaseHelper.CreateTableIfNotExists(habitName);
                break;
            default:
                habitName = tableNames[habitTableNumber - 1];
                DatabaseHelper.CreateTableIfNotExists(habitName);
                break;
        }

        Console.WriteLine(lineSpacing);

        var recordDetails = GetRecordDetailsInput();

        if (recordDetails.date == "q" || recordDetails.quantityUnit == "q" || recordDetails.quantity == -1)
        {
            Console.Clear();
            return (false, "q");
        }

        bool wasInserted = DatabaseHelper.InsertRecord(recordDetails.date, recordDetails.quantity, recordDetails.quantityUnit, habitName);

        return (wasInserted, habitName);
    }

    public static (bool wasDeleted, string habitTable, string actionString) ArrangeDeletionOfARecord()
    {
        tableNames = DatabaseHelper.GetTableNames();
        string habitToDeleteFrom = "";
        string actionDelete = "delete";

        int habitTableToDeleteFromNumber = GetHabitSelectionInput(tableNames, actionDelete);

        switch (habitTableToDeleteFromNumber)
        {
            case -1:
                Console.Clear();
                return (false, "q", actionDelete);
            case 0:
                habitToDeleteFrom = GetHabitNameInput();
                if (habitToDeleteFrom == "q")
                {
                    Console.Clear();
                    return (false, habitToDeleteFrom, actionDelete);
                }
                break;
            default:
                habitToDeleteFrom = tableNames[habitTableToDeleteFromNumber - 1];
                break;
        }

        habitRecords = DatabaseHelper.GetAllRecordsFromTable(habitToDeleteFrom);
        if (habitRecords.Count == 0)
        {
            return (false, habitToDeleteFrom, actionDelete);
        }
        else
        {
            DisplayRecords(habitRecords);

            
            int idToDelete = GetIdToManipulateInput(actionDelete, habitRecords);
            if (idToDelete == -1)
            {
                return (false, habitToDeleteFrom, actionDelete);
            }

            bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete, habitToDeleteFrom);

            if (wasDeleted)
            {
                habitRecords.RemoveAll(x => x.Id == idToDelete);
            }

            return (wasDeleted, habitToDeleteFrom, actionDelete);
        }
    }

    public static (bool wasUpdated, string tableName, string actionString) ArrangeUpdateOfARecord()
    {
        tableNames = DatabaseHelper.GetTableNames();
        string habitToUpdate = "";
        string actionUpdate = "update";

        int habitTableToUpdateNumber = GetHabitSelectionInput(tableNames, actionUpdate);

        switch (habitTableToUpdateNumber)
        {
            case -1:
                Console.Clear();
                return (false, "q", actionUpdate);
            default:
                habitToUpdate = tableNames[habitTableToUpdateNumber - 1];
                break;
        }

        habitRecords = DatabaseHelper.GetAllRecordsFromTable(habitToUpdate);
        if (habitRecords.Count == 0)
        {
            return (false, "0", actionUpdate);
        }
        else
        {
            DisplayRecords(habitRecords);

            
            int idToUpdate = GetIdToManipulateInput(actionUpdate, habitRecords);
            if (idToUpdate == -1)
            {
                Console.Clear();
                return (false, habitToUpdate, actionUpdate);
            }

            var newRecordDetails = GetRecordDetailsInput();

            if (newRecordDetails.date == "q" || newRecordDetails.quantity == -1 || newRecordDetails.quantityUnit == "q")
            {
                Console.Clear();
                return (false, habitToUpdate, actionUpdate);
            }

            bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newRecordDetails.date, newRecordDetails.quantity, newRecordDetails.quantityUnit, habitToUpdate);

            if (wasUpdated)
            {
                Habit habitRecordToUpdate = habitRecords.First(x => x.Id == idToUpdate);

                habitRecordToUpdate.Date = DateTime.ParseExact(newRecordDetails.date, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
                habitRecordToUpdate.Quantity = newRecordDetails.quantity;
                habitRecordToUpdate.Unit = newRecordDetails.quantityUnit;

                return (wasUpdated, habitToUpdate, actionUpdate);
            }

            return (wasUpdated, habitToUpdate, actionUpdate);
        }
    }

    public static (bool wasRemoved, string tableName, string actionString) ArrangeDroppingHabitTable()
    {
        tableNames = DatabaseHelper.GetTableNames();

        if (tableNames.Length == 0 || tableNames[0].Contains("sqlite_sequence"))
        {
            Console.WriteLine("No existing habits found in the database.");
            return (false, "empty", "remove");
        }

        string habitToRemove;

        string actionRemove = "remove";
        int habitTableToRemoveNumber = GetHabitSelectionInput(tableNames, actionRemove);

        switch (habitTableToRemoveNumber)
        {
            case -1:
                Console.Clear();
                return (false, "q", actionRemove);
            default:
                habitToRemove = tableNames[habitTableToRemoveNumber - 1];
                break;
        }

        bool wasDropped = DatabaseHelper.DropTable(habitToRemove);
        if (wasDropped)
        {
            
            return (true, habitToRemove, actionRemove);
        }
        else
        {
            
            return (false, habitToRemove, actionRemove);  
        }
    }

    public static string GetHabitNameInput()
    {
        Regex habitRegex = new(@"^[a-zA-Z-_ ]*$");

        string habitNameInstructions = @"Enter the habit name you want to track (e.g., drinking water, running, reading, etc.).
Only lower and upper case letters a-z, _ and space are allowed.
Type q to return to main menu.";

        Console.WriteLine(habitNameInstructions);
        string habitNameInput = Console.ReadLine().Trim().ToLower();

        while (string.IsNullOrWhiteSpace(habitNameInput) || !habitRegex.IsMatch(habitNameInput))
        {
            if (habitNameInput == "q")
            {
                return habitNameInput;
            }

            if (Regex.IsMatch(habitNameInput, @"[\/\\\:\*\?\""\<\>\|]") || !habitRegex.IsMatch(habitNameInput))
            {
                Console.Clear();
                Console.WriteLine(habitNameInstructions);
                Console.WriteLine("Habit name cannot contain numbers or any of the following characters: / \\ : * ? \" < > |");
                Console.WriteLine("Please enter a valid habit name or q to return to the main menu: ");
                habitNameInput = Console.ReadLine().Trim().ToLower();
                habitNameInput.Replace(" ", "_");
                continue;
            }

            Console.WriteLine("Invalid input. Please enter a valid habit name or q to return to the main menu: ");
            habitNameInput = Console.ReadLine().Trim().ToLower();
            habitNameInput.Replace(" ", "_");
        }
        return habitNameInput;
    }

    public static string GetDateInput()
    {
        string dateInstructions = "Enter the date for the habit record in the format dd-MM-yyyy. Type q to return to main menu.";

        Console.Write(dateInstructions);

        DateTime userDateTime;
        string dateInput = Console.ReadLine().Trim().ToLower();
        while (!DateTime.TryParse(dateInput, out userDateTime))
        {
            if (dateInput == "q")
            {
                return dateInput;
            }

            Console.Clear();
            Console.WriteLine(dateInstructions);
            Console.Write("Invalid date format. Please enter the date in the format dd-MM-yyyy: ");
            dateInput = Console.ReadLine().Trim().ToLower();
        }

        string dateResult = userDateTime.ToString("dd-MM-yyyy");

        return dateResult;
    }

    public static double GetNumberInput(string message)
    {
        Console.Write(message);
        double result;
        string quantityInput = Console.ReadLine();
        while (!double.TryParse(quantityInput, out result) || result < 0)
        {
            if (quantityInput == "q")
            {
                return -1;
            }

            Console.Clear();
            Console.WriteLine(message);
            Console.Write("Invalid input. Please enter a valid positive number or q to return to the main menu: ");
            quantityInput = Console.ReadLine();
        }

        return result;
    }

    public static string GetQuantityUnitInput(string message)
    {
        Console.Write(message);
        string quantityUnit = Console.ReadLine().Trim().ToLower();
        while (string.IsNullOrWhiteSpace(quantityUnit))
        {
            if (quantityUnit == "q")
            {
                return quantityUnit;
            }

            Console.Clear();
            Console.WriteLine(message);
            Console.Write("Invalid input. Please enter a valid unit of measurement or q to return to the main menu: ");
            quantityUnit = Console.ReadLine().Trim().ToLower();
        }
        return quantityUnit;
    }

    public static int GetIdToManipulateInput(string actionToDisplay, List<Habit> givenRecords)
    {
        string idInstructions = $"Enter the ID of the record you want to {actionToDisplay}. Type q to return to the main menu.";

        Console.Write(idInstructions);
        int idToManipulate;

        string? input = Console.ReadLine();
        if (input == "q")
        {
            return -1;
        }

        bool validIdInput = int.TryParse(input, out idToManipulate);
        while (!validIdInput || !givenRecords.Any(x=> x.Id == idToManipulate))
        {
            Console.Clear();
            Console.WriteLine(idInstructions);
            Console.Write($"Invalid ID. Please enter a valid record ID to {actionToDisplay} or type q to return to the main menu. ");
            input = Console.ReadLine();
            if (input == "q")
            {
                return -1;
            }
            validIdInput = int.TryParse(input, out idToManipulate);
        }

        return idToManipulate;
    }

    public static int GetHabitSelectionInput(string[] existingHabits, string action)
    {
        string promptMessage;
        switch (action)
        {
            case "delete":
                promptMessage = "Type the number of the habit you want to delete from or q to return to the main menu: ";
                break;
            case "insert":
                promptMessage = "Type the number of the habit you want to insert a record for, type q to return to the main menu\n or type n to create a new habit:\n";
                break;
            case "update":
                promptMessage = "Type the number of the habit you want to update a record for or q to return to the main menu: ";
                break;
            case "remove":
                promptMessage = "Type the number of the habit you want to drop tracking.\nThis will destroy all records of the habit. \nYou can also type q to return to the main menu. ";
                break;
            default:
                Console.WriteLine("Invalid action specified for habit selection input.");
                return -1;
        }

        if (existingHabits.Length == 0 || existingHabits[0].Contains("sqlite_sequence"))
        {
            Console.WriteLine("No existing habits found in the database.");
            return -1;
        }
        else
        {
            Console.WriteLine("Existing habits in the database:\n");

            DisplayTableNames(existingHabits);
            Console.WriteLine();

            Console.Write(promptMessage);
            string habitChoice = Console.ReadLine().Trim().ToLower();
            while (habitChoice != "n" && (!int.TryParse(habitChoice, out int habitNumber) && action.Equals("insert") || habitNumber < 1 || habitNumber > existingHabits.Length) || habitChoice.Equals("q"))
            {
                if (habitChoice.Equals("q"))
                {
                    return -1;
                }

                Console.Clear();
                DisplayTableNames(existingHabits);

                Console.WriteLine(promptMessage);
                Console.WriteLine("Invalid input. Please type a valid number of an existing habit or n to create a new habit:");
                habitChoice = Console.ReadLine().Trim().ToLower();                
            }

            if (habitChoice != "n")
            {
                return int.Parse(habitChoice);
            }

            if (!action.Equals("insert"))
            {
                return -1;
            }

            return 0;
        }
    }

    public static (string date, double quantity, string quantityUnit) GetRecordDetailsInput()
    {
        Console.WriteLine("You will now be asked to enter Date, Quantity and Unit for your habit.\n");

        string date = GetDateInput();
        if (date == "q")
        {
            return (date, -1, "q");
        }

        double quantity = GetNumberInput(getQuantityMessage);
        if (quantity == -1)
        {
            return (date, quantity, "q");
        }

        string quantityUnit = GetQuantityUnitInput(getQuantityUnitMessage);
        if (quantityUnit == "q")
        {
            return (date, -1, quantityUnit);
        }

        return (date, quantity, quantityUnit);
    }

    public static void DisplayMainMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine(lineSpacing);
        Console.WriteLine("What would you like to do?");
        Console.WriteLine();
        Console.WriteLine("Type q to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("Type 5 to Remove Tracking of a Habit (erasing all records of it).");
        Console.WriteLine(lineSpacing);
        Console.Write("Select an option: ");
    }

    public static void DisplayRecords(List<Habit> givenRecords)
    {
        Console.WriteLine(lineSpacing);
        foreach (var record in givenRecords)
        {
            Console.WriteLine($"ID: {record.Id} | Date: {record.Date:dd-MM-yyyy} | Quantity: {record.Quantity} {record.Unit}".TrimEnd());
        }
        Console.WriteLine(lineSpacing);
    }

    public static bool DisplayAllTableRecords(string[] givenTableNames)
    {        
        tableNames = givenTableNames;
        if (tableNames.Length == 0 || tableNames[0].Contains("sqlite_sequence"))
        {
            return false;
        }
        else
        {
            int totalRecords = 0;

            Console.WriteLine("Existing habits in the database:\n");
            for (int i = 0; i < tableNames.Length; i++)
            {
                Console.WriteLine($"Habit {i + 1}: {tableNames[i]}");

                habitRecords = DatabaseHelper.GetAllRecordsFromTable(tableNames[i]);
                if (habitRecords.Count == 0)
                {
                    Console.WriteLine(lineSpacing);
                    Console.WriteLine($"No records for the habit {tableNames[i]}.");
                    Console.WriteLine(lineSpacing);
                }
                else
                {
                    DisplayRecords(habitRecords);
                    totalRecords += habitRecords.Count;
                }
                Console.WriteLine();
            }
            Console.WriteLine($"For a total of {totalRecords} records for {tableNames.Length} habits.\n");
            return true;
        }
    }

    public static bool DisplayTableNames(string[] givenTableNames)
    { 
        tableNames = givenTableNames;

        if (tableNames.Length == 0 || tableNames[0].Contains("sqlite_sequence"))
        {
            return false;
        }
        else
        {
            Console.WriteLine("Existing habits in the database:\n");
            for (int i = 0; i < tableNames.Length; i++)
            {
                Console.WriteLine($"Habit {i + 1}: {tableNames[i]}");
            }
            Console.WriteLine();
            return true;
        }
    }
}

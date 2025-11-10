namespace HabitTracker;
public class Menu
{
    static readonly string lineSpacing = "----------------------------------------";
    static readonly string getQuantityUnitMessage = "Enter the unit of measurement.\n(e.g., liters, kilometers, bunches, minutes, rotations. etc.).\nType q to return to main menu. ";
    static readonly string getQuantityMessage = "Enter the quantity. Type q to return to main menu. ";
    static string[] tableNames;
    private static List<Habit> habitRecords = new();

    #region Input Methods
    public static bool GetUserInputForMainMenu()
    {
        DisplayMainMenu();

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "q":
                Console.WriteLine("Closing Application...");
                return true;
            case "1":
                Console.Clear();
                tableNames = DatabaseHelper.GetTableNames();
                if(tableNames.Length == 0 || tableNames[0].Contains("sqlite_sequence"))
                {
                    Console.WriteLine("No tables found in the database.");
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    Console.WriteLine("Existing habits in the database:");
                    for(int i = 0; i < tableNames.Length - 1; i++) // -1 to exclude sqlite_sequence table
                    {
                        Console.WriteLine($"Habit: {tableNames[i]}");

                        habitRecords = DatabaseHelper.GetAllRecords(tableNames[i]);
                        if (habitRecords.Count == 0)
                        {
                            Console.WriteLine(lineSpacing);
                            Console.WriteLine($"No records for the habit {tableNames[i]}.");
                            Console.WriteLine(lineSpacing);
                        }
                        else
                        {
                            DisplayRecords(habitRecords);
                        }
                    }
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }                        
            case "2":
                tableNames = DatabaseHelper.GetTableNames();
                string habitName = "";

                int habitTableNumber = GetHabitSelectionInput(tableNames, "insert");      
                
                switch (habitTableNumber)
                {
                    case -1:
                        Console.Clear();
                        return false;
                    case 0:
                        habitName = GetHabitNameInput();
                        if (habitName == "q")
                        {
                            Console.Clear();
                            return false;
                        }
                        DatabaseHelper.CreateTableIfNotExists(habitName);
                        break;
                    default:
                        habitName = tableNames[habitTableNumber - 1];
                        DatabaseHelper.CreateTableIfNotExists(habitName);
                        break;
                }

                Console.WriteLine(lineSpacing);

                string date = GetDateInput();
                if (date == "q")
                {
                    Console.Clear();
                    return false;
                }

                string quantityUnit = GetQuantityUnit(getQuantityUnitMessage);
                if (quantityUnit == "q")
                {
                    Console.Clear();
                    return false;
                }

                int quantity = GetNumberInput(getQuantityMessage);
                if (quantity == -1)
                {
                    Console.Clear();
                    return false;
                }       

                bool wasInserted = DatabaseHelper.InsertRecord(date, quantity, quantityUnit, habitName);

                if (wasInserted)
                {
                    Console.WriteLine("Record inserted successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to insert record into database.");
                }

                Console.WriteLine("Press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
                return false;
            case "3":               
                tableNames = DatabaseHelper.GetTableNames();
                string habitToDeleteFrom = "";

                int habitTableToDeleteFromNumber = GetHabitSelectionInput(tableNames, "delete");

                switch(habitTableToDeleteFromNumber)
                {
                    case -1:
                        Console.Clear();
                        return false;
                    case 0:
                        habitToDeleteFrom = GetHabitNameInput();
                        if (habitToDeleteFrom == "q")
                        {
                            Console.Clear();
                            return false;
                        }
                        DatabaseHelper.CreateTableIfNotExists(habitToDeleteFrom);
                        break;
                    default:
                        habitToDeleteFrom = tableNames[habitTableToDeleteFromNumber - 1];
                        DatabaseHelper.CreateTableIfNotExists(habitToDeleteFrom);
                        break;
                }

                habitRecords = DatabaseHelper.GetAllRecords(habitToDeleteFrom);
                if (habitRecords.Count == 0)
                {
                    Console.WriteLine("No records to display or delete. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    DisplayRecords(habitRecords);

                    string action = "delete";
                    int idToDelete = GetIdToManipulateInput(action, habitRecords);
                    if (idToDelete == -1)
                    {
                        Console.ReadKey();
                        Console.Clear();
                        return false;
                    }

                    bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete, habitToDeleteFrom);

                    if (wasDeleted)
                    {
                        Console.WriteLine($"Record {action}d successfully.");
                        habitRecords.RemoveAll(x => x.Id == idToDelete);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to {action} record from database.");
                    }

                    return false;
                }
            case "4":
                tableNames = DatabaseHelper.GetTableNames();
                string habitToUpdate = "";

                int habitTableToUpdateNumber = GetHabitSelectionInput(tableNames, "update");

                switch (habitTableToUpdateNumber)
                {
                    case -1:
                        Console.Clear();
                        return false;
                    default:
                        habitToUpdate = tableNames[habitTableToUpdateNumber - 1];
                        break;
                }

                habitRecords = DatabaseHelper.GetAllRecords(habitToUpdate);
                if (habitRecords.Count == 0)
                {
                    Console.WriteLine("No records to display or update. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    DisplayRecords(habitRecords);

                    string action = "update";
                    int idToUpdate = GetIdToManipulateInput(action, habitRecords);
                    if (idToUpdate == -1)
                    {
                        Console.Clear();
                        return false;
                    }

                    string newDate = GetDateInput();
                    if (newDate == "q")
                    {
                        Console.Clear();
                        return false;
                    }

                    string newQuantityUnit = GetQuantityUnit(getQuantityUnitMessage);
                    if (newQuantityUnit == "q")
                    {
                        Console.Clear();
                        return false;
                    }

                    int newQuantity = GetNumberInput(getQuantityMessage);
                    if (newQuantity == -1)
                    {
                        Console.Clear();
                        return false;
                    }

                    bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newDate, newQuantity, newQuantityUnit, habitToUpdate);

                    if (wasUpdated)
                    {
                        Console.WriteLine($"Record {action}d successfully.");
                        Habit habitRecordToUpdate = habitRecords.First(x => x.Id == idToUpdate);

                        habitRecordToUpdate.Date = DateTime.ParseExact(newDate, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        habitRecordToUpdate.Quantity = newQuantity;
                        habitRecordToUpdate.QuantityUnit = newQuantityUnit;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to {action} record from database.");
                    }

                    return false;
                }
            default:
                Console.WriteLine("Invalid option. Please type a number from 0 to 4.");
                return false;
        }
    }

    public static string GetHabitNameInput()
    {
        Console.Write("Enter the habit name you want to track (e.g., drinking_water, running, reading, etc.). Type q to return to main menu. ");
        string habitNameInput = Console.ReadLine().Trim().ToLower();
        while (string.IsNullOrWhiteSpace(habitNameInput))
        {
            if (habitNameInput == "q")
            {
                return habitNameInput;
            }

            Console.Write("Invalid input. Please enter a valid habit name or q to return to the main menu: ");
            habitNameInput = Console.ReadLine().Trim().ToLower();
            habitNameInput.Replace(" ", "_").Replace("-", "_");
        }
        return habitNameInput;
    }

    public static string GetDateInput()
    {
        Console.Write("Enter the dateInput: (Format: dd-MM-yyyy). Type q to return to main menu. ");

        DateTime userDateTime;
        string dateInput = Console.ReadLine().Trim().ToLower();
        while (!DateTime.TryParse(dateInput, out userDateTime))
        {
            if (dateInput == "q")
            {
                return dateInput;
            }

            Console.Write("Invalid date format. Please enter the date in the format dd-MM-yyyy: ");
            dateInput = Console.ReadLine().Trim().ToLower();
        }

        string dateResult = userDateTime.ToString("dd-MM-yyyy");

        return dateResult;
    }

    public static int GetNumberInput(string message)
    {
        Console.Write(message);
        int result;
        string quantityInput = Console.ReadLine();
        while (!int.TryParse(quantityInput, out result) || result < 0)
        {
            if (quantityInput == "q")
            {
                return -1;
            }

            Console.Write("Invalid input. Please enter a valid positive, whole number or q to return to the main menu: ");
            quantityInput = Console.ReadLine();
        }

        return result;
    }

    public static string GetQuantityUnit(string message)
    {
        Console.Write(message);
        string quantityUnit = Console.ReadLine().Trim().ToLower();
        while (string.IsNullOrWhiteSpace(quantityUnit))
        {
            if (quantityUnit == "q")
            {
                return quantityUnit;
            }

            Console.Write("Invalid input. Please enter a valid unit of measurement or q to return to the main menu: ");
            quantityUnit = Console.ReadLine().Trim().ToLower();
        }
        return quantityUnit;
    }

    public static int GetIdToManipulateInput(string actionToDisplay, List<Habit> givenDrinkingWaterRecords)
    {
        Console.Write($"Enter the ID of the record you want to {actionToDisplay}. Type q to return to the main menu. ");
        int idToManipulate;

        string? input = Console.ReadLine();
        if (input == "q")
        {
            return -1;
        }

        bool validIdInput = int.TryParse(input, out idToManipulate);
        while (!validIdInput || !givenDrinkingWaterRecords.Any(x=> x.Id == idToManipulate))
        {
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
        string promptMessage = "";
        switch (action)
        {
            case "delete":
                promptMessage = "Type the number of the habit you want to delete from: ";
                break;
            case "insert":
                promptMessage = "Type the number of the habit you want to insert a record for, or type n to create a new habit: ";
                break;
            case "update":
                promptMessage = "Type the number of the habit you want to update a record for: ";
                break;
        }

        if (existingHabits.Length == 0 || existingHabits[0].Contains("sqlite_sequence"))
        {
            Console.WriteLine("No existing habits found in the database.");
            return -1;
        }
        else
        {
            Console.WriteLine("Existing habits in the database:");

            for (int i = 0; i < existingHabits.Length - 1; i++)
            {
                Console.WriteLine($"{i + 1}: {existingHabits[i]}");
            }

            Console.Write(promptMessage);
            string habitChoice = Console.ReadLine().Trim().ToLower();
            while (habitChoice != "n" && (!int.TryParse(habitChoice, out int habitNumber) && action.Equals("insert") || habitNumber < 1 || habitNumber > existingHabits.Length - 1))
            {
                Console.Write("Invalid input. Please type a valid number of an existing habit or n to create a new habit: ");
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

    #endregion // Input Methods

    #region Display Methods
    public static void DisplayMainMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("Type q to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine(lineSpacing);
        Console.Write("Select an option: ");
    }

    public static void DisplayRecords(List<Habit> drinkingWaterRecords)
    {
        Console.WriteLine(lineSpacing);
        foreach (var record in drinkingWaterRecords)
        {
            Console.WriteLine($"ID: {record.Id} | Date: {record.Date.ToString("dd-MM-yyyy")} | Quantity: {record.Quantity} {record.QuantityUnit}");
        }
        Console.WriteLine(lineSpacing);
    }
    #endregion // Display Methods
}

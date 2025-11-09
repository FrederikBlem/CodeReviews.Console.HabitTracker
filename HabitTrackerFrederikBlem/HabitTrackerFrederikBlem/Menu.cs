namespace HabitTracker;
public class Menu
{
    static readonly string lineSpacing = "----------------------------------------";
    static readonly string GetWaterIntakeMessage = "Enter the number of glasses, bottles or other measure of your choice (no decimals allowed).\nType q to return to main menu. ";
    private static List<DrinkingWater> drinkingWaterRecords = new();

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
                drinkingWaterRecords = DatabaseHelper.GetAllRecords();
                if (drinkingWaterRecords.Count == 0)
                {
                    Console.WriteLine("No records to display. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    DisplayRecords(drinkingWaterRecords);
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
            case "2":
                string date = GetDateInput();
                if (date == "q")
                {
                    Console.Clear();
                    return false;
                }
                int quantity = GetNumberInput(GetWaterIntakeMessage);
                if (quantity == -1)
                {
                    Console.Clear();
                    return false;
                }

                bool wasInserted = DatabaseHelper.InsertRecord(date, quantity);
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
                drinkingWaterRecords = DatabaseHelper.GetAllRecords();
                if (drinkingWaterRecords.Count == 0)
                {
                    Console.WriteLine("No records to display or delete. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    DisplayRecords(drinkingWaterRecords);

                    string action = "delete";
                    int idToDelete = GetIdToManipulateInput(action, drinkingWaterRecords);
                    if (idToDelete == -1)
                    {
                        Console.ReadKey();
                        Console.Clear();
                        return false;
                    }

                    bool wasDeleted = DatabaseHelper.DeleteRecord(idToDelete);

                    if (wasDeleted)
                    {
                        Console.WriteLine($"Record {action}d successfully.");
                        drinkingWaterRecords.RemoveAll(x => x.Id == idToDelete);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to {action} record from database.");
                    }

                    return false;
                }
            case "4":
                drinkingWaterRecords = DatabaseHelper.GetAllRecords();
                if (drinkingWaterRecords.Count == 0)
                {
                    Console.WriteLine("No records to display or update. Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
                else
                {
                    DisplayRecords(drinkingWaterRecords);

                    string action = "update";
                    int idToUpdate = GetIdToManipulateInput(action, drinkingWaterRecords);
                    if (idToUpdate == -1)
                    {
                        Console.ReadKey();
                        Console.Clear();
                        return false;
                    }

                    string newDate = GetDateInput();
                    if (newDate == "q")
                    {
                        Console.Clear();
                        return false;
                    }

                    int newQuantity = GetNumberInput(GetWaterIntakeMessage);
                    if (newQuantity == -1)
                    {
                        Console.Clear();
                        return false;
                    }

                    bool wasUpdated = DatabaseHelper.UpdateRecord(idToUpdate, newDate, newQuantity);

                    if (wasUpdated)
                    {
                        Console.WriteLine($"Record {action}d successfully.");
                        DrinkingWater drinkingWaterRecordToUpdate = drinkingWaterRecords.First(x => x.Id == idToUpdate);

                        drinkingWaterRecordToUpdate.Date = DateTime.ParseExact(newDate, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
                        drinkingWaterRecordToUpdate.Quantity = newQuantity;
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

    public static int GetIdToManipulateInput(string actionToDisplay, List<DrinkingWater> givenDrinkingWaterRecords)
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

    public static void DisplayRecords(List<DrinkingWater> drinkingWaterRecords)
    {
        Console.WriteLine(lineSpacing);
        foreach (var record in drinkingWaterRecords)
        {
            Console.WriteLine($"ID: {record.Id} | Date: {record.Date.ToString("dd-MM-yyyy")} | Quantity: {record.Quantity}");
        }
        Console.WriteLine(lineSpacing);
    }
    #endregion // Display Methods
}

using System.Globalization;

namespace HabitTracker;

public class Program
{
    public static void Main(string[] args)
    {       
        CultureInfo.CurrentCulture = new CultureInfo("en-GB");

        string[] tableNames = DatabaseHelper.GetTableNames(writeToConsole: false);
        if (tableNames.Length == 0)
        {
            DatabaseHelper.CreateTableIfNotExists("tracking_habits");
            DateTime currentDate = DateTime.Now;
            DatabaseHelper.InsertRecord(currentDate.ToString("dd-MM-yyyy"), 1, "time(s)", "tracking_habits");
        }

        Console.WriteLine("Welcome to Habit Tracker!");
        Console.WriteLine("-------------------------");
        Console.WriteLine("This app allows you to track your habits by having you enter details about them.");
        Console.WriteLine("You can add, view, update, and delete habit records.");
        Console.WriteLine("The responsibility for keeping the records correct lies on you.");
        Console.WriteLine("-------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        bool closeApp = false;
        while (!closeApp)
        {
            Console.Clear();
            closeApp = Menu.GetUserInputForMainMenu();
        }
        Environment.Exit(0);
    }      
}
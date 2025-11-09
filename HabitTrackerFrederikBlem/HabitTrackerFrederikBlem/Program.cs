namespace HabitTracker;
public class Program
{
    public static void Main(string[] args)
    {       
        DatabaseHelper.CreateTableIfNotExists();

        bool closeApp = false;
        while (!closeApp)
        {
            Console.Clear();
            closeApp = Menu.GetUserInputForMainMenu();
        }
        Environment.Exit(0);
    }      
}
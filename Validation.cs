using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using Spectre.Console;

namespace Lawang.Coding_Tracker;

public class Validation
{
    //Takes user Input -> Validate whether the Inputted time is in "12 hr" format or not
    //(user's input) 12:30 am -> (Valid) Since it is in "12 hr" format it will return DateTime object.
    //(user's input) 14:00 -> (Invalid) Since it is not in "12 hr" format, method will prompt user to input time again.
    public DateTime ValidateUserTime(Rule? rule = null)
    {
        // Define the expected time formats
        DateTime time;
        //loop until user inputs the right format for time or presses "0" to exit to the menu
        while (true)
        {
            if (rule != null)
                AnsiConsole.Write(rule);

            // Create the panel with the time prompt inside
            var panel = new Panel(new Markup("Please enter a [green]time[/] (e.g., 12:30 [cyan]AM[/] or 02:30 [cyan]PM[/]) in 12 hr format:\n\t\t[grey bold](press '0' to go back to Menu.)[/]"))
                .Header("[bold cyan]Time Input[/]", Justify.Center)
                .Padding(1, 1, 1, 1)
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue3);

            // Render the panel
            AnsiConsole.Write(panel);

            //Ask the user for time input as a string
            string input = AnsiConsole.Ask<string>("[green]Time[/]: ");

            // Try parse the user input with the 12 hr format
            if (DateTime.TryParseExact(input, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                break;
            }
            else if (input == "0")
            {
                throw new ExitOutOfOperationException("");
            }
            else
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[red bold]Invalid time format! Please try again.[/]\n");
                AnsiConsole.MarkupLine("[grey](Tips: Don't Forget to add 'AM' or 'PM' after the input time) [/]\n");
            }

        }
        Console.Clear();
        return time;
    }

    //Takes User Input -> Validates the user Input.
    // "two" (user's input) -> (Invalid) Input is string value, so this method will prompt user to enter the correct value.
    // 235 (user's input) -> (Invalid) If there isn't any record in the database with the given record Id,
    //                      User is propmted again to enter the correct value.
    // 0 (user's input) -> (Valid) Exits to Main Menu.
    public CodingSession ValidateCodingSession(List<CodingSession> codingSessions)
    {
        CodingSession? codingSessionRecord;
        do
        {
            //Prompts the user to enter the integer value.
            int userInput = AnsiConsole.Ask<int>("[bold]Enter the Id of the record you want to update: [/]");
            //Returns to the Main menu.
            if (userInput == 0)
            {
                throw new ExitOutOfOperationException("");
            }
            codingSessionRecord = codingSessions.FirstOrDefault(session => session.Id == userInput);
            if (codingSessionRecord == null)
            {
                AnsiConsole.MarkupLine($"[red underline]Record with specified ID {userInput} is not present[/]");
            }
        } while (codingSessionRecord == null);

        return codingSessionRecord;
    }

}

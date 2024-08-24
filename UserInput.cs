using System.Globalization;
using Spectre.Console;

namespace Lawang.Coding_Tracker;

public class UserInput
{
    private Validation _validation;
    private CodingController _codingController;
    public UserInput()
    {
        _validation = new Validation();
        _codingController = new CodingController();
    }
    public void MainMenu()
    {
        bool runApp = true;
        while (runApp)
        {
            Console.Clear();

            //To show the project title "Coding Tracker" in figlet text
            var titlePanel = new Panel(new FigletText("Coding Tracker").Color(Color.Red))
                .BorderColor(Color.Aquamarine3)
                .PadTop(1)
                .PadBottom(1)
                .Header(new PanelHeader("[blue3 bold]APPLICATION[/]"))
                .Border(BoxBorder.Double)
                .Expand();


            AnsiConsole.Write(titlePanel);

            // Selecting the Operation that user want to do in application
            var selectedOption = SelectMenuOption();

            switch (selectedOption.SelectedValue)
            {
                case 1:
                    Console.Clear();
                    List<CodingSession> codingSessions = _codingController.GetAllData();

                    var tableTitle = new Panel(new Markup("[bold underline]CODING - SESSION RECORDS[/]").Centered())
                        .Padding(1, 1, 1, 1)
                        .Border(BoxBorder.Double)
                        .BorderColor(Color.Aqua)
                        .Expand();
                    AnsiConsole.Write(tableTitle);

                    if (codingSessions != null)
                        ViewTable(codingSessions);
                    break;
                case 2:
                    Console.Clear();
                    try
                    {
                        var codingRecord = GetUserInput();
                        int rowsAffected = _codingController.Post(codingRecord);
                        if (rowsAffected == 1)
                        {
                            Panel panel = new Panel(new Markup($"[green bold]{rowsAffected} rows Affected[/]\n[grey](Press 'Enter' to Continue.)[/]"))
                                .Padding(1, 1, 1, 1)
                                .Header("Result")
                                .Border(BoxBorder.Rounded);

                            AnsiConsole.Write(panel);
                            Console.ReadLine();
                        }
                        else
                        {
                            Panel panel = new Panel(new Markup($"[red bold]{rowsAffected} rows Affected[/]\n[grey](Press 'Enter' to Continue.)[/]"))
                                .Padding(1, 1, 1, 1)
                                .Header("Result")
                                .Border(BoxBorder.Rounded);
                            Console.ReadLine();
                        }
                    }
                    catch (ExitOutOfOperationException) { }

                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 0:
                    Console.Clear();
                    Console.WriteLine("Have a Nice Day");
                    runApp = false;
                    break;
            }
        }
    }


    private MenuOption SelectMenuOption()
    {
        AnsiConsole.Write(new Rule("[blue3]Menu Options[/]").LeftJustified().RuleStyle("red"));

        // All the main menu options 
        List<MenuOption> options = new List<MenuOption>()
        {
            new MenuOption() {Display = "View All the Records.", SelectedValue = 1},
            new MenuOption() {Display = "Add a Record.", SelectedValue = 2},
            new MenuOption() {Display = "Update a Record.", SelectedValue = 3},
            new MenuOption() {Display = "Delete a Record.", SelectedValue = 4},
            new MenuOption() {Display = "Show a Report.", SelectedValue = 5},
            new MenuOption() {Display = "Exit the Application.", SelectedValue = 0}


        };

        // for showing the user all the menu options which user can select from.
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOption>()
            .Title("\n[bold cyan underline]What [green]operation[/] do you want to perform?[/]\n")
            .UseConverter<MenuOption>(c => c.Display)
            .MoreChoicesText("[grey](Press 'up' and 'down' key to navigate.[/])")
            .AddChoices(options)
            .HighlightStyle(Color.Blue3)
            .WrapAround()
        );

        return selection;
    }

    private CodingSession GetUserInput()
    {
        var rule = new Rule("[blue3]Start Time[/]").LeftJustified();
        DateTime startTime = GetUserTime(rule);

        rule = new Rule("[blue3]End Time[/]").LeftJustified();
        DateTime endTime = GetUserTime(rule);

        /*
            Usually endTime  is greater than startTime;
            so, error should occur if end time is smaller than startTime
            but, 
            Exception -> User start coding at night 11:00 pm till 2:00 am,
            Solution -> Add 1 day to the endTime if it is smaller than startTime.  
        */
        if (endTime < startTime)
        {
            endTime = endTime.AddDays(1);
        }
        TimeSpan duration = endTime - startTime;

        var codingSession = new CodingSession()
        {
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration,
            //Date on which startTime was intiated
            Date = startTime.Date
        };


        return codingSession;
    }

    private DateTime GetUserTime(Rule? rule = null)
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

    private void ViewTable(List<CodingSession> codingSessions)
    {
        var table = new Table()
             .Border(TableBorder.Rounded)
             .Expand()
             .BorderColor(Color.Aqua);
        
        table.ShowRowSeparators = true;

        table.AddColumns(new TableColumn[]
            {
                 new TableColumn("[green]ID[/]").Centered(),
                 new TableColumn("[cyan3]Start-Time[/]").Centered(),
                 new TableColumn("[deeppink4_2]End-Time[/]").Centered(),
                 new TableColumn("[darkolivegreen2]Duration[/]").Centered()
            });
        
        foreach(var codingSession in codingSessions)
        {
            table.AddRow(
                new Markup($"[green]{codingSession.Id}[/]").Centered(),
                new Markup($"[cyan3]{codingSession.StartTime.ToString("hh:mm tt")}[/]").Centered(),
                new Markup($"[deeppink4_2]{codingSession.EndTime.ToString("hh:mm tt")}[/]").Centered(),
                new Markup($"[darkolivegreen2]{codingSession.Duration.ToString()}[/]").Centered()
            );
        }
        
        AnsiConsole.Write(table);
        AnsiConsole.Markup("[grey](press 'ENTER' to go back to Menu.)[/]");
        Console.ReadLine();

    }
}

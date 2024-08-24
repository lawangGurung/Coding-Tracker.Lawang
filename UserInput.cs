using System.Globalization;
using Spectre.Console;

namespace Lawang.Coding_Tracker;

public class UserInput
{
    private Validation _validation;
    private CodingController _codingController;
    private Visual _visual;
    public UserInput()
    {
        _validation = new Validation();
        _codingController = new CodingController();
        _visual = new Visual();
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
                    ViewAllRecords();

                    AnsiConsole.Markup("[grey](press 'ENTER' to go back to Menu.)[/]");
                    Console.ReadLine();
                    break;
                case 2:
                    Console.Clear();
                    try
                    {
                        var codingRecord = GetUserInput();
                        int rowsAffected = _codingController.Post(codingRecord);

                        //renders result (rows affected by inserting) using Spectre.Console
                        _visual.RenderResult(rowsAffected);
                    }
                    catch (ExitOutOfOperationException) { }

                    break;
                case 3:
                    Console.Clear();
                    try
                    {
                        var codingSessions = _codingController.GetAllData();
                        _visual.RenderTable(codingSessions);
                        AnsiConsole.MarkupLine("[grey](Press '0' to go back to main menu.)[/]");
                        int affectedRow = UpdateRecord();

                        //renders result (rows affected by updating) using Spectre.Console
                        _visual.RenderResult(affectedRow);

                    }
                    catch (ExitOutOfOperationException) { }

                    break;
                case 4:
                    Console.Clear();
                    try
                    {
                        var codingSessions = _codingController.GetAllData();
                        _visual.RenderTable(codingSessions);
                        AnsiConsole.MarkupLine("[grey](Press '0' to go back to main menu.)[/]");
                        int affectedRow = DeleteRecord();

                        //renders result (rows affected by updating) using Spectre.Console
                        _visual.RenderResult(affectedRow);

                    }
                    catch (ExitOutOfOperationException) { }
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
        var rule = new Rule("[steelblue1_1]Start Time[/]").LeftJustified();
        DateTime startTime = _validation.ValidateUserTime(rule);

        rule = new Rule("[steelblue1_1]End Time[/]").LeftJustified();
        DateTime endTime = _validation.ValidateUserTime(rule);

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

    private void ViewAllRecords()
    {
        Console.Clear();
        List<CodingSession> codingSessions = _codingController.GetAllData();

        // title of the codingSessions table
        var tableTitle = new Panel(new Markup("[bold underline] CODING - SESSIONS [/]").Centered())
            .Padding(1, 1, 1, 1)
            .Border(BoxBorder.Double)
            .BorderColor(Color.Aqua)
            .Expand();
        AnsiConsole.Write(tableTitle);

        if (codingSessions != null)
            // this renders the table in console
            _visual.RenderTable(codingSessions);
    }

    private int UpdateRecord()
    {
        var codingSessions = _codingController.GetAllData();
        CodingSession codingSessionToUpdate = _validation.ValidateCodingSession(codingSessions);

        codingSessionToUpdate.StartTime = _validation.ValidateUserTime(new Rule("[steelblue1_1]Update Start-Time[/]").LeftJustified());
        codingSessionToUpdate.EndTime = _validation.ValidateUserTime(new Rule("[steelblue1_1]Update End-Time[/]").LeftJustified());

        if (codingSessionToUpdate.StartTime > codingSessionToUpdate.EndTime)
        {
            codingSessionToUpdate.EndTime = codingSessionToUpdate.EndTime.AddDays(1);
        }

        codingSessionToUpdate.Duration = codingSessionToUpdate.EndTime - codingSessionToUpdate.StartTime;

        return _codingController.Update(codingSessionToUpdate);

    }

    private int DeleteRecord()
    {
        var codingSessions = _codingController.GetAllData();
        CodingSession codingSessionToDelete = _validation.ValidateCodingSession(codingSessions);
        
        return _codingController.Delete(codingSessionToDelete);
    }

}

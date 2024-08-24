using System;
using Spectre.Console;

namespace Lawang.Coding_Tracker;

public class Visual
{
    public void RenderTable(List<CodingSession> codingSessions)
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

        foreach (var codingSession in codingSessions)
        {
            table.AddRow(
                new Markup($"[green]{codingSession.Id}[/]").Centered(),
                new Markup($"[cyan3]{codingSession.StartTime.ToString("hh:mm tt")}[/]").Centered(),
                new Markup($"[deeppink4_2]{codingSession.EndTime.ToString("hh:mm tt")}[/]").Centered(),
                new Markup($"[darkolivegreen2]{codingSession.Duration.ToString()}[/]").Centered()
            );
        }

        AnsiConsole.Write(table);
    }

    public void RenderResult(int rowsAffected)
    {
        if (rowsAffected == 1)
        {
            ShowResult("green", rowsAffected);
        }
        else
        {
           ShowResult("red", rowsAffected); 
        }
    }

    private void ShowResult(string color, int rowsAffected)
    {
        Panel panel = new Panel(new Markup($"[{color} bold]{rowsAffected} rows Affected[/]\n[grey](Press 'Enter' to Continue.)[/]"))
                        .Padding(1, 1, 1, 1)
                        .Header("Result")
                        .Border(BoxBorder.Rounded);

        AnsiConsole.Write(panel);
        Console.ReadLine();
    }
}

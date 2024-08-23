using System.Configuration;
using Lawang.Coding_Tracker;


string cs = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
var dbManager = new DatabaseManager(cs);

dbManager.SeedDataIntoTable();
dbManager.CreateTable();

var userInput = new UserInput();

userInput.MainMenu();



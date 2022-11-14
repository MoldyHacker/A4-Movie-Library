using Spectre.Console;

namespace A4_Movie_Library
{
    public class Menu
    {
        public enum MenuOptions
        {
            Display,
            Search,
            Add,
            Update,
            Delete,
            Exit
        }

        public Menu()
        {

        }

        public MenuOptions ChooseAction()
        {
            var menuOptions = Enum.GetNames(typeof(MenuOptions));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do with the movie library?")
                    .AddChoices(menuOptions));

            return (MenuOptions)Enum.Parse(typeof(MenuOptions), choice);
        }

        public void Exit()
        {
            AnsiConsole.Write(
                new FigletText("Hope you had fun!")
                    .LeftAligned()
                    .Color(Color.Blue));
        }
    }
}


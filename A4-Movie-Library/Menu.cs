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
            Back
        }

        public enum MainMenu
        {
            Movies,
            Users,
            Exit
        }

        public enum MenuUser
        {
            AddUser,
            AddRating,
            Back
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

        public MenuUser ChooseUserAction()
        {
            var menuOptions = Enum.GetNames(typeof(MenuUser));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("User section options")
                    .AddChoices(menuOptions));

            return (MenuUser)Enum.Parse(typeof(MenuUser), choice);
        }


        public MainMenu ChooseInitialAction()
        {
            var menuOptions = Enum.GetNames(typeof(MainMenu));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What section would you like to enter?")
                    .AddChoices(menuOptions));

            return (MainMenu)Enum.Parse(typeof(MainMenu), choice);
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


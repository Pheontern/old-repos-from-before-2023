using System;
using static Main.ConsoleUtils;

namespace Main
{
    
    public class Program
    {

        //Main-metod.
        public static void Main()
        {

            Console.SetWindowSize(ConsoleUtils.width, ConsoleUtils.height);//Skriver ConsoleUtils i onödan för att förtydliga var variablerna kommer ifrån.

            MainMenu();

        }

        //Startmenyn med alla minigames.
        public static void MainMenu()
        {

            while (true)
            {
                //Kör menysystemet och sparar användarens val.
                uint menu = Menu(50, false, "Välj ett spel:", "Gissa talet", "Reaktionstest", "Snake", "Avsluta");

                //Jämför och aktiverar användarens input i menyn.
                switch (menu)
                {
                    case 1:
                        Games.GuessNumber();
                        break;
                    case 2:
                        Games.ReactionTest();
                        break;
                    case 3:
                        Games.SnakeGame();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                }
            }

        }

    }

}

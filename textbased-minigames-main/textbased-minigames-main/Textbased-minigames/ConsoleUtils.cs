using System;
using System.Threading;
using System.Text;

namespace Main
{
    
    public static class ConsoleUtils
    {

        //Storlek på konsolfönstret.
        public static int width = 100, height = 40;

        //Startar en animerad meny med valt antal alternativ.
        public static uint Menu(int aniSpeed, bool reverse, string title, params string[] menuChoices)
        {

            //Antal alternativ är antal element i menuChoices arrayen.
            int numAlts = menuChoices.Length;

            uint menu;

            //Loop med hela mekanismen.
            while (true)
            {

                Console.Clear();
                
                //Hela menytexten.
                string text = title;
                
                //Konkatenerar hela menytexten med varje alternativ. 
                for (int i = 0; i < numAlts; i++)
                {
                    
                    text += string.Format("\n{0}, {1}.", i + 1, menuChoices[i]);

                }
                text += "\n";

                //Skriver ut menyn.
                AniWrite(text, aniSpeed, reverse);

                //Läser användarens input och testar om valet är giltigt.
                string input = Console.ReadKey(true).KeyChar.ToString();
                if (uint.TryParse(input, out menu) && menu > 0 && menu <= numAlts) break;

                //Annars rättar den användaren och börjar om menyn.
                else
                {
                    AniWrite("Skriv endast ett tal 1 - " + numAlts + ".", 30);
                    Thread.Sleep(1000);
                }

            }

            return menu;

        }

        //Skriver ut text animerat.
        public static void AniWrite(string text, int time = 30/*tid i ms mellan varje tecken*/, bool reverse = true, int charStart = 1/*Tecknet att starta med,*/, bool cursor = true)
        {

            //Om tiden inte är noll skrivs texten ut animerat.
            if (time != 0)
            {

                //Rensar input för att texten inte ska kunna skippas instant.
                ClearInputBuffer();

                //Konsolmarkörens position från start. Används för att börja skriva på rätt ställe igen efter att konsolytan rensats. 
                int[] cursorPos = GetCursorPosition();

                //Antal rader och största antalet tecken på en rad. Används för att endast rensa så stor yta som krävs vid utskrift.
                int[] linesAndChars = LinesAndChars(text);

                //Om texten ska skrivas ut baklänges.
                if (reverse == true)
                {

                    //Nuvarande text som skrivs ut i ett "steg".
                    string current;

                    //Tar bort rätt antal tecken från texten, skriver ut, väntar och rensar. Tar sedan bort ett tecken mindre o.s.v. Samt kontrollerar om animationen ska skippas.
                    for (int i = text.Length - charStart; i > 0; i--)
                    {

                        current = text;

                        current = current.Remove(0, i);

                        Console.Write(current);

                        if (cursor == true) Console.CursorVisible = true;

                        Thread.Sleep(time);

                        if (cursor == true) Console.CursorVisible = false;

                        NoFlickerClear(linesAndChars, cursorPos);

                        if (Console.KeyAvailable) break;

                    }

                }

                //Annars framlänges.
                else
                {

                    //Loopar igenom varje tecken i stringen. Skriver ut ett tecken, väntar i ett antal ms och kontrollerar om användaren vill skippa animationen.
                    foreach (char i in text)
                    {
                        Console.Write(i);

                        Thread.Sleep(time);

                        if (Console.KeyAvailable) break;
                    }

                    if (cursor == true) Console.CursorVisible = false;

                    Console.SetCursorPosition(cursorPos[0], cursorPos[1]);

                }

            }

            //Annars normalt. Detta kommer alltid ske för att skriva ut sista steget i animationerna samt för att kunna skippa dem.
            Console.Write(text);

            if (cursor == true) Console.CursorVisible = true;

            //Rensar lagrad input så att inget som sparats under animationen räknas.
            ClearInputBuffer();

        }

        //Tömmer en yta av konsolen utan att blinka skärmen.
        public static void NoFlickerClear(int[] linesAndChars, int[] cursorPos)
        {

            //Börjar cleara från korrekt position.
            Console.SetCursorPosition(cursorPos[0], cursorPos[1]);

            //Konstruerar en rad fylld av korrekt antal mellanrum.
            StringBuilder emptyLine = new StringBuilder();
            emptyLine.Append(' ', linesAndChars[1]);
            emptyLine.AppendLine();
            
            //Sätter ihop korrekt antal rader.
            string emptyLineStr = emptyLine.ToString();
            string whitespace = "";
            for (int i = 0; i < linesAndChars[0]; i++) whitespace += emptyLineStr;

            //Skriver ut mellanrummen (rensar) och sätter tillbaka markören till ursprunglig position.
            Console.Write(whitespace);
            Console.SetCursorPosition(cursorPos[0], cursorPos[1]);

        }

        //Hämtar och returnerar en array med konsol-markörens position i x- och y-led.
        public static int[] GetCursorPosition()
        {

            int[] cursorPos = { 0, 0 };

            //Sparar koordinater för markören i arrayen.
            cursorPos[0] = Console.CursorLeft;
            cursorPos[1] = Console.CursorTop;

            return cursorPos;

        }

        //Räknar antalet rader och tecken på längsta raden i en given string.
        public static int[] LinesAndChars(string text)
        {

            string[] lines = text.Split('\n');

            int linesCount = lines.Length;

            int charsCount = 0;

            foreach (string line in lines) if (charsCount < line.Length) charsCount = line.Length;

            return new int[2] { linesCount, charsCount };

        }

        //Rensar input som användaren givit medan text skrivits ut t.ex.
        public static void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        //Vänder en string baklänges. Används inte i programmet.
        public static string Reverse(string text)
        {

            char[] input = text.ToCharArray();
            Array.Reverse(input);
            return new String(input);

        }

        //Animerad banners, ganska oanvändbar eftersom man ändå inte kan skriva mer än en del åt gången... Används inte i programmet.
        public static void AnimatedBanner(string text)
        {

            int[] origin = { 0, 0 };

            int[] linesAndChars = LinesAndChars(text);

            int width = ConsoleUtils.width, height = ConsoleUtils.height;
            Console.SetWindowSize(width, height);

            StringBuilder bannerX = new StringBuilder();

            int space = width - text.Length;

            bannerX.Append(text);

            if (space >= 0) bannerX.Append(' ', space);

            string banner = bannerX.ToString();
            banner += text;

            bool count = false;
            while (true)
            {

                NoFlickerClear(linesAndChars, origin);

                if (count == false)
                {
                    Console.CursorVisible = false;
                    AniWrite(banner, 30, true, text.Length, false);
                    count = true;
                }

                else
                {
                    Console.Write(banner.Remove(text.Length));
                    count = false;
                }

            }

        }

    }

}

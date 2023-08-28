using System;
using System.Threading;
using System.Diagnostics;
using static Main.ConsoleUtils;

namespace Main
{

    public static class Games
    {

        //Instansierar Stopwatch-klass, skapar alltså ett objekt som används för tidtagning.
        static readonly Stopwatch timer = new Stopwatch();

        static readonly Random rand = new Random(31415);

        //Gissa talet.
        public static void GuessNumber()
        {

            Console.Clear();
            AniWrite("Välkommen till gissa talet.");
            Thread.Sleep(2000);

            //Variabel som kontrollerar hur stort spann siffran som ska gissas får vara i.
            int guessInside;

            //Läser input och sparar guessInside.
            while (true)
            {
                Console.Clear();
                AniWrite("Programmet väljer ett tal mellan 0 och X som du ska gissa, välj vad X ska vara: ");

                //Läser input och testar om det är ett giltigt tal.
                if (int.TryParse(Console.ReadLine(), out guessInside) && guessInside <= 10000 && guessInside > 0) break;

                //Annars rättar den användaren och börjar om.
                else
                {
                    AniWrite("Bara ett tal 0 < X <= 10000.", 10);
                    Thread.Sleep(1000);
                }
                
            }

            int numToGuess = rand.Next(guessInside);
            uint guess;

            //Loop för hela gissningssystemet.
            while (true)
            {

                Thread.Sleep(1000);
                ClearInputBuffer();

                //Läser input och testar om det är en giltig gissning.
                while (true)
                {
                    Console.Clear();
                    AniWrite("Gissa! ", 0);

                    if (uint.TryParse(Console.ReadLine(), out guess) && (guess < guessInside) && (guess > 0)) break;

                    else
                    {
                        AniWrite("Endast siffror inom ditt val! ", 20);
                        Thread.Sleep(1000);
                    }
                }


                //Testar gissningen och svarar högre eller lägre tills korrekt gissning.
                if (guess == numToGuess)
                {
                    AniWrite("Bra gjort!\n\n", 200);
                    AniWrite("Tryck på space för att återgå till huvudmenyn.\n");
                    Console.ReadKey();
                    break;
                }
                else if (guess < numToGuess)
                {
                    AniWrite("Högre! ", 0);
                    guess = 0;
                }
                else
                {
                    AniWrite("Lägre! ", 0);
                    guess = 0;
                }
            
            }

        }

        //Reaktionstest.
        public static void ReactionTest()
        {

            Console.Clear();
            AniWrite("Välkommen till reaktionstestet.");
            Thread.Sleep(2000);

            //Startar meny som frågar om reaktionstest baserat på hörsel, syn eller både och.
            uint menu = Menu(35, false, "Välj testtyp:", "Hörsel", "Syn", "Både och", "Avsluta");

            //Avslutar om användaren väljer det.
            if (menu == 4) Environment.Exit(0);

            //sessionRecord sparar rekordet från den pågående sessionen, i framtiden kanske jag ska spara ner resultatet i en text-fil eller något.
            int sessionRecord = 999999999;

            //Loop för hela reaktionssystemet. 
            while (true)
            {

                int waitTime = 1000 * rand.Next(3, 17);

                Console.Clear();

                //Skriver ut nuvarande sessionsrekord.
                if (sessionRecord != 999999999)
                {
                    AniWrite("Ditt rekord för den här sessionen: " + sessionRecord + " ms.\n\n", 0);
                }

                //Spelet startar.
                AniWrite("Vänta på ditt valda alternativ, tryck sedan på space så snabbt du bara kan!\n", 15, false);

                Thread.Sleep(waitTime);

                //Kollar om spelaren trycker för tidigt, visas dock inte förrän väntetiden är slut.
                if (Console.KeyAvailable) AniWrite("För tidigt!\n\n");
                
                //Testar användarens reaktionsval och kör antingen ljud, färg eller båda.
                else
                {

                    ClearInputBuffer();

                    Console.BackgroundColor = ConsoleColor.Green;

                    switch (menu)
                    {

                        case 1:
                            Console.Beep();
                            break;

                        case 2:
                            Console.Clear();
                            break;

                        case 3:
                            Console.Clear();
                            Console.Beep();
                            break;

                    }

                    //Tar tid och sparar resultatet.
                    timer.Start();
                    Console.ReadKey(true);
                    timer.Stop();
                    Console.ResetColor();
                    Console.Clear();
                    TimeSpan ts = timer.Elapsed;
                    timer.Reset();


                    Thread.Sleep(1000);

                    //Skriver ut tiden och testar om det är ett sessionsrekord.
                    AniWrite(ts.Milliseconds.ToString() + " ms\n", 500, false);

                    if (sessionRecord > ts.Milliseconds)
                    {

                        AniWrite("Nytt rekord för sessionen!\n\n", 60);
                        sessionRecord = ts.Milliseconds;

                    }

                }

                //Spelaren får chansen att avsluta eller fortsätta.
                AniWrite("Tryck på space för att försöka igen eller på X för att avsluta.\n", 20);

                if (Console.ReadKey(true).KeyChar == 'x') Environment.Exit(0);
                    

            }

        }

        //Snake.
        public static void SnakeGame()
        {

            //Metod som körs en gång.
            Snake.Initialize();

            //Oändlig loop som kör metoden Update om och om igen. Tills den bryts.
            while (true)
            {
                Snake.Update();
            }

        }

    }

}

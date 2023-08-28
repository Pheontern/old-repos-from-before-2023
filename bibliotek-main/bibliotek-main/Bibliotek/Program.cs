using System;
using static Bibliotek.Library;

namespace Bibliotek
{
    class Program
    {

        static void Main(string[] args)
        {
            //Repeterar hela programmet, menyn.
            while (true)
            {

                //Använder metod för att skriva ut menyalternativ och läsa in användarens val.
                uint menu = ConsoleUtils.Menu(0, false, "Välkommen till biblioteket! (inuti menyerna kan du alltid skriva \"bryt\" för att återgå hit)", "Sök, låna och återlämna", "Lägg till bok", "Ta bort bok", "Ta bort författare (och alla dennes böcker)", "Ändra en boks namn eller författare", "Lista alla böcker", "Avsluta programmet");

                Console.Clear();

                //Kontrollerar menyalternativ och kör rätt metoder.
                switch (menu)
                {

                    //Sök, låna och återlämna.
                    case 1:
                        switch (Search())
                        {

                            case "noResults":
                                Console.WriteLine("Inga sökresultat.");
                                break;

                            case "borrowed":
                                Console.WriteLine("Varsågod! Kom ihåg att lämna tillbaka den i tid.");
                                break;

                            case "incorrectInput":
                                Console.WriteLine("Den boken fanns inte i dina sökresultat, stavfel?");
                                break;

                        }
                        break;

                    //Lägg till bok.
                    case 2:
                        Console.WriteLine("Författare som redan finns i systemet: \n");
                        ListAuthors(libData);
                        if(AddBook()) Console.WriteLine("Tillagd!");
                        break;

                    //Ta bort bok.
                    case 3:
                        Console.WriteLine("Böcker som finns i systemet: \n");
                        ListBooks(libData);
                        if (RemoveBook()) Console.WriteLine("Borttagen!");
                        else Console.WriteLine("Den här boken finns inte i systemet.");
                        break;

                    //Ta bort författare och alla dennes böcker.
                    case 4:
                        Console.WriteLine("Författare som finns i systemet: \n");
                        ListAuthors(libData);
                        if (RemoveAuthor()) Console.WriteLine("Författaren och alla dennes böcker har tagits bort.");
                        else Console.WriteLine("Den här författaren finns inte i systemet.");
                        break;

                    case 5:
                        ListBooks(libData);
                        if (!ModifyBook()) Console.WriteLine("Något gick fel, skrev du fel någonstans? ");
                        else Console.WriteLine("Ändring genomförd! ");
                        break;

                    //Lista alla författare och böcker.
                    case 6:
                        List(libData);
                        break;

                    //Spara datan och stäng programmet.
                    case 7:
                        SaveData();
                        Environment.Exit(0);
                        break;

                }

                Console.WriteLine("\nEnter för att återvända till menyn (och spara eventuella ändringar).");
                Console.ReadKey();
                SaveData();

            }

        }

    }
    
}

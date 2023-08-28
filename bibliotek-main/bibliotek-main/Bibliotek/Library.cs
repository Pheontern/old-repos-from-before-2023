using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Bibliotek
{
    static class Library
    {

        //File-arrayen innehåller ett string-element för varje rad i bokregisterfilen och läses in i Library-konstruktorn.
        //Filen måste vara formaterad exakt rätt, inga extra rader exempelvis.
        static readonly string[] file;

        //Lista av författarobjekt, eftersom författarobjekten innehåller bok-listor innehåller denna lista all data som krävs för biblioteket.
        public static List<Author> libData = new List<Author>();

        //Load Library-konstruktorn, en statisk konstruktor kallas på när något i klassen kallas på tror jag.
        //Laddar in information från bokregisterfilen och sparar i libData-listan.
        static Library() 
        {

            if (File.Exists("../../../../bokregisterhär/bokregister.txt"))
            {
                file = File.ReadAllLines("../../../../bokregisterhär/bokregister.txt", Encoding.UTF8);
            }
            //Om Filen inte finns läses den in som tom, den skapas när SaveData() körs.
            else file = new string[0];
                

            Author tempAuthor = null;
            for (int i = 0; i < file.Length; i++)
            {
                
                if (file[i][0] == '$') 
                { 
                    tempAuthor = new Author(file[i][2..(file[i].Length - 1)]); 
                    libData.Add(tempAuthor);
                }

                else
                {
                    if (tempAuthor == null) throw new ArgumentException("Felaktigt formaterat bokregister. Det måste börja med en författare som indikeras med '$'.");

                    bool inStock = false;
                    if (file[i][0] == '+') inStock = true;

                    Book tempBook = new Book(file[i][2..(file[i].Length - 1)], tempAuthor.name, inStock);
                    tempAuthor.books.Add(tempBook);
                }

            }

        }

        //Listar alla författare och böcker.
        public static void List(List<Author> toList) 
        {

            string toPrint = "";
            foreach(Author auth in toList)
            {

                toPrint += $"{auth.name} (författare)\n";

                foreach(Book book in auth.books)
                {

                    string inStock = "Utlånad";
                    if (book.inStock) inStock = "Ledig";

                    toPrint += $"\"{book.title}\" - {inStock}\n";

                }

                toPrint += '\n';

            }

            Console.Write(toPrint);
        
        }

        //Listar alla författare.
        public static void ListAuthors(List<Author> toList)
        {

            foreach(Author auth in toList) Console.WriteLine($"{auth.name}");
            Console.WriteLine();

        }

        //Listar alla böcker.
        public static void ListBooks(List<Author> toList)
        {

            foreach(Author auth in toList)
            {

                foreach(Book book in auth.books)
                {

                    Console.WriteLine($"{book.title}");

                }

            }

            Console.WriteLine();

        }

        //Läser in namn och författare från användaren och returnerar ett bokobjekt.
        public static Book InputBook(bool inStock = true)
        {

            string author = ConsoleUtils.ReadInput("Vem är författaren? ");
            if (author == "bryt") return new Book("bryt", "bryt", false);
            string title = ConsoleUtils.ReadInput("Vad heter boken? ");
            if (title == "bryt") return new Book("bryt", "bryt", false);
            return new Book(title, author, inStock);

        }

        //Sparar ner all data i registertextfilen.
        public static void SaveData()
        {

            string newFile = "";

            foreach (Author author in libData)
            {

                newFile += $"$\"{author.name}\"\n";

                foreach (Book book in author.books)
                {

                    if (book.inStock) newFile += $"+\"{book.title}\"\n";
                    else newFile += $"-\"{book.title}\"\n";

                }

            }

            File.WriteAllText("../../../../bokregisterhär/bokregister.txt", newFile[0..(newFile.Length - 1)], Encoding.UTF8);

        }

        //Lägger till bok till specifik författare.
        public static bool AddBook()
        {

            Console.WriteLine("Om författaren inte finns i systemet läggs vederbörande också till.");
            Console.WriteLine("Lägg till bok: \n");

            Book toAdd = InputBook();
            if (toAdd.title == "bryt") return false;
            bool checker = false;
            foreach (Author author in libData)
            {
                if (author.name == toAdd.author)
                {
                    author.books.Add(toAdd);
                    checker = true;
                    break;
                }
            }
            if (!checker)
            {
                Author author = new Author(toAdd.author);
                author.books.Add(toAdd);
                libData.Add(author);
            }

            return true;

        }

        //Tar bort bok från specifik författare.
        public static bool RemoveBook()
        {

            string toRemove = ConsoleUtils.ReadInput("Vad heter boken du vill ta bort? ");
            if (toRemove == "bryt") return false;

            foreach (Author auth in libData)
            {

                foreach (Book book in auth.books)
                {

                    if (toRemove == book.title)
                    {

                        auth.books.Remove(book);
                        return true;

                    }

                }

            }

            return false;

        }

        //Tar bort författare ur libData, listan med all data alltså.
        public static bool RemoveAuthor()
        {

            string toRemove = ConsoleUtils.ReadInput("Vem är författaren du vill ta bort? ");
            if (toRemove == "bryt") return false;

            foreach (Author auth in libData)
            {

                if (toRemove == auth.name)
                {

                    libData.Remove(auth);
                    return true;

                }

            }

            return false;

        }

        //Söker efter författare och böcker.
        public static string Search()
        {

            Console.WriteLine("Den här funktionen letar igenom hela biblioteket utifrån dina sökord, om en författare eller bok innehåller din söktext listas den.\n");
            string query = ConsoleUtils.ReadInput("Söktext: ").ToLower();
            if (query == "bryt") return "bryt";
            Console.WriteLine();

            List<Author> authorsToList = new List<Author>();
            foreach(Author auth in libData)
            {

                if (auth.name.ToLower().Contains(query)) authorsToList.Add(auth);
                else 
                {

                    Author tempAuthor = new Author(auth.name);
                    foreach (Book book in auth.books)
                    {
                        
                        if (book.title.ToLower().Contains(query))
                        {
                            
                            tempAuthor.books.Add(book);
                            if (!authorsToList.Contains(tempAuthor)) authorsToList.Add(tempAuthor);

                        }

                    }

                }

            }

            if (authorsToList.Count > 0) List(authorsToList);
            else return "noResults";

            string nameToBorrow = ConsoleUtils.ReadInput("Ange det exakta namnet på boken du vill låna eller återlämna (ur sökresultat): \n");
            if (nameToBorrow == "bryt") return "bryt";
            Book tempBook = null;
            foreach (Author auth in authorsToList)
            {

                foreach (Book book in auth.books)
                {

                    if (book.title == nameToBorrow) 
                    {
                        if (book.inStock == true) { book.inStock = false; return "borrowed"; }
                        tempBook = book;
                        goto Returning;//Enligt de flesta är det ok att använda gotos för att bryta ut ur nestlade loopar.
                    }

                }

            }

            Returning:
            if (tempBook != null) { ReturnBook(tempBook); return "returned"; }

            return "incorrectInput";

        }

        //Återlämnar bok, ändrar dess inStock-värde till true (den registreras alltså som i lager).
        public static void ReturnBook(Book toReturn)
        {

            string input = ConsoleUtils.ReadInput("\nDen här boken är redan utlånad.\nÄr det du som har den, och i så fall, vill du lämna tillbaka den? ja/nej eller j/n ");
            if (input == "ja" || input == "j") { Console.WriteLine("Tack så mycket!"); toReturn.inStock = true; }


        }

        //Ändrar en boks namn eller författare.
        //Den här funktionen är antagligen skriven väldigt ineffektivt och krångligt eftersom jag stressade ihop den på några minuter. Bara för att nå kravet i uppgiften som las till i efterhand.
        public static bool ModifyBook()
        {
            Author tempAuth = null;
            Book tempBook = null;
            bool tempInStock = true;
            string toChange = ConsoleUtils.ReadInput("Vad heter boken du vill ändra exakt? ");
            string whatChange = ConsoleUtils.ReadInput("Vill du ändra titel eller författare (svara med författare/titel)? ");
            if (whatChange == "titel")
            {

                Console.Clear();
                foreach (Author auth in libData)
                {

                    foreach (Book book in auth.books)
                    {

                        if (book.title == toChange)
                        {

                            string newTitle = ConsoleUtils.ReadInput("Vad ska boken heta istället exakt? ");
                            book.title = newTitle;
                            return true;

                        }

                    }

                }
                return false;
            }
            else if (whatChange == "författare")
            {
                ListAuthors(libData);
                string newAuthor = ConsoleUtils.ReadInput("Vad heter den nya författaren exakt (välj ur listan ovan)? ");
                foreach (Author auth in libData)
                {
                    foreach (Book book in auth.books)
                    {
                        if (book.title == toChange)
                        {
                            tempInStock = book.inStock;
                            tempAuth = auth;
                            tempBook = book;
                        }
                    }
                }
                if (tempAuth != null)
                {
                    tempAuth.books.Remove(tempBook);
                }
                
                foreach (Author auth in libData)
                {
                    if (auth.name == newAuthor)
                    {
                        auth.books.Add(new Book(toChange, auth.name, tempInStock));
                        return true;
                    }
                }

                return false;


            }
            return false;

        }

    }
}
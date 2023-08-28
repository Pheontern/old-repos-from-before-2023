using System.Collections.Generic;

namespace Bibliotek
{
    class Author
    {
        
        public string name;
        //Alla böcker författaren har skrivit.
        public List<Book> books = new List<Book>();

        public Author(string name) { this.name = name; }

    }
}

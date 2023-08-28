
namespace Bibliotek
{
    class Book
    {

        public string title;
        public string author;
        public bool inStock;

        public Book(string title, string author, bool inStock)
        {
            this.title = title;
            this.author = author;
            this.inStock = inStock;
        }

    }
}

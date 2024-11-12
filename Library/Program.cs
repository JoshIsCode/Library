using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Book b = Book.EpubToBook(@"C:\Users\s123325\source\repos\Library\Library\books\harry-potter-book-1.epub");
        }
    }
}

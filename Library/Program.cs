using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Program
    {

        static string[] title =
        {
            @"  _____            _ _       ",
            @"  \_   \__ _ _ __ (_| |_ ___ ",
            @"   / /\/ _` | '_ \| | __/ _ \",
            @"/\/ /_| (_| | | | | | ||  __/",
            @"\____/ \__, |_| |_|_|\__\___|",
            @"       |___/                 "
        };
        static void Main(string[] args)
        {

            Book b = Book.JSONToBook(@"C:\Users\s123325\source\repos\Library\Library\books\harry-potter-book-1-updated.json");
            Console.WriteLine(b);
            BookReader reader = new BookReader();
            reader.open(b);

            drawPageBorder(Console.WindowWidth, Console.WindowHeight);

            Console.SetCursorPosition(2, 1);

            foreach (string line in title)
            {
                printInLine(line);
            }
            printInLine(new string('-', Console.WindowWidth - 4));

            int tileWidth = Console.WindowWidth / 6;
            int tileHeight = (int)(tileWidth*0.6);

            List<BookCard> bookCards = new List<BookCard>();
            bookCards.Add(new BookCard(b, tileWidth, tileHeight));

            foreach (BookCard bookCard in bookCards)
            {
                printInLine(bookCard.ToString());
            }



            

            Console.ReadKey(true);

            //Console.

            
        }

        private static void printInLine(string text)
        {
            int left = Console.CursorLeft;
            foreach (string line in text.Split('\n'))
            {
                Console.WriteLine(line);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            
        }

        private static void drawPageBorder(int w, int h)
        {
            char[] border = { '─', '│', '┌', '┐', '└', '┘' };
            int left = 0;

            Console.SetCursorPosition(left, Console.CursorTop);
            Console.WriteLine(border[2] + new string(border[0], w - 2) + border[3]);
            Console.SetCursorPosition(left, Console.CursorTop);
            for (int i = 0; i < h - 2; i++)
            {
                Console.WriteLine(border[1] + new string(' ', w - 2) + border[1]);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            Console.Write(border[4] + new string(border[0], w - 2) + border[5]);

        }
    }
}

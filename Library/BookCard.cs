using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Class that builds and dispalys the book tiles for the menu
// Dynamic display
// - Change the widht or height
namespace Library
{
    internal class BookCard
    {

        private char[] border = { '─', '│', '┌', '┐', '└', '┘' };

        public Book Book;

        public bool highlight = false;

        public int Width {
            get { return width; }
            set
            {
                if (value > 4)
                    width = value;
            }
        }
        private int width;
        public int Height {
            get { return height; }
            set
            {
                if (value > 4)
                    height = value;
            }
        }
        public int height;

        public BookCard(Book book, int width, int height) {
            this.Book = book;
            this.Width = width;
            this.Height = height;
        }

        public override string ToString() {
            // add 

            string output =  "" + border[2] + new string(border[0], Width-2) + border[3] + "\n";

            output += Book.Title + "\n";
            for (int i = 0; i < Height-4; i++)
            {
                output += "" + border[1] + new string(' ', Width - 2) + border[1] + "\n";
            }
            output += Book.Author + "\n"; 
            output += "" + border[4] + new string(border[0], Width - 2) + border[5] + "\n";

            return output;

        }

    }
}
/*
--------------
|Harry Potter|
|  Sorcerers |
|    Stone   |
|            |
| JK Rowling |
|            |
--------------
*/
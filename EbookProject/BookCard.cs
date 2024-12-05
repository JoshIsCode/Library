using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Class that builds and dispalys the book tiles for the menu
// Dynamic display
// - Change the widht or height
namespace EBookProject
{
    internal class BookCard
    {

        private char[] border = { '─', '│', '┌', '┐', '└', '┘' };
        private char[] highLightedBorder = {'━','┃','█','█','█','█'};

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

        public string Display(bool selected = false) {

            highlight = selected;
            // add 

            char[] border = highlight?this.highLightedBorder : this.border;

            string output =  "" + border[2] + new string(border[0], Width-2) + border[3] + "\n";

            
            for (int i = 0; i < Height-2; i++)
            {
                if (i == 1){
                    output += createLine(Book.Title.Substring(0,Math.Min(width-2,Book.Title.Length))) + "\n";
                } else if (i == Height-4){
                    output += createLine(Book.Author) + "\n";
                } else if (i == Height-3){
                    output += createLine(Book.Genre) + "\n";
                } else {
                    output += "" + border[1] + new string(' ', Width - 2) + border[1] + "\n";
                }
                
            }
            
            output += "" + border[4] + new string(border[0], Width - 2) + border[5];

            return output;

        }

        private string createLine(string line){
            char[] border = highlight?this.highLightedBorder : this.border;
            return border[1] + new string(' ',Math.Max((width-2-line.Length)/2,0)) + line + new string(' ',Math.Max((width-1-line.Length)/2,0)) + border[1];
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
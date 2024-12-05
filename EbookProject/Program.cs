using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EBookProject
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

        public static List<BookCard> bookCards;

        static int tileWidth;
        static int tileHeight;

        static int selected = 0;
        static void Main(string[] args)
        {

            Book b = Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/Book1.json");
            Console.WriteLine(b);

            tileWidth = Console.WindowWidth / 6;
            tileHeight = (int)(tileWidth*0.6);

            bookCards = new List<BookCard>();
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/Book1.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Last_Leaf.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Mysterious_Lighthouse.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Clockmaker's_Apprentice.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Silent_Watchman.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Time_Traveler's_Diary.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Whispering_Forest.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Shadow_in_the_Library.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Forgotten_Kingdom.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(@"/Users/joshkarr/Desktop/code/C# Projects/projects/EbookProject/books/The_Secret_Beneath_the_Lake.json"),tileWidth,tileHeight));

            

            menuPage();

            

            

            Console.ReadKey(true);

            //Console.

            
        }

        private static void menuPage(){
            drawPageBorder(Console.WindowWidth, Console.WindowHeight);

            Console.SetCursorPosition(2, 1);

            foreach (string line in title)
            {
                printInLine(line);
            }
            printInLine(new string('-', Console.WindowWidth - 4));

            Console.CursorVisible = true;
            

            List<List<int>> grid = new List<List<int>>();
            bool running = true;
            int firstTop = Console.CursorTop;
            
            int left = Console.CursorLeft;

            int selectedR = 0;
            int selectedC = 0;

            int width = Console.WindowWidth;
            int hieght = Console.WindowHeight;

            List<int> row = new List<int>();
            for (int i = 0; i < bookCards.Count; i++)
            {
                row.Add(i);
                if ((row.Count+1)*tileWidth > Console.WindowWidth-3){
                    grid.Add(row);
                    row = new List<int>();
                }
            }
            if (row.Count > 0) {
                grid.Add(row);
            }

            selectedR = selected/grid.Count;
            selectedC = selected % grid[0].Count;

            while (running){
                int top = firstTop;
                if (Console.WindowHeight != hieght || Console.WindowWidth != width){
                    menuPage();
                    return;
                }
                // Look if resize is needed:

                
                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++){
                        printInLine(bookCards[i*grid[i].Count+j].Display(i*grid[i].Count+j==selected), true);
                        if (j < grid[i].Count - 1)
                            Console.SetCursorPosition(Console.CursorLeft, top);
                    }
                    Console.WriteLine();
                    Console.SetCursorPosition(left, Console.CursorTop);
                    top = Console.CursorTop;
                }
                

                switch(Console.ReadKey(true).Key){
                    case ConsoleKey.Escape: 
                        return;
                    case ConsoleKey.LeftArrow: 
                        selected --;
                        if ((selected+grid[0].Count)%grid[0].Count==grid[0].Count-1){
                            selected+=grid[0].Count;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        selected++;
                        if (selected%grid[0].Count==0){
                            selected-=grid[0].Count;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        selected-= grid[0].Count;
                        if (selected < 0){
                            selected = grid[0].Count*(grid.Count-1)+selected%grid[0].Count;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        selected+= grid[0].Count;
                        if (selected >= grid.Count*grid[0].Count){
                            selected %= grid[0].Count;
                        }
                        break;
                    case ConsoleKey.Enter:
                        BookReader reader = new BookReader();
                        reader.open(bookCards[selected].Book);
                        return;
                }
            
                Console.SetCursorPosition(left,firstTop);
            }
        }

        private static void printInLine(string text, bool stayLast = false)
        {
            int left = Console.CursorLeft;
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {   
                if (stayLast && i == lines.Length - 1){
                    Console.Write(lines[i]);
                    
                } else {
                    Console.WriteLine(lines[i]);
                    Console.SetCursorPosition(left, Console.CursorTop);
                }
                
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

/*
 *  Ignite Ebook Platform
 *  By Joshua Karr
 *  
 *  This quick prototype console application is an ebook reader similar to the Amazon Kindle. 
 *  
 *  Current Features:
 *   - Adaptive sized menu with genreated Book Covers.
 *   - Adaptive sized book reader that breaks chapters into pages and allows you to read the book
 *   
 *   How to Use:
 *    - Run the script (if the window is to small then you will have to resize it and press any arrow key;
 *    - Use arrow keys to select different books.
 *    - Cick escape to close the program
 *    - Click enter to open the book in the reader.
 *    - Click left or right to flip through pages
 *    - Click escape to close out of book reader and return to menu
 *   
 *   Program class:
 *    - Main functions and runs the menu application.
 *    - Imports books from a directory formated as JSON
 *    
 *   Book Class:
 *    - Holds the data about the book that is stored in the json files
 *    - Holds chapter class which organises the paragraphs and chapter name
 *   
 *   BookCard Class:
 *    - Generates the graphical text book covers that are in the menu and stores the book that they correspond to
 *    
 *    BookReader Class:
 *     - Takes a book instance and create a graphical interface to read the book
 *     - Splits the chaptres up into book and handles inputs
 */

namespace EBookProject
{
    internal class Program
    {

        private static string[] title =
        {
            @"  _____            _ _       ",
            @"  \_   \__ _ _ __ (_| |_ ___ ",
            @"   / /\/ _` | '_ \| | __/ _ \",
            @"/\/ /_| (_| | | | | | ||  __/",
            @"\____/ \__, |_| |_|_|\__\___|",
            @"       |___/                 "
        };

        public static char[] border = { '─', '│', '┌', '┐', '└', '┘' };

        private static List<BookCard> bookCards;

        private static int tileWidth;
        private static int tileHeight;

        static int selected = 0;
        static void Main(string[] args)
        {
            string path = @"C:\Users\s123325\source\repos\EBookProject\EbookProject\books\";
            Encoding e = Encoding.GetEncoding("iso-8859-1");
            Console.OutputEncoding = Encoding.UTF8;

            tileWidth = Console.WindowWidth / 6;
            tileHeight = (int)(tileWidth*0.6);
            // input books
            bookCards = new List<BookCard>();
            bookCards.Add(new BookCard(new Book(), tileWidth, tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Last_Leaf.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Mysterious_Lighthouse.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Clockmaker's_Apprentice.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Silent_Watchman.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Time_Traveler's_Diary.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Whispering_Forest.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Shadow_in_the_Library.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Forgotten_Kingdom.json"),tileWidth,tileHeight));
            bookCards.Add(new BookCard(Book.JSONToBook(path + "The_Secret_Beneath_the_Lake.json"),tileWidth,tileHeight));

            

            MenuPage();
        }

        // draws the book menu
        public static void MenuPage(){
            DrawPageBorder(Console.WindowWidth, Console.WindowHeight);

            Console.SetCursorPosition(2, 1);

            foreach (string line in title)
            {
                PrintInLine(line);
            }
            PrintInLine(new string('-', Console.WindowWidth - 4));

            Console.CursorVisible = false;


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

            // redraw loop for when selection is changed
            while (running){
                int top = firstTop;
                // Look to see if a resize is needed;
                if (Console.WindowHeight != hieght || Console.WindowWidth != width){
                    MenuPage();
                    return;
                }
                

                // print each tile
                for (int i = 0; i < grid.Count; i++)
                {
                    for (int j = 0; j < grid[i].Count; j++){
                        PrintInLine(bookCards[i*grid[i].Count+j].Display(i*grid[i].Count+j==selected), true);
                        if (j < grid[i].Count - 1)
                            Console.SetCursorPosition(Console.CursorLeft, top);
                    }
                    Console.WriteLine();
                    Console.SetCursorPosition(left, Console.CursorTop);
                    top = Console.CursorTop;
                }
                
                // Input manager that recieves key strokes from console
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
                        reader.Open(bookCards[selected].getBook());
                        return;
                }
            
                Console.SetCursorPosition(left,firstTop);
            }
        }

        /// <summary>
        /// Prints to console but keeps the left padding
        /// </summary>
        /// <param name="text">The text that is outputed to the console</param>
        /// <param name="stayLast">Whether to have a new line charector at the end</param>
        public static void PrintInLine(string text, bool stayLast = false)
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

        /// <summary>
        /// Draws the border charectors around the screen
        /// </summary>
        /// <param name="w">Width of the box</param>
        /// <param name="h">Hieght of the box</param>
        private static void DrawPageBorder(int w, int h)
        {
            
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

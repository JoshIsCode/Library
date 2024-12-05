

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookProject
{
    internal class BookReader
    {
        public BookReader() { 
            
        }


        /// <summary>
        /// Opens the select book into the book reader
        /// </summary>
        /// <param name="book">An instance of the book class that will be displayed</param>
        public void Open(Book book)
        {
            Console.CursorVisible = false;

            int pageH = Console.WindowHeight-4;
            int pageW = (int)(pageH*1.6);
            bool running = true;

            int totalPages = 0;

            List<string[][]> chapters = new List<string[][]>();
            foreach (Chapter chapter in book.Chapters)
            {
                if (chapter.Paragraphs.Count() > 0)
                {
                    string[][] pages = ChapterToPages(chapter.Paragraphs, pageW - 4, pageH - 4);
                    chapters.Add(pages);
                    totalPages += pages.Length;
                }
                    
            }

            int chap = 0;
            int page = 0;
            int chapPage = 0;

            

            while (running)
            {
                // create table contents
                Console.Clear();
                int top = Console.CursorTop + 2;
                int left = Console.WindowWidth / 2 - pageW / 2 + 2;


                // print the page
                DrawPageBorder($"{book.Author} || {book.Title}", pageW, pageH);
                Console.SetCursorPosition(left-2, Console.CursorTop);
                Console.WriteLine($"{"[<-] Previus page".PadRight(pageW / 2 -1)}{page+1}/{totalPages}{"Next Page [->]".PadLeft(pageW / 2 - (""+ totalPages+(page+1)).Length + 1)}");
                Console.SetCursorPosition(left-2, Console.CursorTop);
                Console.WriteLine($"{"[ESC] home".PadRight(pageW / 2)} {"Chapters [SHIFT]".PadLeft(pageW / 2)}");

                Console.SetCursorPosition(left, top);

                foreach (string line in chapters[chap][chapPage])
                {
                    Program.PrintInLine(line);
                }

                // Look for input
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.Spacebar:
                        chapPage++;
                        page++;
                        if (chapPage == chapters[chap].Length)
                        {
                            if (chap == chapters.Count() - 1)
                            {
                                chapPage--;
                                page--;
                            } else
                            {
                                chap++;
                                chapPage = 0;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        chapPage--;
                        page--;
                        if (chapPage < 0)
                        {
                            if (chap == 0)
                            {
                                chapPage++;
                                page++;
                            }
                            else
                            {
                                chap--;
                                chapPage = chapters[chap].Length-1;
                            }
                        }
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        Program.MenuPage();
                        return;
                }
            }
        }

        
        /// <summary>
        /// Draws the border that goes around the page and the metadata at the top
        /// </summary>
        /// <param name="title">Text that is displayed above the book</param>
        /// <param name="w">Width of the book</param>
        /// <param name="h">Height of the book</param>
        private void DrawPageBorder(string title, int w, int h)
        {

            int left = Console.WindowWidth/2-w/2;

            Console.WriteLine(new String(' ', (Console.WindowWidth-title.Length)/2) + title);
            Console.SetCursorPosition(left, Console.CursorTop);
            Console.WriteLine(Program.border[2]+new string(Program.border[0],w-2) + Program.border[3]);
            Console.SetCursorPosition(left,Console.CursorTop);
            for (int i = 0; i < h - 2; i++)
            {
                Console.WriteLine(Program.border[1] + new string(' ', w - 2) + Program.border[1]);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            Console.WriteLine(Program.border[4] + new string(Program.border[0], w - 2) + Program.border[5]);
            
        }

        /// <summary>
        /// Splits a lists of paragraphs into multiple pages of lines.
        /// </summary>
        /// <param name="paragraphs">the chapter</param>
        /// <param name="w">the max width of a line</param>
        /// <param name="h">the max lines of a page</param>
        /// <returns></returns>
        private string[][] ChapterToPages(string[] paragraphs, int w, int h)
        {
            List<string[]> pages = new List<string[]>();


            List<string> page = new List<string>();
            foreach (string paragraph in paragraphs)
            {
                foreach(string line in ParagraphToLines(paragraph, w))
                {
                    page.Add(line);
                    if (page.Count() == h)
                    {
                        pages.Add(page.ToArray());
                        page = new List<string>();
                    }
                }
            }
            
            if (page.Count > 0)
            {
                pages.Add(page.ToArray());
            }

            return pages.ToArray();
        }
        
        /// <summary>
        /// Splits a paragraph into lines
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="w">the max number of charectors of a line</param>
        /// <returns></returns>
        private string[] ParagraphToLines(string paragraph, int w)
        {
            List<string> lines = new List<string>();
            string line = "";

            for (int i = 0; i < paragraph.Length; i++)
            {
                if (paragraph[i] == '\n')
                {
                    lines.Add(line);
                    line = "";
                } else if (line.Length == w)
                {
                    lines.Add(line);
                    line = "" + paragraph[i];
                } else
                {
                    line += paragraph[i];
                }
            }
            if (line != ""){
                lines.Add(line);
            }

            return lines.ToArray();
        }
    }
}

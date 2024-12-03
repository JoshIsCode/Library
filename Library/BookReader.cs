using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class BookReader
    {
        public BookReader() { 
            
        }

        public char[] border = { '─', '│', '┌', '┐', '└', '┘' };

        public void open(Book book)
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
                    string[][] pages = chapterToPages(chapter.Paragraphs, pageW - 4, pageH - 4);
                    chapters.Add(pages);
                    totalPages += pages.Length;
                }
                    
            }

            //Console.WriteLine(chapters[0].Count());

            //foreach (string[][] pages in chapters)
            //{
            //    Console.WriteLine("\n[[[[[[[[[[]]]]]]]]]]]]]]]]]][[[[[[[[[[[[[[[]]]]]");
            //    foreach (string[] page in pages)
            //    {
            //        Console.WriteLine("-----------------");
            //        foreach (string lin in page)
            //        {
            //            Console.WriteLine(lin);
            //        }
            //    }
            //}

            //return;

            int chap = 0;
            int page = 0;
            int chapPage = 0;

            

            while (running)
            {
                // create table contents
                Console.Clear();
                int top = Console.CursorTop + 2;
                int left = Console.WindowWidth / 2 - pageW / 2 + 2;

                drawPageBorder($"{book.Author} || {book.Title}", pageW, pageH);
                Console.SetCursorPosition(left-2, Console.CursorTop);
                Console.WriteLine($"{"[<-] Previus page".PadRight(pageW / 2 -1)}{page+1}/{totalPages}{"Next Page [->]".PadLeft(pageW / 2 - (""+ totalPages+(page+1)).Length + 1)}");
                Console.SetCursorPosition(left-2, Console.CursorTop);
                Console.WriteLine($"{"[ESC] home".PadRight(pageW / 2)} {"Chapters [SHIFT]".PadLeft(pageW / 2)}");

                Console.SetCursorPosition(left, top);

                foreach (string line in chapters[chap][chapPage])
                {
                    printInLine(line);
                }
                



                // print book

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
                        break;
                }
            }
        }

        private void printInLine(string text)
        {
            int left = Console.CursorLeft;
            Console.WriteLine(text);
            Console.SetCursorPosition(left, Console.CursorTop);
        }

        private void drawPageBorder(string title, int w, int h)
        {

            int left = Console.WindowWidth/2-w/2;

            Console.WriteLine(new String(' ', (Console.WindowWidth-title.Length)/2) + title);
            Console.SetCursorPosition(left, Console.CursorTop);
            Console.WriteLine(border[2]+new string(border[0],w-2) + border[3]);
            Console.SetCursorPosition(left,Console.CursorTop);
            for (int i = 0; i < h - 2; i++)
            {
                Console.WriteLine(border[1] + new string(' ', w - 2) + border[1]);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            Console.WriteLine(border[4] + new string(border[0], w - 2) + border[5]);
            
        }

        private string[][] chapterToPages(string[] paragraphs, int w, int h)
        {
            List<string[]> pages = new List<string[]>();


            List<string> page = new List<string>();
            foreach (string paragraph in paragraphs)
            {
                foreach(string line in paragraphToLines(paragraph, w))
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

        private string[] paragraphToLines(string paragraph, int w)
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
/*
┌───────┐    ┌───────┐    ┌───────┐
│  o o  │    │  ^  ^ │    │ *   * │
│   >   │    │   >   │    │   -   │
│ -──── │    │ └───┘ │    │  ~~~~ │
└───────┘    └───────┘    └───────┘
*/
using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO.Compression;
using System.IO;
using System.Xml;
using System.Text.Json;
using System.Xml.Linq;

namespace EBookProject
{

    

    internal class Book
    {

        public Chapter[] Chapters { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public Book(string title, string author, string genre, Chapter[] chapters) {
            Chapters = chapters;
            Title = title;
            Author = author;
            Genre = genre;
        }

        public Book()
        {
            Chapters = [new Chapter("emtpy", new string[] { "empty chapter" })];
            Title = "Default Book";
            Author = "Nobody";
            Genre = "none";
        }

        override public string ToString ()
        {
            return $"Title: {Title} | Author: {Author} | Genre: {Genre} ";
        }


        public static Book JSONToBook(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                Book obj = JsonSerializer.Deserialize<Book>(jsonString);
                return obj;
            }
            return null;
        }

      

    }

    class Chapter
    {
        public string[] Paragraphs { get; set; }
        public string Name { get; set; }

        public Chapter(string name, string[] paragraphs)
        {
            Paragraphs = paragraphs;
            Name = name;
        }
    }
}
// Archived genre class
//     public class Genre
//     {
//         public readonly bool IsNonFiction;
//         public readonly string Label;


//         // Fiction genres
//         public static readonly Genre LiteraryFiction = new Genre("Literary Fiction", false);
//         public static readonly Genre ScienceFiction = new Genre("Science Fiction", false);
//         public static readonly Genre Fantasy = new Genre("Fantasy", false);
//         public static readonly Genre MysteryThrillerCrime = new Genre("Mystery/Thriller/Crime", false);
//         public static readonly Genre Romance = new Genre("Romance", false);
//         public static readonly Genre Horror = new Genre("Horror", false);
//         public static readonly Genre HistoricalFiction = new Genre("Historical Fiction", false);
//         public static readonly Genre Adventure = new Genre("Adventure", false);
//         public static readonly Genre YoungAdult = new Genre("Young Adult", false);
//         public static readonly Genre ChildrenFiction = new Genre("Children's Fiction", false);

//         // Nonfiction genres
//         public static readonly Genre Biography = new Genre("Biography/Autobiography/Memoir", true);
//         public static readonly Genre SelfHelp = new Genre("Self-Help", true);
//         public static readonly Genre TrueCrime = new Genre("True Crime", true);
//         public static readonly Genre History = new Genre("History", true);
//         public static readonly Genre Science = new Genre("Science", true);
//         public static readonly Genre Philosophy = new Genre("Philosophy", true);
//         public static readonly Genre Travel = new Genre("Travel", true);
//         public static readonly Genre CookbooksFoodWriting = new Genre("Cookbooks/Food Writing", true);
//         public static readonly Genre BusinessFinance = new Genre("Business/Finance", true);
//         public static readonly Genre ReligionSpirituality = new Genre("Religion/Spirituality", true);


//         private Genre(string label, bool isNonFiction)
//         {
//             Label = label;
//             IsNonFiction = isNonFiction;    
//         }

//         public override string ToString()
//         {
//             return Label;
//         }

//     }

    
// }

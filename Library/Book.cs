using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO.Compression;
using System.IO;

namespace Library
{

    

    internal class Book
    {

        private Chapter[] Chapters;
        public readonly string Title;
        public readonly string Author;
        public readonly string Publisher;
        public readonly Genre genre;

        public Book(string title, string author, Genre genre, string publisher, Chapter[] chapters) {
            Chapters = chapters;
            Title = title;
            Publisher = publisher;
            Author = author;
            this.genre = genre;
        }


        public static Book EpubToBook(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(filePath))
                    {
                        Console.WriteLine("Contents of the ZIP file:");

                        var mimetype = archive.GetEntry("mimetype");

                        if (mimetype != null)
                        {
                            // Read the file's content (assuming it's a text file in this case)
                            using (var reader = new StreamReader(mimetype.Open()))
                            {
                                string content = reader.ReadToEnd();
                                if (content == "application/epub+zip")
                                {
                                    return makeBookFromEpub(archive);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"File 'mimetype' with extension '.mimetype' not found in the archive.");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("invalid file type");
                }
                
            } else
            {
                Console.WriteLine("no file found");
            }
            
            return null;
        }

        private static Book makeBookFromEpub(ZipArchive archive)
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                // Check if the entry belongs to the specified directory
                if (entry.FullName.StartsWith("OEBPS", StringComparison.OrdinalIgnoreCase)&&entry.FullName.EndsWith(".xhtml"))
                {
                    Console.WriteLine("Found file: " + entry.FullName);
                    
                }
            }
            return null;
        }

    }

    class Chapter
    {
        public readonly string[] Paragraphs;
        public readonly string Name;
        public readonly int Id;

        public Chapter(string name, int id, string[] paragraphs)
        {
            Paragraphs = paragraphs;
            Name = name;
            Id = id;
        }
    }

    public class Genre
    {
        public readonly bool IsNonFiction;
        public readonly string Label;


        // Fiction genres
        public static readonly Genre LiteraryFiction = new Genre("Literary Fiction", false);
        public static readonly Genre ScienceFiction = new Genre("Science Fiction", false);
        public static readonly Genre Fantasy = new Genre("Fantasy", false);
        public static readonly Genre MysteryThrillerCrime = new Genre("Mystery/Thriller/Crime", false);
        public static readonly Genre Romance = new Genre("Romance", false);
        public static readonly Genre Horror = new Genre("Horror", false);
        public static readonly Genre HistoricalFiction = new Genre("Historical Fiction", false);
        public static readonly Genre Adventure = new Genre("Adventure", false);
        public static readonly Genre YoungAdult = new Genre("Young Adult", false);
        public static readonly Genre ChildrenFiction = new Genre("Children's Fiction", false);

        // Nonfiction genres
        public static readonly Genre Biography = new Genre("Biography/Autobiography/Memoir", true);
        public static readonly Genre SelfHelp = new Genre("Self-Help", true);
        public static readonly Genre TrueCrime = new Genre("True Crime", true);
        public static readonly Genre History = new Genre("History", true);
        public static readonly Genre Science = new Genre("Science", true);
        public static readonly Genre Philosophy = new Genre("Philosophy", true);
        public static readonly Genre Travel = new Genre("Travel", true);
        public static readonly Genre CookbooksFoodWriting = new Genre("Cookbooks/Food Writing", true);
        public static readonly Genre BusinessFinance = new Genre("Business/Finance", true);
        public static readonly Genre ReligionSpirituality = new Genre("Religion/Spirituality", true);


        private Genre(string label, bool isNonFiction)
        {
            Label = label;
            IsNonFiction = isNonFiction;    
        }

    }

    
}

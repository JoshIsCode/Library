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

namespace Library
{

    

    internal class Book
    {

        public Chapter[] Chapters { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre genre { get; set; }

        public Book(string title, string author, Genre genre, Chapter[] chapters) {
            Chapters = chapters;
            Title = title;
            Author = author;
            this.genre = genre;
        }

        override public string ToString ()
        {
            return $"Title: {Title} | Author: {Author} | Genre: {genre} ";
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

        public static Book EpubToBook(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(filePath))
                    {

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
            ZipArchiveEntry content = archive.GetEntry("content.opf");

            if (content == null)
            {
                Console.WriteLine($"Unable to find content file or content file is empty");
                return null;
            }  

            using (var reader = new StreamReader(content.Open()))
            {
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                nsmgr.AddNamespace("opf", "http://www.idpf.org/2007/opf");
                doc.LoadXml(reader.ReadToEnd());
                //string author = doc.SelectSingleNode();

                // Book Metadata

                string author = "N/A";
                string title = "N/A";

                XmlNode metadata = doc.SelectSingleNode(@"//opf:package/opf:metadata", nsmgr);
                if (metadata != null)
                {
                    XmlNode authorNode = metadata.SelectSingleNode("dc:creator", nsmgr);
                    author = authorNode.InnerText ?? "N/A";
                    XmlNode titeNode = metadata.SelectSingleNode("dc:title", nsmgr);
                    title = titeNode.InnerText ?? "N/A";
                }

                XmlNode manifest = doc.SelectSingleNode(@"//opf:package/opf:manifest", nsmgr);
                if (manifest == null) return null;

                Dictionary<string, string> items = new Dictionary<string, string>();
                Console.WriteLine("Manifest: " + manifest.Name);
           
                foreach (XmlNode node in manifest.ChildNodes)
                {
                    string href = node.Attributes["href"].Value;
                    string id = node.Attributes["id"].Value;
                    items[id] = href;
                    Console.WriteLine(href + " " + id);
                }



                

                // Get Chapters



                //foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                //{

                //    string text = node.Name; //or loop through its children as well
                //    Console.WriteLine($"{ text}");
                //    foreach (XmlNode childNode in node.ChildNodes)
                //    {
                //        text = childNode.Name;
                //        Console.WriteLine($"{text}");
                //    }
                //}
            }
            //foreach (ZipArchiveEntry entry in archive.Entries)
            //{
            //    // Check if the entry belongs to the specified directory
            //    if (entry.FullName.StartsWith("OEBPS", StringComparison.OrdinalIgnoreCase)&&entry.FullName.EndsWith(".xhtml"))
            //    {
            //        Console.WriteLine("Found file: " + entry.FullName);
                    
            //    }
            //}
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

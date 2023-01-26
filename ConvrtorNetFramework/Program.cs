using System;
using System.IO;
using System.Linq;

namespace ConvrtorNetFramework
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Выберите режим: 1 - PDF to PNG, 2 - PNGS TO PDF");
            switch (Console.ReadLine())
            {
                case "1":
                {
                    Console.Write("Введите полный путь до файла:");
                    var filepathfullname = Console.ReadLine();
                    var fileInfo = new FileInfo(filepathfullname);


                    var fileNameWithoutExt = "name";
                    using (var stream = new FileStream(fileInfo.FullName, FileMode.Open))
                    {
                        Converting.Pdf2Png(fileInfo.Directory.FullName, fileNameWithoutExt , 300, stream);
                    }

                    break;
                }
                case "2":
                {
                    Console.Write("Введите полный путь до файлов png:");
                    DirectoryInfo dir = new DirectoryInfo(Console.ReadLine());
                    var imgs = dir.EnumerateFiles()
                        .Where(e => e.Name == ".png")
                        .OrderBy(e=>e.Name)
                        .ToList();
                    Converting.Pngs2Pdf(imgs);
                    break;
                }
                default: return;
            }
        }
    }
}
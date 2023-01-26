using System;
using System.Collections.Generic;
using Ghostscript.NET.Rasterizer;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using Image = iTextSharp.text.Image;

namespace ConvertingNetFramework
{
    public static class Converting
{
    /// <summary>
    /// Convert a PDF to PNG-files, and a thumbnail for each PNG.
    /// Using GhostScript.NET.
    /// </summary>
    /// <param name="imagePath">Desired path where files will be saved</param>
    /// /// <param name="imageName">Desired name on the image files</param>
    /// <param name="dpi">Desired dpi. 100-600 is normal. The higher dpi, the bigger file, and slower converting.</param>
    /// <param name="heightResolution">Set desired height Resolution</param>
    /// <param name="widthResolution">Set desired width Resolution</param>
    /// <param name="file">pdf file send as HttpPostedFileBase</param>
    public static void Pdf2Png(string imagePath, string imageName, int dpi, Stream pdfFile)
    {
        if (pdfFile != null)
        {
            string outputFolder = imagePath;

            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {
                rasterizer.Open(pdfFile);
                var numberOfPages = rasterizer.PageCount;
                for (int i = 0; i < numberOfPages; i++)
                {
                    var outputPNGPath =
                        Path.Combine(outputFolder, string.Format("{0}.png", imageName + i.ToString("D2")));
                    //var outputPNGThumbnailPath = Path.Combine(outputFolder, string.Format("Thumbnail_{0}.png", imageName + i.ToString("D2")));

                    var pdf2PNG = rasterizer.GetPage(dpi, i + 1);

                    Bitmap bitmap = new Bitmap(pdf2PNG);

                    bitmap.Save(outputPNGPath, ImageFormat.Png);
                    //thumbnailBitMap.Save(outputPNGThumbnailPath, ImageFormat.Png);
                }
            }
        }
    }
    
    public static void Pngs2Pdf(List<FileInfo> imagePaths)
    {
        // Создание документа PDF
        Document pdfDoc = new Document();

        // Запись документа PDF в поток
        var directoryFullName = imagePaths.FirstOrDefault()?.Directory?.FullName;
        Console.WriteLine(directoryFullName);
        if (directoryFullName != null)
        {
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc,
                new FileStream(directoryFullName, FileMode.Create));
        }

        pdfDoc.Open();

        // Добавление изображений в документ
        foreach (var image in imagePaths) {
            var img = MediaTypeNames.Image.GetInstance(image.FullName);
            pdfDoc.Add(img);
        }

        pdfDoc.Close();
    }
}
}


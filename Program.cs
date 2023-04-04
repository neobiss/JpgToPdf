using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using sharpPDF;
using System.Drawing.Drawing2D;
using sharpPDF.Enumerators;
using System.Drawing.Imaging;

namespace JpgToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir_in = @"d:\screen";
        
            List<string> dataList = new List<string>();
            foreach (string NameImg in Directory.GetFiles(dir_in, "*.jpg"))
            {
                dataList.Add(NameImg);
            }
            dataList.Sort();

            sharpPDF.pdfDocument newDoc = new pdfDocument("mypdf_from_jpg","My");
            sharpPDF.pdfPage page = newDoc.addPage();
            PointF locationTPage = new PointF(20, 750);

            string tName = "";
            int nom = 0;
            int nomPage = 1;
            int x = 50;
            int y1 = 0;
            int y = 0;
            bool first = true;
            dataList.ForEach(delegate (string name)
            {
                FileInfo test = new FileInfo(name);
                string name1 = test.Name; 
                nom = nom + 1;
                if ((tName != name1.Substring(0, 6)) &(!first))
                {
                    page = newDoc.addPage();
                    nom = 1;
                    nomPage++;
                }
                else if (nom>3)
                {
                    page = newDoc.addPage();
                    nomPage++;
                    nom = 1;
                }
                tName = name1.Substring(0, 6);
                PointF locationT = new PointF(0, 0);
                switch (nom)
                {
                    case 1:
                        y1 = 750;
                        locationT = new PointF(70, y1);
                        y = 520;
                        break;
                    case 2:
                        y1 = 500;
                        locationT = new PointF(70, y1);
                        y = 270;
                        break;
                    case 3:
                        y1 = 250;
                        locationT = new PointF(70, y1);
                        y = 20;
                        break;
                    default:
                         break;
                }
                page.addText(name1, 80, y1, predefinedFont.csHelvetica, 10);

                InsertImage(page, name, 40, y);
                first = false;
            });
            //page.addText(nomPage.ToString(), 20, 10, predefinedFont.csHelvetica, 10);
            newDoc.createPDF(@"d:\out_jpg.pdf");

        }

        static void InsertImage(sharpPDF.pdfPage page,string fileimg, int x, int y)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileimg);
            System.Drawing.Image img2 = Resize(img, 950, 470);
            page.addImage(img2, x, y, 220, 500);
        }

        public static Bitmap Resize(Image image, int width, int height)
        {

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }


}






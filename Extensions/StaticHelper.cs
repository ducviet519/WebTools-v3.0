using BarcodeEncoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebTools.Extensions
{
    public class StaticHelper
    {
        #region Barcode

        private static Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
        {
            // Compute actual size, shrink if needed
            while (true)
            {
                SizeF size = g.MeasureString(text, font);

                // It fits, back out
                if (size.Height <= proposedSize.Height &&
                     size.Width <= proposedSize.Width) { return font; }

                // Try a smaller font (90% of old size)
                Font oldFont = font;
                font = new Font(font.Name, (float)(font.Size * .9), font.Style);
                oldFont.Dispose();
            }
        }

        public static string GenBarCode(string barCode)
        {
            Int32 barcodeHeight = 30, barcodeWidth = 120;
            Bitmap bmpBarcode = new Bitmap(barcodeWidth, barcodeHeight);
            bmpBarcode.SetResolution(100, 100);
            Font drawFont;
            string image = "";

            if (barCode == "")
            {
            }
            else
            {
                // draw the barcode and text to the bitmap
                clsBarCode barcodeGenerator = new clsBarCode();
                String barcodeReadyData = barcodeGenerator.Code128(barCode, false);

                using (drawFont = new Font("IDAutomationC128M", 16))
                {
                    using (SolidBrush drawBrush = new SolidBrush(Color.Black))
                    {
                        using (Graphics dc = Graphics.FromImage(bmpBarcode))
                        {
                            // paint the whole bitmap white
                            dc.FillRectangle(new SolidBrush(Color.White), new RectangleF(0, 0, bmpBarcode.Width, bmpBarcode.Height));
                            // draw the barcode
                            //dc.DrawString(barcodeReadyData, drawFont, drawBrush, new RectangleF(0, 0, barcodeWidth, barcodeHeight - 70));
                            dc.DrawString(barcodeReadyData, drawFont, drawBrush, new RectangleF(0, 0, 150, 30));
                            // draw the human readable
                            //dc.DrawString(barCode, readableFont, drawBrush, new RectangleF(0, 30, barcodeWidth, barcodeHeight));
                        }
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmpBarcode.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    Convert.ToBase64String(byteImage);
                    image = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }

            return image;

            //using (Bitmap bitMap = new Bitmap(barCode.Length * 40, 80))
            //{
            //    using (Graphics graphics = Graphics.FromImage(bitMap))
            //    {
            //        Font oFont = new Font("IDAUTOMATIONC128S", 16);
            //        PointF point = new PointF(2f, 2f);
            //        SolidBrush blackBrush = new SolidBrush(Color.Black);
            //        SolidBrush whiteBrush = new SolidBrush(Color.White);
            //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
            //        graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            //    }
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        byte[] byteImage = ms.ToArray();

            //        Convert.ToBase64String(byteImage);
            //        image = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            //    }
            //}

            //return image;
        }

        #endregion Barcode

        #region Khử dấu cho string        
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #endregion


    }
}

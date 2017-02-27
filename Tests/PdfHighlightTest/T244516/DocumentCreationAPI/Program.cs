using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DocumentCreationAPI
{
    class Program
    {

        static void Main(string[] args)
        {
            PdfDocumentProcessor processor = new PdfDocumentProcessor();
            processor.LoadDocument("test.pdf");

            FindWord("glad to go back home to my mother; give me my wages.\" The master answered, \"You have served me faithfully and honestly;", processor, new SolidBrush(Color.FromArgb(100, 255, 255, 0)));
            FindWord("seven", processor, new SolidBrush(Color.FromArgb(100, 255, 255, 0)));

        }
        static void FindWord(string textToFind, PdfDocumentProcessor processor, SolidBrush brush)
        {
            PdfTextSearchResults result = processor.FindText(textToFind);
            while (result.Status == PdfTextSearchStatus.Found)
            {
                using (PdfGraphics graph = processor.CreateGraphics())
                {
                    DrawGraphics(graph, result, brush);
                }
                result = processor.FindText(textToFind);
            }
            processor.SaveDocument("result.pdf");
            Process.Start("result.pdf");
        }
        #region #Graphics
        static void DrawGraphics(PdfGraphics graph, PdfTextSearchResults result, SolidBrush brush)
        {
            for (int i = 0; i < result.Rectangles.Count; i++)
            {
                RectangleF rect = new RectangleF(new PointF((float)result.Rectangles[i].Left, (float)result.Page.CropBox.Top - (float)result.Rectangles[i].Top),
                    new SizeF((float)result.Rectangles[i].Width, (float)result.Rectangles[i].Height));
                graph.FillRectangle(brush, rect);
            }
        
            graph.AddToPageForeground(result.Page, 72, 72);
        }
        #endregion #Graphics

    }
}
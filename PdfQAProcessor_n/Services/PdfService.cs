using PdfiumViewer;
using System.Text;

namespace PdfQAProcessor_n.Services
{
    public class PdfService
    {
        public string ExtractText(string pdfPath)
        {
            StringBuilder extractedText = new StringBuilder();

            // Load the PDF using PdfiumViewer
            using (var pdfDocument = PdfDocument.Load(pdfPath))
            {
                // Loop through all pages to extract text
                for (int i = 0; i < pdfDocument.PageCount; i++)
                {
                    string pageText = pdfDocument.GetPdfText(i);
                    extractedText.AppendLine(pageText);
                }
            }

            // Limit the length of the extracted text (e.g., 500 characters) for better performance
            if (extractedText.Length > 500)
            {
                return extractedText.ToString().Substring(0, 500);  // First 500 characters
            }

            return extractedText.ToString();
        }
    }
}

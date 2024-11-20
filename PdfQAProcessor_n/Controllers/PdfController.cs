using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfQAProcessor_n.Services;

namespace PdfQAProcessor_n.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _pdfService;
        private readonly HuggingFaceService _huggingFaceService;

        public PdfController(PdfService pdfService, HuggingFaceService huggingFaceService)
        {
            _pdfService = pdfService;
            _huggingFaceService = huggingFaceService;
        }

        // Endpoint to upload PDF, extract text and ask question
        [HttpPost("UploadPDF")]
        public async Task<IActionResult> UploadPDF(IFormFile pdfFile, [FromQuery] string question)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest(new { Message = "No file uploaded." });
            }

            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest(new { Message = "Please provide a question." });
            }

            try
            {
                // Save the uploaded PDF file
                var tempFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                // Extract text from PDF
                string extractedText = _pdfService.ExtractText(tempFilePath);

                // Get the answer based on the extracted text and the user's question
                string answer = await _huggingFaceService.AskQuestionAsync(extractedText, question);

                return Ok(new { Question = question, Answer = answer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"ERROR: {ex.Message}" });
            }
        }
    }
}

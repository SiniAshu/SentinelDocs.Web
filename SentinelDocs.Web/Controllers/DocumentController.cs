using Microsoft.AspNetCore.Mvc;
using SentinelDocs.Infrastructure.Repositories;
using SentinelDocs.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SentinelDocs.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService DocService;

        public DocumentController(IDocumentService docService)
        {
            DocService = docService;
        }

        // GET: Document/Dashboard
        public async Task<IActionResult> Index()
        {
            var data = await DocService.GetAllDocuments();
            return View(data);
        }

        // POST: Document/Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please select a valid text file.");

            await DocService.ProcessDocumentAsync(file);

            return RedirectToAction("Index");
        }

        // This handles the "View Data" button for your 121 records
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            // Fetch the file name and the 121 metadata entries
            var details = await DocService.GetDocument(id);

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }
    }
}

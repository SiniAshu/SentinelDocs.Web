using Microsoft.AspNetCore.Mvc;
using SentinelDocs.Services.Interfaces;

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
            return View("~/Views/Home/Index.cshtml", data);
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
    }
}

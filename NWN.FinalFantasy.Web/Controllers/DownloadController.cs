using Microsoft.AspNetCore.Mvc;

namespace NWN.FinalFantasy.Web.Controllers
{
    public class DownloadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DownloadHaks()
        {
            // Could live in a DB or something, but don't want to complicate it.
            const string LocalPath = "/var/www/ffo_public_files/FFOHaks.rar";
            const string ContentType = "application/octet-stream";
            const string FileName = "FFODevelopmentHaks.rar";

            if (!System.IO.File.Exists(LocalPath))
            {
                return Content("Could not find file.");
            }

            return File(System.IO.File.OpenRead(LocalPath), ContentType, FileName);
        }
    }
}

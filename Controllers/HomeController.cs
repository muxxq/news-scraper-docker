using Microsoft.AspNetCore.Mvc;
using NewsScraperApp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NewsScraperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            string targetUrl = "https://ace.ucv.ro/media/"; 
            
            string xpathSelector = "//h3/a | //h2/a";
          

            List<string> newsTitles = await NewsScraper.GetNewsTitlesFromHtml(targetUrl, xpathSelector);

            return View(newsTitles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

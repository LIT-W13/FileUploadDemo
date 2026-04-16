using System.Diagnostics;
using FileUploadDemo.Data;
using FileUploadDemo.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=FileUploadDemo;Integrated Security=true;Trust Server Certificate=true";

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var repo = new ImageRepository(_connectionString);
            
            return View(new HomePageViewModel { Images = repo.GetAll()});
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(string title, IFormFile image)
        {

            //_webHostEnvironment.WebRootPath; - this is the full path to the wwwroot folder

            string fileName = $"{Guid.NewGuid()}-{image.FileName}";
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

            using FileStream fs = new FileStream(fullPath, FileMode.Create);
            image.CopyTo(fs);
            var repo = new ImageRepository(_connectionString);
            repo.Add(new Image
            {
                Title = title,
                ImagePath = fileName
            });
            return RedirectToAction("Index");

        }

    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSite.Data;
using ShopSite.Models;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;

namespace ShopSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _appDb.Categories.ToListAsync();
            return View(categories);
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
        public ViewResult Login() => View();
        [HttpPost]
        public async Task<IActionResult> Login(AdminModel model)
        {
            try
            {
                if (model.UserName == "admin+9821" && model.Password.EndsWith("@admin"))
                {
                    TempData["success"] = "Successfully entered the admin page";
                    return RedirectToAction("EnterAdmin", "User");
                }
                TempData["failed"] = "Wrong username or password";
                return View();
            }
            catch(Exception exp)
            {
                TempData["failed"] = exp.Message;
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("MyCookie");
                TempData["success"] = "Logged out successfully";
                return RedirectToAction("Index");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("Index", "Home");

            }

        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> EditSite()
        {
            var model = await _appDb.EditModels.SingleOrDefaultAsync();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditSite(EditModel edModel)
        {
            try
            {
                if (await _appDb.EditModels.AnyAsync())
                {
                    var editModel = await _appDb.EditModels.SingleOrDefaultAsync();
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var addressFilePath = Path.Combine(wwwRootPath + "/txt/", "address.txt");
                    var aboutUsFilePath = Path.Combine(wwwRootPath + "/txt/", "aboutus.txt");
                    if (edModel.IconFile?.FileName != null)
                    {
                        string extension = Path.GetExtension(edModel.IconFile?.FileName);
                        edModel.IconFileName = DateTime.Now.ToString("yymmssfff") + extension;
                        string iconPath = Path.Combine(wwwRootPath + "/image/", edModel.IconFileName);
                        using (var fileStream = new FileStream(iconPath, FileMode.Create))
                        {
                            await edModel.IconFile.CopyToAsync(fileStream);
                        }
                        editModel.IconFileName = edModel.IconFileName;
                    }
                    if (!string.IsNullOrWhiteSpace(edModel.Address))
                    {
                        using (StreamWriter sw = new StreamWriter(addressFilePath))
                        {
                            await sw.WriteAsync(string.Empty);
                            await sw.WriteAsync(edModel.Address);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(edModel.AboutUs))
                    {
                        using (StreamWriter sw = new StreamWriter(aboutUsFilePath))
                        {
                            await sw.WriteAsync(string.Empty);
                            await sw.WriteAsync(edModel.AboutUs);
                        }
                    }
                    editModel.Title = edModel.Title;
                    editModel.PhoneNumber = edModel.PhoneNumber;
                    editModel.Email = edModel.Email;
                    editModel.FacebookUrl = edModel.FacebookUrl;
                    editModel.InstagramUrl = edModel.InstagramUrl;
                    editModel.TwitterUrl = edModel.TwitterUrl;
                    editModel.LinkedinUrl = edModel.LinkedinUrl;
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("Index");

                }
                else
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var addressFilePath = Path.Combine(wwwRootPath + "/txt/", "address.txt");
                    var aboutUsFilePath = Path.Combine(wwwRootPath + "/txt/", "aboutus.txt");
                    if (edModel.IconFile?.FileName != null)
                    {
                        string extension = Path.GetExtension(edModel.IconFile?.FileName);
                        edModel.IconFileName = DateTime.Now.ToString("yymmssfff") + extension;
                        string iconPath = Path.Combine(wwwRootPath + "/image/", edModel.IconFileName);
                        using (var fileStream = new FileStream(iconPath, FileMode.Create))
                        {
                            await edModel.IconFile.CopyToAsync(fileStream);
                        }
                    }
                    using (StreamWriter sw = new StreamWriter(addressFilePath))
                    {
                        await sw.WriteAsync(edModel.Address);
                    }
                    using (StreamWriter sw = new StreamWriter(aboutUsFilePath))
                    {
                        await sw.WriteAsync(edModel.AboutUs);
                    }
                    await _appDb.EditModels.AddAsync(edModel);
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("Index");

                }
            }catch(Exception exp)
            {
                TempData["failed"] = "Something unexpected happened. Try again";
                return RedirectToAction("AdminPanel");
            }

        }
        public ViewResult UI() => View();
        [Authorize(Roles ="Admin")]
        public ViewResult AdminPanel() => View();
        public ViewResult Aboutus() => View();
    }
}
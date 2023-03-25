using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSite.Data;
using ShopSite.Models;

namespace ShopSite.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDb _appDb;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(AppDb appDb, IWebHostEnvironment webHostEnvironment)
        {
            _appDb = appDb;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _appDb.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _appDb.Categories.FindAsync(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            try
            {
                var dbCategory = await _appDb.Categories.FindAsync(category.Id);
                if(await _appDb.Categories.AnyAsync(c=> c.CategoryName == category.CategoryName && c.Id != category.Id))
                {
                    TempData["failed"] = "There's a category with the same name already";
                    return RedirectToAction("Index", "Home");
                }
                if (category.Picture?.FileName != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(category.Picture.FileName);
                    category.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                    string picturePath = Path.Combine(wwwRootPath + "/image/", category.PictureName);
                    using (var fileStream = new FileStream(picturePath, FileMode.Create))
                    {
                        await category.Picture.CopyToAsync(fileStream);
                    }
                    dbCategory.CategoryName = category.CategoryName;
                    dbCategory.PictureName = category.PictureName;
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("Index", "Home");

                }
                dbCategory.CategoryName = category.CategoryName;
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return View();
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> NewUpdateCategory(Category category)
        {
            try
            {
                var dbCategory = await _appDb.Categories.FindAsync(category.Id);
                if (await _appDb.Categories.AnyAsync(c => c.CategoryName == category.CategoryName && c.Id != category.Id))
                {
                    TempData["failed"] = "There's a category with the same name already";
                    return RedirectToAction("GetAllCategories");
                }
                if (category.Picture?.FileName != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(category.Picture.FileName);
                    category.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                    string picturePath = Path.Combine(wwwRootPath + "/image/", category.PictureName);
                    using (var fileStream = new FileStream(picturePath, FileMode.Create))
                    {
                        await category.Picture.CopyToAsync(fileStream);
                    }
                    dbCategory.CategoryName = category.CategoryName;
                    dbCategory.PictureName = category.PictureName;
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("GetAllCategories");

                }
                dbCategory.CategoryName = category.CategoryName;
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("GetAllCategories");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("GetAllCategories");
            }

        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _appDb.Categories.FindAsync(id);
                _appDb.Categories.Remove(category);
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("Index", "Home");
            }
            catch(Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("Index", "Home");

            }
            
        }
        [HttpPost]
        public async Task<IActionResult> NewDeleteCategory(int id)
        {
            try
            {
                var category = await _appDb.Categories.FindAsync(id);
                _appDb.Categories.Remove(category);
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("GetAllCategories");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("GetAllCategories");

            }

        }
        public ViewResult AddCategory() => View();
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            try
            {
                if(await _appDb.Categories.AnyAsync(c=> c.CategoryName == category.CategoryName))
                {
                    TempData["failed"] = "There's a category with the same name already";
                    return RedirectToAction("AdminPanel","Home");
                }
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(category.Picture.FileName);
                category.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                string picturePath = Path.Combine(wwwRootPath + "/image/", category.PictureName);
                using (var fileStream = new FileStream(picturePath, FileMode.Create))
                {
                    await category.Picture.CopyToAsync(fileStream);
                }
                await _appDb.Categories.AddAsync(category);
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("AdminPanel","Home");

            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("AdminPanel", "Home");

            }
        }
    }
}

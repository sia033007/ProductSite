using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopSite.Data;
using ShopSite.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

namespace ShopSite.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDb _appDb;
        public ProductController(IWebHostEnvironment webHostEnvironment, AppDb appDb)
        {
            _webHostEnvironment = webHostEnvironment;
            _appDb = appDb;
        }
        public ViewResult AddProduct() => View();
        [HttpPost]

        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                if(await _appDb.Products.AnyAsync(p=> p.Name == product.Name))
                {
                    TempData["failed"] = "There's a product with the same name already";
                    return RedirectToAction("AdminPanel","Home");
                }
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(product.Picture.FileName);
                product.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                string picturePath = Path.Combine(wwwRootPath + "/image/", product.PictureName);
                using (var fileStream = new FileStream(picturePath, FileMode.Create))
                {
                    await product.Picture.CopyToAsync(fileStream);
                }
                //product.DescriptionFileName = DateTime.Now.ToString("yymmssfff") + ".txt";
                //string txtPath = Path.Combine(wwwRootPath + "/txt/", product.DescriptionFileName);
                //using (StreamWriter sw = new StreamWriter(txtPath))
                //{
                //    await sw.WriteAsync(product.Description);  
                //}
                await _appDb.Products.AddAsync(product);
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("AdminPanel", "Home");
            }
            catch(Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("AdminPanel", "Home");

            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsOfACategory(int? id, int? pageNumber = 1)
        {
            var products = await PaginatedList<Product>.CreateAsync(_appDb.Products.Where(p => p.CategoryId == id).OrderByDescending(b => b.Id), pageNumber, 8);
            HttpContext.Session.SetString("pageNumber", pageNumber.ToString());
            return View(products);


        }
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await _appDb.Products.FindAsync(id);
            return PartialView("_ProductPartial", product);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            try
            {
                var category = await _appDb.Categories.SingleOrDefaultAsync(c => c.Id == product.CategoryId);
                int pageNumber = Convert.ToInt32(HttpContext.Session.GetString("pageNumber"));
                var dbProduct = await _appDb.Products.FindAsync(product.Id);
                if(await _appDb.Products.AnyAsync(p=> p.Name == product.Name && p.Id != product.Id))
                {
                    TempData["failed"] = "There's a product with the same name already";
                    return RedirectToAction("GetAllProductsOfACategory", new {id = category.Id, pageNumber = pageNumber});
                }
                if (product.Picture?.FileName != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(product.Picture.FileName);
                    product.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                    string picturePath = Path.Combine(wwwRootPath + "/image/", product.PictureName);
                    using (var fileStream = new FileStream(picturePath, FileMode.Create))
                    {
                        await product.Picture.CopyToAsync(fileStream);
                    }
                    dbProduct.Name = product.Name;
                    dbProduct.Price = product.Price;
                    dbProduct.OldPrice = product.OldPrice;
                    dbProduct.Quantity = product.Quantity;
                    dbProduct.Currency = product.Currency;
                    dbProduct.PictureName = product.PictureName;
                    dbProduct.CategoryId = product.CategoryId;
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("GetAllProductsOfACategory", new { id = category.Id, pageNumber = pageNumber });

                }
                dbProduct.Name = product.Name;
                dbProduct.Price = product.Price;
                dbProduct.OldPrice = product.OldPrice;
                dbProduct.Quantity = product.Quantity;
                dbProduct.Currency = product.Currency;
                dbProduct.CategoryId = product.CategoryId;
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("GetAllProductsOfACategory", new { id = category.Id, pageNumber = pageNumber });
            }
            catch(Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewUpdateProduct(Product product)
        {
            try
            {
                var dbProduct = await _appDb.Products.FindAsync(product.Id);
                if (await _appDb.Products.AnyAsync(p => p.Name == product.Name && p.Id != product.Id))
                {
                    TempData["failed"] = "There's a product with the same name already";
                    return RedirectToAction("GetAllProducts");
                }
                if (product.Picture?.FileName != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(product.Picture.FileName);
                    product.PictureName = DateTime.Now.ToString("yymmssfff") + extension;
                    string picturePath = Path.Combine(wwwRootPath + "/image/", product.PictureName);
                    using (var fileStream = new FileStream(picturePath, FileMode.Create))
                    {
                        await product.Picture.CopyToAsync(fileStream);
                    }
                    dbProduct.Name = product.Name;
                    dbProduct.Price = product.Price;
                    dbProduct.OldPrice = product.OldPrice;
                    dbProduct.Quantity = product.Quantity;
                    dbProduct.Currency = product.Currency;
                    dbProduct.PictureName = product.PictureName;
                    dbProduct.CategoryId = product.CategoryId;
                    await _appDb.SaveChangesAsync();
                    TempData["success"] = "The operation was successful";
                    return RedirectToAction("GetAllProducts");

                }
                dbProduct.Name = product.Name;
                dbProduct.Price = product.Price;
                dbProduct.OldPrice = product.OldPrice;
                dbProduct.Quantity = product.Quantity;
                dbProduct.Currency = product.Currency;
                dbProduct.CategoryId = product.CategoryId;
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("GetAllProducts");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("GetAllProducts");
            }
        }
        public async Task<IActionResult> GetAllProducts()
        {
            return View(await _appDb.Products.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _appDb.Products.FindAsync(id);
                _appDb.Products.Remove(product);
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
        public async Task<IActionResult> NewDeleteProduct(int id)
        {
            try
            {
                var product = await _appDb.Products.FindAsync(id);
                _appDb.Products.Remove(product);
                await _appDb.SaveChangesAsync();
                TempData["success"] = "The operation was successful";
                return RedirectToAction("GetAllProducts");
            }
            catch (Exception exp)
            {
                TempData["failed"] = exp.Message;
                return RedirectToAction("GetAllProducts");
            }

        }
    }

    
}

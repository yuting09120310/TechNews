 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechNews.Areas.BackEnd.Models;
using TechNews.Areas.BackEnd.ViewModel.News;

namespace TechNews.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class NewsController : GenericController
    {
        private readonly TechNewsDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public NewsController(TechNewsDBContext context, IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }


        // GET: BackEnd/News
        public async Task<IActionResult> Index()
        {
            GetMenu();

            var newsList = await _context.News
                .Include(n => n.Category)
                .Select(n => new NewsIndexViewModel
                {
                    NewsId = n.NewsId,
                    Title = n.Title,
                    Description = n.Description,
                    CategoryName = n.Category.Name,
                    IsActive = n.IsActive,
                    PublishDate = n.PublishDate,
                    ExpireDate = n.ExpireDate,
                    ViewCount = n.ViewCount
                })
                .ToListAsync();

            return View(newsList);
        }


        // GET: BackEnd/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: BackEnd/News/Create
        public IActionResult Create()
        {
            GetMenu();
            ViewData["CategoryId"] = new SelectList(_context.NewsCategories, "CategoryId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = new News
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    IsActive = model.IsActive,
                    PublishDate = model.PublishDate,
                    ModifiedDate = DateTime.UtcNow,
                    LanguageCode = "1"
                };

                // 處理圖片上傳
                if (model.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "News");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    news.ImagePath = "/uploads/" + uniqueFileName;
                }

                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.NewsCategories, "CategoryId", "Name", model.CategoryId);
            return View(model);
        }


        public IActionResult Edit(int id)
        {
            GetMenu();

            var news = _context.News.FirstOrDefault(n => n.NewsId == id);
            if (news == null) return NotFound();

            var viewModel = new NewsEditViewModel
            {
                Id = news.NewsId,
                Title = news.Title,
                Description = news.Description,
                Content = news.Content,
                CategoryId = news.CategoryId,
                IsActive = news.IsActive,
                PublishDate = news.PublishDate,
                ExistingImagePath = news.ImagePath
            };

            ViewData["CategoryId"] = new SelectList(_context.NewsCategories, "CategoryId", "Name");
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.NewsCategories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            var news = await _context.News.FindAsync(model.Id);
            if (news == null)
            {
                return NotFound();
            }

            // 更新新聞屬性
            news.Title = model.Title;
            news.Description = model.Description;
            news.Content = model.Content;
            news.CategoryId = model.CategoryId;
            news.IsActive = model.IsActive;
            news.PublishDate = model.PublishDate;
            news.ModifiedDate = DateTime.UtcNow;

            // 處理圖片上傳
            if (model.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "News");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 確保目錄存在
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                news.ImagePath = "/uploads/News/" + uniqueFileName;
            }

            try
            {
                _context.Update(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"更新失敗: {ex.Message}");
                ViewData["CategoryId"] = new SelectList(_context.NewsCategories, "CategoryId", "Name", model.CategoryId);
                return View(model);
            }
        }



        // GET: BackEnd/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: BackEnd/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'TechNewsDBContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }


        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 設定檔案大小限制，單位：byte
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                try
                {
                    // 設定儲存路徑
                    var filePath = Path.Combine("wwwroot/uploads", upload.FileName);

                    // 確保目錄存在
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                    // 儲存檔案到伺服器
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.CopyToAsync(stream);
                    }

                    // 回傳給 CKEditor 的 JSON 格式

                    var aa = new
                    {
                        uploaded = true,
                        url = $"https://localhost:5655/uploads/{upload.FileName}"
                    };

                    return Json(aa);
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        uploaded = false,
                        error = new { message = "上傳失敗：" + ex.Message }
                    });
                }
            }

            return Json(new
            {
                uploaded = false,
                error = new { message = "沒有選擇檔案或檔案無效" }
            });
        }

    }
}

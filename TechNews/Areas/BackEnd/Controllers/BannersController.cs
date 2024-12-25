using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TechNews.Areas.BackEnd.Models;
using TechNews.Areas.BackEnd.ViewModel.Banners;

namespace TechNews.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class BannersController : GenericController
    {
        private readonly TechNewsDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public BannersController(TechNewsDBContext context, IWebHostEnvironment environment) : base(context)
        {
            _context = context;
            _environment = environment;
        }


        // GET: BackEnd/Banners
        public IActionResult Index()
        {
            GetMenu();

            var banners = _context.Banners
                .Select(b => new BannerIndexViewModel
                {
                    BannerId = b.BannerId,
                    Title = b.Title,
                    Description = b.Description,
                    ImagePath = "https://localhost:5655/" + b.ImagePath,
                    IsActive = b.IsActive,
                    PublishDate = b.PublishDate,
                    ExpireDate = b.ExpireDate
                })
                .ToList();

            return View(banners);
        }

       

        // GET: BackEnd/Banners/Create
        public IActionResult Create()
        {
            GetMenu();
            return View();
        }


        // POST: BackEnd/Banners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BannerCreateViewModel model)
        {
            GetMenu();
            if (ModelState.IsValid)
            {
                string? imagePath = null;

                // 處理圖片上傳
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "banners");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    imagePath = Path.Combine("uploads", "banners", uniqueFileName);
                }

                var banner = new Banner
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImagePath = imagePath,
                    ImageAlt = model.ImageAlt,
                    IsActive = model.IsActive,
                    PublishDate = model.PublishDate,
                    ExpireDate = model.ExpireDate
                };

                _context.Banners.Add(banner);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }



        // GET: BackEnd/Banners/Edit/5
        public IActionResult Edit(int id)
        {
            GetMenu();

            var banner = _context.Banners.Find(id);
            if (banner == null)
            {
                return NotFound();
            }

            var model = new BannerEditViewModel
            {
                BannerId = banner.BannerId,
                Title = banner.Title,
                Description = banner.Description,
                CurrentImagePath = banner.ImagePath,
                ImageAlt = banner.ImageAlt,
                IsActive = banner.IsActive,
                PublishDate = banner.PublishDate,
                ExpireDate = banner.ExpireDate
            };

            return View(model);
        }


        // POST: BackEnd/Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BannerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var banner = _context.Banners.Find(model.BannerId);
                if (banner == null)
                {
                    return NotFound();
                }

                string? newImagePath = banner.ImagePath;

                // 處理圖片更新
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "banners");
                    Directory.CreateDirectory(uploadsFolder);

                    // 刪除舊圖片
                    if (!string.IsNullOrEmpty(banner.ImagePath))
                    {
                        var oldFilePath = Path.Combine(_environment.WebRootPath, banner.ImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // 儲存新圖片
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    newImagePath = Path.Combine("uploads", "banners", uniqueFileName);
                }

                // 更新資料庫
                banner.Title = model.Title;
                banner.Description = model.Description;
                banner.ImagePath = newImagePath;
                banner.ImageAlt = model.ImageAlt;
                banner.IsActive = model.IsActive;
                banner.PublishDate = model.PublishDate;
                banner.ExpireDate = model.ExpireDate;

                _context.Update(banner);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: BackEnd/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            GetMenu();

            if (id == null || _context.Banners == null)
            {
                return NotFound();
            }

            var banner = await _context.Banners
                .FirstOrDefaultAsync(m => m.BannerId == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }


        // POST: BackEnd/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Banners == null)
            {
                return Problem("Entity set 'TechNewsDBContext.Banners' is null.");
            }

            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                // 確認檔案是否存在於伺服器
                if (!string.IsNullOrEmpty(banner.ImagePath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", banner.ImagePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        // 刪除檔案
                        System.IO.File.Delete(filePath);
                    }
                }

                // 從資料庫中移除橫幅
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool BannerExists(int id)
        {
          return (_context.Banners?.Any(e => e.BannerId == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechNews.Areas.BackEnd.Models;
using TechNews.Areas.BackEnd.ViewModel.Admins;

namespace TechNews.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class AdminsController : GenericController
    {
        private readonly TechNewsDBContext _context;

        public AdminsController(TechNewsDBContext context) : base(context)
        {   
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            GetMenu();

            // 查詢 Admins 並投影到 ViewModel
            var admins = await _context.Admins
                .Include(a => a.Group)
                .Select(a => new AdminsIndexViewModel
                {
                    AdminId = a.AdminId,
                    AdminAccount = a.AdminAccount,
                    AdminName = a.AdminName,
                    GroupName = a.Group.GroupName,
                    IsActive = a.IsActive,
                    LastLogin = a.LastLogin,
                    ModifiedDate = a.ModifiedDate
                })
                .ToListAsync();

            return View(admins);
        }


        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            GetMenu();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            GetMenu();

            ViewData["GroupId"] = new SelectList(_context.AdminGroups, "GroupId", "GroupId");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,GroupId,AdminAccount,AdminPassword,AdminName,IsActive,LastLogin,ModifiedDate")] Admin admin)
        {
            GetMenu();

            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.AdminGroups, "GroupId", "GroupId", admin.GroupId);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetMenu();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            // 將 Admin 實體轉換為 ViewModel
            var adminsEditViewModel = new AdminsEditViewModel
            {
                AdminId = admin.AdminId,
                AdminAccount = admin.AdminAccount,
                AdminPassword = admin.AdminPassword,
                AdminName = admin.AdminName,
                GroupId = admin.GroupId,
                IsActive = admin.IsActive
            };

            // 傳遞下拉選單資料
            ViewData["GroupId"] = new SelectList(_context.AdminGroups, "GroupName", "GroupName", adminsEditViewModel.GroupId);
            return View(adminsEditViewModel);
        }


        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminsEditViewModel adminsEditViewModel)
        {
            GetMenu();

            // 確認 URL 的 id 與 ViewModel 中的 AdminId 是否一致
            if (id != adminsEditViewModel.AdminId)
            {
                return NotFound();
            }

            // 驗證 ViewModel
            if (ModelState.IsValid)
            {
                try
                {
                    // 將 ViewModel 轉換為 Entity 模型
                    var admin = await _context.Admins.FindAsync(id);
                    if (admin == null)
                    {
                        return NotFound();
                    }

                    // 更新數據
                    admin.AdminAccount = adminsEditViewModel.AdminAccount;
                    admin.AdminPassword = adminsEditViewModel.AdminPassword;
                    admin.AdminName = adminsEditViewModel.AdminName;
                    admin.GroupId = adminsEditViewModel.GroupId;
                    admin.IsActive = adminsEditViewModel.IsActive;
                    admin.ModifiedDate = DateTime.Now; // 更新修改時間

                    // 保存變更
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(adminsEditViewModel.AdminId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // 如果驗證失敗，回傳 View 並附加必要資料
            ViewData["GroupId"] = new SelectList(_context.AdminGroups, "GroupId", "GroupName", adminsEditViewModel.GroupId);
            return View(adminsEditViewModel);
        }


        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetMenu();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            GetMenu();

            if (_context.Admins == null)
            {
                return Problem("Entity set 'TechNewsDBContext.Admins'  is null.");
            }
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id)
        {
            GetMenu();

            return (_context.Admins?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }
    }
}

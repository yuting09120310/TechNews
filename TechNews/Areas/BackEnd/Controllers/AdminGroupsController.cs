using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechNews.Areas.BackEnd.Models;
using TechNews.Areas.BackEnd.ViewModel.AdminGroups;

namespace TechNews.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class AdminGroupsController : GenericController
    {
        private readonly TechNewsDBContext _context;

        public AdminGroupsController(TechNewsDBContext context) : base(context)
        {
            _context = context;
        }

        // GET: BackEnd/AdminGroups
        public async Task<IActionResult> Index()
        {
            GetMenu();

            // 使用 LINQ 投影到 ViewModel
            var adminGroups = await _context.AdminGroups
                .Select(group => new AdminGroupsIndexViewModel
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    GroupInfo = group.GroupInfo,
                    IsActive = group.IsActive,
                    ModifiedDate = group.ModifiedDate
                })
                .ToListAsync();

            return View(adminGroups);
        }

        
        // GET: BackEnd/AdminGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BackEnd/AdminGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupName,GroupInfo,IsActive,ModifiedDate")] AdminGroup adminGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminGroup);
        }

        // GET: BackEnd/AdminGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            GetMenu();

            if (id == null || _context.AdminGroups == null)
            {
                return NotFound();
            }

            var adminGroup = await _context.AdminGroups.FindAsync(id);
            if (adminGroup == null)
            {
                return NotFound();
            }

            AdminGroupsEditViewModel adminGroupViewModel = new AdminGroupsEditViewModel()
            {
                GroupId = adminGroup.GroupId,
                GroupName = adminGroup.GroupName,
                GroupInfo = adminGroup.GroupInfo,
                IsActive = adminGroup.IsActive,

                AdminRoleModels = _context.AdminRoles.Where(x => x.GroupId == id).ToList(),
                MenuGroupModels = _context.MenuGroups.Where(x => x.IsActive == true).ToList(),
                MenuSubModels = _context.MenuSubs.Where(x => x.IsActive == true).ToList(),
            };


            return View(adminGroupViewModel);
        }

        // POST: BackEnd/AdminGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection Collection)
        {
            //取得變更的群組id
            int groupNum = Convert.ToInt32(Collection["GroupId"]);

            //取得關於Role開頭的Key 重組成字典 以便於後續操作
            Dictionary<string, string> roleDicts = Collection
             .Where(kv => kv.Key.StartsWith("Role"))
             .Select(kv => new KeyValuePair<string, string>(kv.Key.Split('_')[1], kv.Value!))
             .ToDictionary(kv => kv.Key, kv => kv.Value);


            //將取出開頭包含Role的字典 跑迴圈 並逐筆變更
            foreach (string roleDict in roleDicts.Keys)
            {
                int menuSubNum = Convert.ToInt32(roleDict);
                AdminRole ar = _context.AdminRoles.Where(x => x.GroupId == groupNum && x.MenuId == menuSubNum).FirstOrDefault();
                if (ar != null)
                {
                    ar.Permission = roleDicts[roleDict];
                    _context.Update(ar);
                }
                else
                {
                    ar = new AdminRole()
                    {
                        GroupId = groupNum,
                        MenuId = menuSubNum,
                        Permission = roleDicts[roleDict],
                        ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    };
                    _context.AdminRoles.Add(ar);
                }
            }


            AdminGroup adminGroup = _context.AdminGroups.Where(x => x.GroupId == groupNum).FirstOrDefault();
            adminGroup.GroupName = Collection["GroupName"].ToString();
            adminGroup.GroupInfo = Collection["GroupInfo"].ToString();
            adminGroup.IsActive = Convert.ToBoolean(Collection["IsActive"].ToString());
            adminGroup.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _context.Update(adminGroup);

            _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        // GET: BackEnd/AdminGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            GetMenu();

            if (id == null || _context.AdminGroups == null)
            {
                return NotFound();
            }

            var adminGroup = await _context.AdminGroups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (adminGroup == null)
            {
                return NotFound();
            }

            return View(adminGroup);
        }

        // POST: BackEnd/AdminGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdminGroups == null)
            {
                return Problem("Entity set 'TechNewsDBContext.AdminGroups'  is null.");
            }
            var adminGroup = await _context.AdminGroups.FindAsync(id);
            if (adminGroup != null)
            {
                _context.AdminGroups.Remove(adminGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminGroupExists(int id)
        {
          return (_context.AdminGroups?.Any(e => e.GroupId == id)).GetValueOrDefault();
        }
    }
}

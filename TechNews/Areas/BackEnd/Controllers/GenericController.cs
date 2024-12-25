using Microsoft.AspNetCore.Mvc;
using TechNews.Areas.BackEnd.Models;

namespace TechNews.Areas.BackEnd.Controllers
{
    public class GenericController : Controller
    {
        private readonly TechNewsDBContext _context;

        public GenericController(TechNewsDBContext context)
        {
            _context = context;
        }

        public void GetMenu()
        {
            //int GroupNum = Convert.ToInt16(HttpContext.Session.GetString("GroupNum"));
            int GroupNum = 1;

            // 取得主選單資訊
            var module = from c in _context.MenuGroups
                         where c.IsActive == true
                         orderby c.MenuGroupId ascending
                         select c;

            ViewBag.module = module.ToList();

            // 取得次選單資訊
            var moduleFun = from c in _context.MenuSubs
                            join s in _context.AdminRoles on c.MenuId equals s.MenuId
                            where c.IsActive == true && s.GroupId == GroupNum
                            select c;

            ViewBag.moduleFun = moduleFun.ToList();
        }
    }
}

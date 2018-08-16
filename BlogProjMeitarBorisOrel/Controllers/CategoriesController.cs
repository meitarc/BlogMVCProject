using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjMeitarBorisOrel.Data;
using BlogProjMeitarBorisOrel.Models;

using BlogProjMeitarBorisOrel.Models.Blog;

namespace BlogProjMeitarBorisOrel.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy, string oBy)
        {
            if (gBy == "CDesc")
            {
                var userNamesByID =
                   from u in _context.Categories
                   group u by u.Category_Description into g
                   select new { Category_Description = g.Key, count = g.Count(), g.First().Category_Name };
                var group = new List<Categories>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Categories()
                    {
                        Category_Name = t.Category_Name,
                        Counter = t.count,
                        Category_Description = t.Category_Description

                    });
                }

                return View(group);
            }
            else if (gBy == "Cname")
            {
                var userNamesByID =
                   from u in _context.Categories
                   group u by u.Category_Name into g
                   select new { Category_Name = g.Key, count = g.Count(), g.First().Category_Description };
                var group = new List<Categories>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Categories()
                    {
                        Category_Name = t.Category_Name,
                        Counter = t.count,
                        Category_Description = t.Category_Description

                    });
                }

                return View(group);
            }
            else if (jBy == "post")
            {
                var join =
                from u in _context.Categories

                join p in _context.Post on u.ID equals p.categoryID

                select new { u.Category_Name, u.Category_Description };

                var UserList = new List<Categories>();
                foreach (var t in join)
                {
                    UserList.Add(new Categories()
                    {
                        Category_Name = t.Category_Name,
                        Category_Description = t.Category_Description
                    });
                }
                return View(UserList);
            }
            else if (oBy == "cName")
            {


                var categories = from s in _context.Categories
                               select s;

                categories = categories.OrderBy(s => s.Category_Name);


                return View(categories.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

            }

            else if (oBy == "fName")
            {


                var categories = from s in _context.Categories
                                 select s;

                categories = categories.OrderBy(s => s.First_Name);


                return View(categories.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

            }

            else
            {

                var categories = from s in _context.Categories
                                 select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    categories = categories.Where(s => s.Category_Name.Contains(searchString));
                }

                if (!String.IsNullOrEmpty(searchString2))
                {

                    categories = categories.Where(s => s.Category_Description.Contains(searchString2));
                }

                if (!String.IsNullOrEmpty(searchString3))
                {
                    categories = categories.Where(s => s.First_Name.Contains(searchString3));
                }

                return View(categories.ToList());
            }
            //return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .Include(c => c.Posts)

                .SingleOrDefaultAsync(m => m.ID == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Category_Name,Category_Description,First_Name,Last_Name")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Category_Name,Category_Description,First_Name,Last_Name")] Categories categories)
        {
            if (id != categories.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.ID))
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
            return View(categories);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .SingleOrDefaultAsync(m => m.ID == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.SingleOrDefaultAsync(m => m.ID == id);
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}

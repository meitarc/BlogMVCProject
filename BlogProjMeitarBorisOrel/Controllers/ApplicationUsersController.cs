using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjMeitarBorisOrel.Data;
using BlogProjMeitarBorisOrel.Models;

namespace BlogProjMeitarBorisOrel.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApplicationUsers
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy)
        {
            if (gBy == "Fname")
            {
                var userNamesByID =
                   from u in _context.User2
                   group u by u.First_Name into g
                   select new { First_Name = g.Key, count = g.Count(), g.First().Last_Name };
                var group = new List<ApplicationUser>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new ApplicationUser()
                    {
                        First_Name = t.First_Name,
                        Counter = t.count,
                        Last_Name = t.Last_Name

                    });
                }

                return View(group);
            }
            else if (gBy == "Lname")
            {
                var userNamesByID =
                    from u in _context.User2
                    group u by u.Last_Name into g
                    select new { First_Name = g.Key, count = g.Count(), g.First().Last_Name };
                var group = new List<ApplicationUser>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new ApplicationUser()
                    {
                        First_Name = t.First_Name,
                        Counter = t.count,
                        Last_Name = t.Last_Name

                    });
                }

                return View(group);
            }
            else if (jBy == "post")
            {
                var join =
                from u in _context.User2

                join p in _context.Post on u.Id equals p.ApplicationUserID

                select new { u.First_Name, u.Last_Name, p.Title};

                var UserList = new List<ApplicationUser>();
                foreach (var t in join)
                {
                    UserList.Add(new ApplicationUser()
                    {
                        First_Name = t.First_Name,
                        Last_Name = t.Last_Name,
                        Title = t.Title
                
                    });
                }
                return View(UserList);
            }
            else if (jBy == "comment")
            {
                var join =
                from u in _context.User2

                join p in _context.Comment on u.Id equals p.ApplicationUserID

                select new { u.First_Name, u.Last_Name, p.Title };

                var UserList = new List<ApplicationUser>();
                foreach (var t in join)
                {
                    UserList.Add(new ApplicationUser()
                    {
                        First_Name = t.First_Name,
                        Last_Name = t.Last_Name,
                        Title = t.Title

                    });
                }
                return View(UserList);
            }
            else
            {

                var appUser = from s in _context.User2
                            select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    appUser = appUser.Where(s => s.First_Name.Contains(searchString));
                }

                if (!String.IsNullOrEmpty(searchString2))
                {

                    appUser = appUser.Where(s => s.Last_Name.Contains(searchString2));
                }

                if (!String.IsNullOrEmpty(searchString3))
                {
                    appUser = appUser.Where(s => s.Country.Contains(searchString3));
                }
                return View(appUser.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());
            }

            //return View(await _context.User2.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.User2
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("First_Name,Last_Name,Country,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.User2.SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("First_Name,Last_Name,Country,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.User2
                .SingleOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationUser = await _context.User2.SingleOrDefaultAsync(m => m.Id == id);
            _context.User2.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.User2.Any(e => e.Id == id);
        }
    }
}

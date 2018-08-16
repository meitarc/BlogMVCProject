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
using Microsoft.AspNetCore.Identity;

namespace BlogProjMeitarBorisOrel.Controllers
{
    
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
      
        // GET: Posts
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy)
        {
            if (gBy == "Aname")
            {
                var userNamesByID =
                   from u in _context.Post
                   group u by u.Author_Name into g
                   select new { Author_Name = g.Key, count = g.Count(), g.First().Title };
                var group = new List<Post>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Post()
                    {
                        Author_Name = t.Author_Name,
                        Counter = t.count,
                        Title = t.Title

                    });
                }

                return View(group);
            }
            else if (gBy == "title")
            {
                var userNamesByID =
                   from u in _context.Post
                   group u by u.Title into g
                   select new { Title = g.Key, count = g.Count(), g.First().Author_Name };
                var group = new List<Post>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Post()
                    {
                        Title = t.Title,
                        Counter = t.count,
                        Author_Name = t.Author_Name

                    });
                }

                return View(group);
            }
            else if (jBy == "category")
            {
                var join =
                from u in _context.Post

                join p in _context.Categories on u.categoryID equals p.ID

                select new { u.Author_Name, u.Title };

                var UserList = new List<Post>();
                foreach (var t in join)
                {
                    UserList.Add(new Post()
                    {
                        Title = t.Title,
                        Author_Name = t.Author_Name
                    });
                }
                return View(UserList);
            }
            else if (jBy == "comment")
            {
                var join =
                from u in _context.Post

                join p in _context.Comment on u.ID equals p.PostID

                select new { u.Author_Name, u.Title };

                var UserList = new List<Post>();
                foreach (var t in join)
                {
                    UserList.Add(new Post()
                    {
                        Title = t.Title,
                        Author_Name = t.Author_Name
                    });
                }
                return View(UserList);
            }
            else
            { 

            var posts = from s in _context.Post
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(searchString2))
            {
                  
                posts = posts.Where(s => s.Author_Name.Contains(searchString2));
            }

            if (!String.IsNullOrEmpty(searchString3))
            {
                posts = posts.Where(s => s.Text.Contains(searchString3));
            }
                ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            return View(posts.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Posts/Details/5
        public void AddLike(Post post)
        {
            post.NumOfLikes++;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["categoryID"] = new SelectList(_context.Set<Categories>(), "ID", "Category_Name");
            var post = await _context.Post.Include(p => p.Categories)
                .Include(p => p.Comments).AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            

            if (post == null)
            {
                return NotFound();
            }
            AddLike(post);
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            if (_userManager.GetUserId(HttpContext.User) != null)
            {
                ViewBag.userid = _userManager.GetUserId(HttpContext.User);
            }
            else
            {
                ViewBag.userid = "Guest";
            }

            ViewData["categoryID"] = new SelectList(_context.Set<Categories>(), "ID", "Category_Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,categoryID,ApplicationUserID,PublishedDate,Title,Author_Name,Text,NumOfLikes,Lat,Lng")] Post post)
        {

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["categoryID"] = new SelectList(_context.Set<Categories>(), "ID", "Category_Name");
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["categoryID"] = new SelectList(_context.Set<Categories>(), "ID", "Category_Name");
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,categoryID,ApplicationUserID,PublishedDate,Title,Author_Name,Text,NumOfLikes,Lat,Lng")] Post post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
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

            ViewData["categoryID"] = new SelectList(_context.Set<Categories>(), "ID", "Category_Name");
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Categories)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.SingleOrDefaultAsync(m => m.ID == id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID == id);
        }
    }
}

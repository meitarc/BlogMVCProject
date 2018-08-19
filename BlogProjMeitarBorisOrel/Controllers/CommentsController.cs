using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjMeitarBorisOrel.Data;
using BlogProjMeitarBorisOrel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BlogProjMeitarBorisOrel.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Comments
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy, string oBy)
        {
            if (gBy == "Aname")
            {
                var userNamesByID =
                   from u in _context.Comment
                   group u by u.Author_Name into g
                   select new { Author_Name = g.Key, count = g.Count(), g.First().Title };
                var group = new List<Comment>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Comment()
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
                   from u in _context.Comment
                   group u by u.Title into g
                   select new { Title = g.Key, count = g.Count(), g.First().Author_Name };
                var group = new List<Comment>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new Comment()
                    {
                        Title = t.Title,
                        Counter = t.count,
                        Author_Name = t.Author_Name

                    });
                }

                return View(group);
            }
            else if (jBy == "post")
            {
                var join =
                from u in _context.Comment

                join p in _context.Post on u.PostID equals p.ID

                select new { u.Author_Name, u.Title };

                var UserList = new List<Comment>();
                foreach (var t in join)
                {
                    UserList.Add(new Comment()
                    {
                        Title = t.Title,
                        Author_Name = t.Author_Name
                    });
                }
                return View(UserList);
            }
            else if (oBy == "title")
            {


                var comments = from s in _context.Comment
                            select s;

                comments = comments.OrderBy(s => s.Title);


                return View(comments.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

            }
            else if (oBy == "author")
            {


                var comments = from s in _context.Comment
                            select s;

                comments = comments.OrderBy(s => s.Author_Name);


                return View(comments.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

            }
            else
            {
                var comms = from s in _context.Comment
                            select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    comms = comms.Where(s => s.Title.Contains(searchString));
                }

                if (!String.IsNullOrEmpty(searchString2))
                {
                    comms = comms.Where(s => s.Text.Contains(searchString2));
                }

                if (!String.IsNullOrEmpty(searchString3))
                {
                    comms = comms.Where(s => s.Author_Name.Contains(searchString3));
                }

                return View(comms.ToList());

                //var applicationDbContext = _context.Comment.Include(c => c.Post).Include(c => c.User);
                //return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Post)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }
           
            return View(comment);
        }

        // GET: Comments/Create
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
            ViewData["PostID"] = new SelectList(_context.Set<Post>(), "ID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ApplicationUserID,PostID,PublishedDate,Title,Author_Name,Text,NumOfLikes")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostID"] = new SelectList(_context.Set<Post>(), "ID", "Title", comment.PostID);
            return View(comment);
        }
        [Authorize(Roles = "Admin")]

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["PostID"] = new SelectList(_context.Set<Post>(), "ID", "Title", comment.PostID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ApplicationUserID,PostID,PublishedDate,Title,Author_Name,Text,NumOfLikes")] Comment comment)
        {
            if (id != comment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.ID))
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
            ViewData["PostID"] = new SelectList(_context.Set<Post>(), "ID", "Title", comment.PostID);
            return View(comment);
        }
        [Authorize(Roles = "Admin")]

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Post)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.SingleOrDefaultAsync(m => m.ID == id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.ID == id);
        }
    }
}

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
using Microsoft.AspNetCore.Authorization;
using Accord.MachineLearning.Rules;

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
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy, string oBy)
        {
            // Example from https://en.wikipedia.org/wiki/Apriori_algorithm

            // Assume that a large supermarket tracks sales data by stock-keeping unit
            // (SKU) for each item: each item, such as "butter" or "bread", is identified 
            // by a numerical SKU. The supermarket has a database of transactions where each
            // transaction is a set of SKUs that were bought together.

            // Let the database of transactions consist of following itemsets:

            //SortedSet<int>[] dataset =
            //{
            //    // Each row represents a set of items that have been bought 
            //    // together. Each number is a SKU identifier for a product.
            //    new SortedSet<int> { 1, 2, 3, 4 }, // bought 4 items
            //    new SortedSet<int> { 1, 2, 4 },    // bought 3 items
            //    new SortedSet<int> { 1, 2 },       // bought 2 items
            //    new SortedSet<int> { 2, 3, 4 },    // ...
            //    new SortedSet<int> { 2, 3 },
            //    new SortedSet<int> { 3, 4 },
            //    new SortedSet<int> { 2, 4 },
            //};
            //SortedSet<int> dataset2 = new SortedSet<int>();
            var x = _context.User2.Include(user => user.Posts).Select(userPost => userPost.Posts).ToList();
            var y = x.Select(delegate (ICollection<Post> posts2)
            {
                return posts2.AsQueryable().Select(post => post.categoryID).ToArray();
            });
            var categories2 = y.ToArray();
            // We will use Apriori to determine the frequent item sets of this database.
            // To do this, we will say that an item set is frequent if it appears in at 
            // least 3 transactions of the database: the value 3 is the support threshold.

            // Create a new a-priori learning algorithm with support 3
            Apriori apriori = new Apriori(threshold: 1, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(categories2);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought items 1 and 2 together:
            int[][] matches = classifier.Decide(new[] { 1 });
            List<Post> postsP = new List<Post>();
            HashSet<int> hs = new HashSet<int>();

            foreach (int[] e in matches)
            {
                foreach (int f in e)
                {
                    hs.Add(f);
                }

            }
            foreach (int id in hs)
            {
                postsP.Add(_context.Post.FirstOrDefault(a => a.categoryID == id));
            }

            // The result should be:
            // 
            //   new int[][]
            //   {
            //       new int[] { 4 },
            //       new int[] { 3 }
            //   };

            // Meaning the most likely product to go alongside the products
            // being bought is item 4, and the second most likely is item 3.

            // We can also obtain the association rules from frequent itemsets:
            AssociationRule<int>[] rules = classifier.Rules;
            // The result will be:
            // {
            //     [1] -> [2]; support: 3, confidence: 1, 
            //     [2] -> [1]; support: 3, confidence: 0.5, 
            //     [2] -> [3]; support: 3, confidence: 0.5, 
            //     [3] -> [2]; support: 3, confidence: 0.75, 
            //     [2] -> [4]; support: 4, confidence: 0.66, 
            //     [4] -> [2]; support: 4, confidence: 0.8, 
            //     [3] -> [4]; support: 3, confidence: 0.75, 
            //     [4] -> [3]; support: 3, confidence: 0.6 
            // };
            ViewBag.postsP = postsP.Take(3).ToList();

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
            else if (oBy == "title")
            {


                var posts = from s in _context.Post
                            select s;

                posts = posts.OrderBy(s => s.Title);


                return View(posts.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

            }
            else if (oBy == "author")
            {


                var posts = from s in _context.Post
                            select s;

                posts = posts.OrderBy(s => s.Author_Name);


                return View(posts.ToList());
                //var applicationDbContext = _context.Post.Include(p => p.User);
                //return View(await applicationDbContext.ToListAsync());

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
       
        [HttpGet]
        public JsonResult Graph()
        {
            var h = from c in _context.Categories
                    join p in _context.Post
                    on c.ID equals p.categoryID into gr
                    select new categoryCount { Name = c.Category_Name, Count = gr.Count() };
            List<categoryCount> mylist = new List<categoryCount>();
            foreach(var x in h)
            {
                var myjson = new categoryCount { Name = x.Name, Count = x.Count };
                mylist.Add(myjson);
            }
            mylist = mylist.OrderByDescending(x => x.Count).Take(4).ToList();
            return Json(mylist);
            

        }
    }
}

//return json with 10 most used tags
//    public JsonResult getJson10MostUsedTags()
//    {
//        var bh = _context.Post.GroupBy(x => x.categoryID);
//        List<TagCount> mylist = new List<TagCount>();
//        foreach (var x in bh)
//        {
//            var myjson = new TagCount { Name = _context.Post.Where(t => t.categoryID == x.Key).FirstOrDefault().Title, Count = x.Count() };
//            mylist.Add(myjson);
//        }
//        mylist.OrderBy(x => x.Count).Take(10);
//        return Json(mylist);
//    }
//}
//public class TagCount
//{
//    public string Name { get; set; }
//    public int Count { get; set; }
//}

//[HttpGet]
////return json with 10 most used tags
//public JsonResult getJsontop10Categories()
//{

//    var p =
//    from u in _context.Post
//    group u by u.categoryID into g
//    select new { categoryID = g.Key};
//    List<categoryIDCount> mylist = new List<categoryIDCount>();
//    foreach (var x in p)
//    {
//        var myjson = new categoryIDCount { Name = _context.Tag.Where(t => t.TagID == x.Key).FirstOrDefault().Name, Count = x.Count() };
//        mylist.Add(myjson);
//    }
//    mylist.OrderBy(x => x.Count).Take(10);
//    return Json(mylist);
//}

//public JsonResult getJson10MostUsedTags()
//{
//    var p =
//    from u in _context.Post
//    group u by u.categoryID into g
//    select new { categoryID = g.Key, count = g.Count() };


//    // var bh = _context.categoryID.GroupBy(x => x.TagID);
//    List<categoryIDCount> mylist = new List<categoryIDCount>();
//    foreach (var x in p)
//    {
//        var myjson = new categoryIDCount { Name = _context.Tag.Where(t => t.TagID == x.Key).FirstOrDefault().Name, Count = x.Count() };
//        mylist.Add(myjson);
//    }
//    mylist.OrderBy(x => x.Count).Take(10);
//    return Json(mylist);
//}






public class categoryCount
{
    public string Name { get; set; }
    public int Count { get; set; }
}


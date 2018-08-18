using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogProjMeitarBorisOrel.Data;
using BlogProjMeitarBorisOrel.Models;
using Accord.MachineLearning.Rules;
using BlogProjMeitarBorisOrel.Models.Blog;
using Microsoft.AspNetCore.Authorization;

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
            // Example from https://en.wikipedia.org/wiki/Apriori_algorithm

            // Assume that a large supermarket tracks sales data by stock-keeping unit
            // (SKU) for each item: each item, such as "butter" or "bread", is identified 
            // by a numerical SKU. The supermarket has a database of transactions where each
            // transaction is a set of SKUs that were bought together.

            // Let the database of transactions consist of following itemsets:

            SortedSet<int>[] dataset =
            {
                // Each row represents a set of items that have been bought 
                // together. Each number is a SKU identifier for a product.
                new SortedSet<int> { 1, 2, 3, 4 }, // bought 4 items
                new SortedSet<int> { 1, 2, 4 },    // bought 3 items
                new SortedSet<int> { 1, 2 },       // bought 2 items
                new SortedSet<int> { 2, 3, 4 },    // ...
                new SortedSet<int> { 2, 3 },
                new SortedSet<int> { 3, 4 },
                new SortedSet<int> { 2, 4 },
            };
            SortedSet<int> dataset2 = new SortedSet<int>();
            var x = _context.User2.Include(user => user.Posts).Select(userPost => userPost.Posts).ToList();
            var y = x.Select(delegate (ICollection<Post> posts)
            {
                return posts.AsQueryable().Select(post => post.categoryID).ToArray();
            });
            var categories2 = y.ToArray();
            // We will use Apriori to determine the frequent item sets of this database.
            // To do this, we will say that an item set is frequent if it appears in at 
            // least 3 transactions of the database: the value 3 is the support threshold.

            // Create a new a-priori learning algorithm with support 3
            Apriori apriori = new Apriori(threshold: 3, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(categories2);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought items 1 and 2 together:
            int[][] matches = classifier.Decide(new[] { 1, 2 });
            
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
            ViewBag.category = classifier;

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
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

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
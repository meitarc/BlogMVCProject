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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Post> _PostList;
        private List<Comment> _CommentList;
        private List<User> _UserList;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Init()
        {
            _PostList = new List<Post>
            {
                new Post{ID=1,categoryID=7,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="aaa",Author_Name="ggg",Text="mmm",UrlImage="sss"},
                new Post{ID=2,categoryID=8,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="bbb",Author_Name="hhh",Text="nnn",UrlImage="ttt"},
                new Post{ID=3,categoryID=9,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="ccc",Author_Name="iii",Text="ooo",UrlImage="uuu"},
                new Post{ID=4,categoryID=10,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="ddd",Author_Name="jjj",Text="ppp",UrlImage="yyy"},
                new Post{ID=5,categoryID=11,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="eee",Author_Name="kkk",Text="qqq",UrlImage="www"},
                new Post{ID=6,categoryID=12,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="fff",Author_Name="lll",Text="rrr",UrlImage="xxx"}
            };
            _CommentList = new List<Comment>
            {
                new Comment{ID=1,PostID=1,UserID=7,PublishedDate=new DateTime(2013,02,02,02,02,02),Title="aaa",Author_Name="ggg",Text="mmm"},
                new Comment{ID=2,PostID=2,UserID=8,PublishedDate= new DateTime(2013,10,10,07,08,05),Title="bbb",Author_Name="hhh",Text="nnn"},
                new Comment{ID=3,PostID=3,UserID=9,PublishedDate=new DateTime(2013,10,10,07,08,05),Title="ccc",Author_Name="iii",Text="ooo"},
                new Comment{ID=4,PostID=4,UserID=10,PublishedDate=new DateTime(2013,10,10,07,08,05),Title="ddd",Author_Name="jjj",Text="ppp"},
                new Comment{ID=5,PostID=5,UserID=11,PublishedDate=new DateTime(2013,10,10,07,08,05),Title="eee",Author_Name="kkk",Text="qqq"},
                new Comment{ID=6,PostID=6,UserID=12,PublishedDate=new DateTime(2015,10,10,4,5,6),Title="fff",Author_Name="lll",Text="rrr"}
            };
            _UserList = new List<User>
            {
                new User{ID=7,User_Name="aaa",First_Name="hhh",Last_Name="mmm",Email="sss",Password="zzz",ConfirmPassword="zzz",Is_Admin=false},
                new User{ID=8,User_Name="bbb",First_Name="hhh",Last_Name="nnn",Email="ttt",Password="mmm",ConfirmPassword="mmm",Is_Admin=false},
                new User{ID=9,User_Name="ccc",First_Name="iii",Last_Name="ooo",Email="uuu",Password="aaa",ConfirmPassword="aaa",Is_Admin=false},
                new User{ID=10,User_Name="ddd",First_Name="jjj",Last_Name="ppp",Email="yyy",Password="bbb",ConfirmPassword="bbb",Is_Admin=false},
                new User{ID=11,User_Name="eee",First_Name="kkk",Last_Name="qqq",Email="www",Password="ccc",ConfirmPassword="ccc",Is_Admin=false},
                new User{ID=12,User_Name="fff",First_Name="lll",Last_Name="rrr",Email="xxx",Password="ddd",ConfirmPassword="ddd",Is_Admin=false}




            };
        }
        // GET: Users
        public async Task<IActionResult> Index(string searchString, string searchString2, string searchString3, string gBy, string jBy)
        {
            //Init();

            //var userNamesByID =
            //        from u in _UserList
            //        group u by u.First_Name into g
            //        select new { First_Name = g.Key, _UserList = g, count=g.Count() };
            //var group = new List<User>();
            //foreach (var t in userNamesByID)
            //{
            //    group.Add(new User()
            //    {
            //        First_Name = t.First_Name,
            //        Counter=t.count
            //    });
            //}
            if (gBy == "fname")
            {
                   var userNamesByID =
                      from u in _context.User
                      group u by u.First_Name into g
                      select new { First_Name = g.Key, count = g.Count(), g.First().Last_Name };
                var group = new List<User>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new User()
                    {
                        First_Name = t.First_Name,
                        Counter = t.count,
                        Last_Name = t.Last_Name
                    });
                }
                return View(group);
            }else if(gBy=="lname")
            {
                var userNamesByID =
                      from u in _context.User
                      group u by u.Last_Name into g
                      select new { First_Name = g.Key, count = g.Count(), g.First().Last_Name };
                var group = new List<User>();
                foreach (var t in userNamesByID)
                {
                    group.Add(new User()
                    {
                        First_Name = t.First_Name,
                        Counter = t.count,
                        Last_Name = t.Last_Name
                    });
                }
                return View(group);
            }
            //else if(jBy=="post")
            //{
            //    var join =
            //    from u in _context.User
            //    join p in _context.Post on u.ID equals p.UserID
            //    select new { u.User_Name, u.First_Name };

            //    var UserList = new List<User>();
            //    foreach (var t in join)
            //    {
            //        UserList.Add(new User()
            //        {
            //            User_Name = t.User_Name,
            //            First_Name = t.First_Name
            //        });
            //    }
            //    return View(UserList);
            //}
            else
            {

            var users = from s in _context.User
                        select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.User_Name.Contains(searchString));
                users = users.OrderBy(s => s.User_Name);
            }
            if (!String.IsNullOrEmpty(searchString2))
            {
                users = users.Where(s => s.First_Name.Contains(searchString2));
                users = users.OrderBy(s => s.First_Name);
            }
            if (!String.IsNullOrEmpty(searchString3))
            {
                users = users.Where(s => s.Last_Name.Contains(searchString3));
                users = users.OrderBy(s => s.Last_Name);
            }
                return View(users.ToList());
            }
              
            /*var join =
                from u in _UserList
                join p in _PostList on u.ID equals p.UserID
                select new { u.User_Name, u.First_Name};

            var UserList = new List<User>();
            foreach (var t in join)
            {
                UserList.Add(new User()
                {
                    User_Name = t.User_Name,
                    First_Name = t.First_Name
                });
            }
            return View(UserList);*/



            //return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User_Name,First_Name,Last_Name,Email,Password,ConfirmPassword,Is_Admin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,User_Name,First_Name,Last_Name,Email,Password,ConfirmPassword,Is_Admin")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.ID == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}

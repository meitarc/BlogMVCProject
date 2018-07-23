using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjMeitarBorisOrel.Models;

namespace ProjMeitarBorisOrel.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext (DbContextOptions<BlogContext> options)
            : base(options)
        {
        }

        public DbSet<ProjMeitarBorisOrel.Models.Comment> Comment { get; set; }

        public DbSet<ProjMeitarBorisOrel.Models.Post> Post { get; set; }

        public DbSet<ProjMeitarBorisOrel.Models.User> User { get; set; }
    }
}

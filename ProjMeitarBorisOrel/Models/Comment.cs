using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjMeitarBorisOrel.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Title { get; set; }
        public string Author_Name { get; set; }
        public string Text { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}

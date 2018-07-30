using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjMeitarBorisOrel.Models
{
    public class Comment
    {
        private DateTime _date = DateTime.Now;

        public int ID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }

        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get { return _date; } set { _date = value; } }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        [Display(Name = "Author Name")]
        public string Author_Name { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}

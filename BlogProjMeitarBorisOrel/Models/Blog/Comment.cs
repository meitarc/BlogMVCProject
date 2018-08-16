using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjMeitarBorisOrel.Models
{
    public class Comment
    {
        private DateTime _date = DateTime.Now;
        [Key]
        public int ID { get; set; }
        public string ApplicationUserID { get; set; }

        public int PostID { get; set; }
        [NotMapped]
        public int Counter { get; set; }

        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get { return _date; } set { _date = value; } }
         
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        [Display(Name = "Author Name")]
        public string Author_Name { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }
        public int NumOfLikes { get; set; }
        public virtual ApplicationUser AppUser { get; set; }

        public virtual Post Post { get; set; }
    }
}

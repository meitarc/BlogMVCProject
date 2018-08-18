using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjMeitarBorisOrel.Models.Blog
{
    public class Categories
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [Display(Name = "Category Name")]
        public string Category_Name { get; set; }
        [Display(Name = "Category Description")]
        public string Category_Description { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last Name  is required")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [NotMapped]
        public int Counter { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}

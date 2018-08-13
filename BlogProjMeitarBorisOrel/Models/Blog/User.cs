using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjMeitarBorisOrel.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage="User Name is required")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 15 characters")]
        [Display(Name = "User Name")]
        public string User_Name { get; set; }

        [NotMapped]
        public int Counter { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name  is required")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Email  is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Admin?")]
        public bool Is_Admin { get; set; }
       
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        
    }
}

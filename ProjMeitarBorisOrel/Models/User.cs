using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjMeitarBorisOrel.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage="User Name is required")]
        public string User_Name { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last Name  is required")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Is_Admin { get; set; }
        [Required(ErrorMessage = "Email  is required")]
        public string Email { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        
    }
}

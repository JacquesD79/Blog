using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models {
    public class UserProfile {

        [Key]
        public int UserID { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Password { get; set; }

        [Column(TypeName = "bit")]
        public bool IsActive { get; set; }

    }
}

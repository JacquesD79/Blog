using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models {
    public class Post {

        public Post[] data { get; set; }

        [Key]
        public int PostID { get; set; }

        [Column(TypeName = "varchar(150)")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Column(TypeName = "varchar(1000)")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        [JsonProperty("publication_date")]
        public DateTime PublicationDate { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string User { get; set; }
    }
}

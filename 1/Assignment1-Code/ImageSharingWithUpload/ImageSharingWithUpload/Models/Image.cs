using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithUpload.Models
{
    public class Image
    {
        // TODO add error messages for validation

        [Required(ErrorMessage = "A image id is required")]
        [RegularExpression(@"[a-zA-Z0-9_]+")]
        public String Id {get; set; }
        [Required(ErrorMessage = "A caption is required")]
        [StringLength(40)]
        public String Caption {get; set;}
        [StringLength(200)]
        public String Description { get; set; }
        [Required(ErrorMessage = "A datetaken is required")]
        [DataType(DataType.Date)]
        public DateTime DateTaken { get; set; }
        public String Userid { get; set; }

        public Image()
        {
        }
    }
}
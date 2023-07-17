using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class SocialMedia:BaseModel
    {
        [Required]
        public string Link { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

    }

}

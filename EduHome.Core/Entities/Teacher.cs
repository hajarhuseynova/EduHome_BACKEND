using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Teacher:BaseModel
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
  
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        [Required]
        public int TeacherPositionCatId { get; set; }
        public TeacherPositionCat? teacherPositionCat { get; set; }
        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }    
        public List<SocialMedia>? SocialMedias { get; set; }
        public List<Skills>? Skills { get; set; }
        public string Desc { get; set; }
        public string Degree { get; set; }  
        public int ExperienceTime { get; set; } 
        public string? Hobbies { get; set; }
        public string Mail { get; set; }
        public string Skype { get; set; }
        public string Phone { get; set; }



    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Course:BaseModel
    {
        public string Info { get; set; }
        public string Link { get; set; }    
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public int CourseCategoryId { get; set; }   
        public CourseCategory? CourseCategory { get; set; }
    }
}

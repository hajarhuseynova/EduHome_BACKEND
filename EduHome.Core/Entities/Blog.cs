using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Blog:BaseModel
    {
        public string Name { get; set; }    
        public string Icon { get; set; }
        public string Desc { get; set; }
        public string Title { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }

        public List<BlogTag> BlogTags { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        public int CourseCategoryId { get; set; }
        public CourseCategory? CourseCategory { get; set; }

    }
}

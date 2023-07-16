using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Course : BaseModel
    {

        public string AboutText { get; set; }
        public string Certification { get; set; }
        public string ApplyText { get; set; }
        public string Info { get; set; }
        public string? Image { get; set; }
    
        public int CourseCategoryId { get; set; }
        public CourseCategory? CourseCategory { get; set; }

        public Feature? Feature { get; set; }
        public int FeatureId { get; set; }


        public List<CourseTag>? CourseTags { get; set; }


        [NotMapped]
        public List<int> TagIds {get;set;}
        [NotMapped]
        public IFormFile? FormFile { get; set; }

    }
}

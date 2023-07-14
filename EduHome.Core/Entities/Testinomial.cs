using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Testinomial:BaseModel
    {
        public string Desc { get; set; }
        public string PersonName { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public int CourseCategoryId { get; set; }
        public CourseCategory? CourseCategory { get; set; }

        public int PositionCategoryId { get; set; }
        public PositionCategory? PositionCategory { get; set; }
    }
}

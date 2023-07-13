using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class CourseCategory:BaseModel
    {
        public string Name { get; set; }    
        public List<Course>? Courses { get; set; }
    }
}

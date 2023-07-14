using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class TeacherPositionCat:BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

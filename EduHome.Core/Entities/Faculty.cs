using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Faculty:BaseModel
    {
        public string Name { get; set; }    
        public List<Teacher>? Teachers { get; set; }
    }
}

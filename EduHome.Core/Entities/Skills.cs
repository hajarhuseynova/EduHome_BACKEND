using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Skills:BaseModel
    {
        public string Name { get; set; }    
        public int TeacherId { get; set; }  
        public Teacher? Teacher { get; set; }    
        public string Percent { get; set; } 
    }
}

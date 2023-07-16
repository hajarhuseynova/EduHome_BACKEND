using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Feature:BaseModel
    {
        public DateTime StartTime { get; set; } 
        public int Duration { get; set; }   
        public int ClassDuration { get; set; }
        public int StudentCount { get; set; }
        public int Fee { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public string Assesments { get; set; }

        //goturmur
        //public Course Course { get; set; }  
        //public int CourseId { get; set; }

    }
}

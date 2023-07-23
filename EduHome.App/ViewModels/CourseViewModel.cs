using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class CourseViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<CourseCategory> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public Course Course { get; set; }
       


    }
}

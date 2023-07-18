using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class BlogViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<CourseCategory> Categories { get; set; }

        public Blog Blog { get; set; }
       

     


    }
}

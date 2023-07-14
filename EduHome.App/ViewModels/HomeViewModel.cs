using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<About> Abouts { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<NoticeBoard> NoticeBoards { get; set; }
        public IEnumerable<Testinomial> Testinomials { get; set; }

        public IEnumerable<PositionCategory> PositionCategories { get; set; }


        public IEnumerable<CourseCategory> CourseCategories { get; set; }


    }
}

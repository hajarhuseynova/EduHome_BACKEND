using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class HomeViewModel
    {
    
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Setting> Settings { get; set; }

        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<NoticeBoard> NoticeBoards { get; set; }
        public IEnumerable<Testinomial> Testinomials { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; }
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Skills> Skills { get; set; }

        public IEnumerable<SocialMedia> SocialMedias { get; set; }
        public IEnumerable<TeacherPositionCat> TeacherPositionCats { get; set; }

        public IEnumerable<PositionCategory> PositionCategories { get; set; }


        public IEnumerable<CourseCategory> CourseCategories { get; set; }


    }
}

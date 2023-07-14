using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext:DbContext
    {
        public DbSet<About> Abouts { get; set; }
        public DbSet<Slider> Slides { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherPositionCat> TeacherPositionCats { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<PositionCategory> PositionCategories { get; set; }
        public DbSet<Testinomial> Testinomials { get; set; }

        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options)
        {

        }
    }

}

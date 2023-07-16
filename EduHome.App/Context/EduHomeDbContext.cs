using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext: IdentityDbContext<AppUser>
    {
        public DbSet<Slider> Slides { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherPositionCat> TeacherPositionCats { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<PositionCategory> PositionCategories { get; set; }
        public DbSet<Testinomial> Testinomials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
               .HasOne(e => e.Feature)
           .WithOne(e => e.Course)
               .HasForeignKey<Feature>();
            base.OnModelCreating(modelBuilder);
        }
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options)
        {

        }
    }

}

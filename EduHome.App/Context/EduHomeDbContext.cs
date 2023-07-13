using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext:DbContext
    {
        public DbSet<About> Abouts { get; set; }
        public DbSet<Slider> Slides { get; set; }
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options)
        {

        }
    }

}

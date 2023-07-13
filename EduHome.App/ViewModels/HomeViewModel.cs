using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<About> Abouts { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }

    }
}

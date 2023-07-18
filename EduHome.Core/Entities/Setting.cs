using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Setting:BaseModel
    {
        public string VideoLink { get; set; }
        public string Title { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string ButtonLink { get; set; }
        public string? AboutImage { get; set; }
        [NotMapped]
        public IFormFile? AboutFormFile { get; set; }
        public string? Logo { get; set; }
        [NotMapped]
        public IFormFile? LogoFormFile { get; set; }
        public string Address { get; set; }
        public string CityCountry { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public string Link4 { get; set; }
        public string Icon1 { get; set; }
        public string Icon2 { get; set; }
        public string Icon3 { get; set; }
        public string Icon4 { get; set; }





    }
}

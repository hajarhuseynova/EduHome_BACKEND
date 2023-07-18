using EduHome.App.Context;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Services.Implementations
{
    public class SettingService:ISettingService
    {
        private readonly EduHomeDbContext _context;

        public SettingService(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<Setting?> Get()
        {
            Setting? setting = await _context.Settings.FirstOrDefaultAsync();
            return setting;
        }

    }
}

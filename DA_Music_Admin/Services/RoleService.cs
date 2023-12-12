using DA_Music_Admin;
using Microsoft.EntityFrameworkCore;
using Services.IServices;

namespace Services
{
    public class RoleService : IRoleService
    {
        public Task<Role> CreateObject(Role data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Role>> GetListAlls()
        {
            var _context = new MusicContext();
            return await _context.Set<Role>().AsNoTracking()
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public Task<Role> GetObjectById(params object[] id)
        {
            throw new NotImplementedException();
        }
    }
}

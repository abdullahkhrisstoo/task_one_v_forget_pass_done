using Microsoft.EntityFrameworkCore;
using task_one_v2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_one_v2.App_Core.UserRole
{
    public enum UserRoleType
    {
        Admin,
        User,
        Chef
    }

    public class UserRoleService
    {
        private readonly ModelContext _context;

        public UserRoleService(ModelContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetRolesAsync() => await _context.Roles.ToListAsync();
        

        public async Task<int?> GetRoleIdAsync(UserRoleType roleType)
        {
            var roleName = roleType.ToString().ToLower();
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName);
            return (int?)(role?.RoleId);
        }
    }
}

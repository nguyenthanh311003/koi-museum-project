using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository() { }

        public UserRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }   
    }
}

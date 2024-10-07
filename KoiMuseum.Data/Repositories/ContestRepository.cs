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
    public class ContestRepository : GenericRepository<Contest>
    {
        public ContestRepository() { }

        public ContestRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<Contest> GetContestByStatus(string status)
        {
            return await _context.Contests.FirstOrDefaultAsync(c => c.Status.Equals(status));
        }
    }
}

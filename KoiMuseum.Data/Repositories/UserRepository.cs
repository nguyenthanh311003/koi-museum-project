using KoiMuseum.Data.Base;
using KoiMuseum.Data.Dtos.Requests.User;
using KoiMuseum.Data.Dtos.Responses.Judge;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async Task<List<JudgeUserResponse>> GetJudgeUser(string searchTerm = "")
        {
            return await _context.Users
                .Where(x => x.Role == "Judge" &&
                    (string.IsNullOrEmpty(searchTerm) ||
                     x.Name.Contains(searchTerm) ||
                     x.Email.Contains(searchTerm) ||
                     x.PhoneNumber.Contains(searchTerm) ||
                     x.Judges.Any(j => j.AssignedContests.Contains(searchTerm))
                     ))
                .Include(x => x.Judges)
                .Select(user => new JudgeUserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = user.Password,
                    Email = user.Email,
                    Role = user.Role,
                    Description = user.Description,
                    AvatarUrl = user.AvatarUrl,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Judges = user.Judges.Select(j => new JudgeDto
                    {
                        Id = j.Id,
                        UserId = j.UserId,
                        Experience = j.Experience,
                        Certifications = j.Certifications,
                        AssignedContests = j.AssignedContests,
                        Status = j.Status
                    }).Where(x => x.AssignedContests.Contains(searchTerm)).ToList()
                })
                .ToListAsync();
        }

        public async Task<JudgeUserResponse> GetJudgeUserByIdAsync(int id)
        {
            return await _context.Users.Where(x => x.Id == id).Include(x => x.Judges)
                .Select(user => new JudgeUserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = user.Password,
                    Email = user.Email,
                    Role = user.Role,
                    Description = user.Description,
                    AvatarUrl = user.AvatarUrl,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Judges = user.Judges.Select(j => new JudgeDto
                    {
                        Id = j.Id,
                        UserId = j.UserId,
                        Experience = j.Experience,
                        Certifications = j.Certifications,
                        AssignedContests = j.AssignedContests,
                        Status = j.Status
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<Boolean> UpdateJudge(UpdateJudgeUserRequest judge)
        {
            try
            {
                var existingJudge = await _context.Judges
                    .FirstOrDefaultAsync(j => j.UserId == judge.UserId && j.AssignedContests == judge.AssignedContests);

                if (existingJudge != null)
                {
                    return false;
                }

                var oldJudge = await _context.Judges.FirstOrDefaultAsync(j => j.Id == judge.Id);
                oldJudge.UserId = judge.UserId;
                oldJudge.Experience = judge.Experience;
                oldJudge.Certifications = judge.Certifications;
                oldJudge.AssignedContests = judge.AssignedContests;
                oldJudge.Status = judge.Status;
                oldJudge.UpdatedDate = DateTime.Now;
                oldJudge.UpdatedBy = 1;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

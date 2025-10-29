using Microsoft.EntityFrameworkCore;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DAL
{
    public class AttemptRepository : GenericRepository<Attempt>, IAttemptRepository
    {
        private readonly QuizeraDB _context;
        public AttemptRepository(QuizeraDB context) : base(context)
        {
            _context = context;
        }

        public async Task<Attempt> GetAllAttemptDetails(int id)
        {
            return await  _context.Attempts
                .Include(a => a.Student)
                .Include(a => a.Exam)
                    .ThenInclude(e => e.Questions)
                        .ThenInclude(q => q.Options)
                .Include(a => a.Answers)
                    .ThenInclude(ans => ans.Question)
                .FirstOrDefaultAsync(att => att.Id == id);
        }
    }
}

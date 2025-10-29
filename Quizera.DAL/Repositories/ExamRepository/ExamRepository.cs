using Microsoft.EntityFrameworkCore;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DAL
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        private readonly QuizeraDB _context;
        public ExamRepository(QuizeraDB context) : base(context)
        {
            _context = context;
        }

        public async Task<Exam> GetDetailsByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Course)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(exam => exam.Id == id);
        }
    }
}

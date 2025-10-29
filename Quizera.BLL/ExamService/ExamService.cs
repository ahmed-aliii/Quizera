using Quizera.DAL;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.BLL
{
    public class ExamService : GenericService<Exam>, IExamService
    {
        private readonly IExamRepository _examRepository;
        public ExamService(IExamRepository repository) : base(repository)
        {
            _examRepository = repository;
        }

        public async Task<Exam?> GetDetailsByIdAsync(int id)
        {
            var exam = await _examRepository.GetDetailsByIdAsync(id);

            return exam;
        }
    }
}

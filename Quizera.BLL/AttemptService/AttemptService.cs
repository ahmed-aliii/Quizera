using Quizera.DAL;
using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.BLL
{
    public class AttemptService : GenericService<Attempt>, IAttemptService
    {
        private readonly IAttemptRepository _repository;
        public AttemptService(IAttemptRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public Task<Attempt> GetAllAttemptDetails(int id)
        {
            return _repository.GetAllAttemptDetails(id);
        }
    }
}

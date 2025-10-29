using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.DAL
{
    public interface IAttemptRepository : IGenericRepository<Attempt>
    {
        Task<Attempt> GetAllAttemptDetails(int id);
    }
}

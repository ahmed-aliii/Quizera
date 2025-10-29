using Quizera.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizera.BLL
{
    public interface IExamService : IGenericService<Exam>
    {
        Task<Exam> GetDetailsByIdAsync(int id);
    }
}

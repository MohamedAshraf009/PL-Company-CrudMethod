using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenricRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(int? id);

        Task<int> AddEntity(T Tentity);

        Task<int> UpdateEntity(T Tentity);

        Task<int> DeleteEntity(T Tentity);
    }
}

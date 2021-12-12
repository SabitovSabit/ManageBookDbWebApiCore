using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageBooksAPI.Interfaces
{
    public interface ICommonRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int Id);
        Task<T> Create(T _object);
        Task<T> Update(T _object);
        Task Delete(int Id);
    }
}

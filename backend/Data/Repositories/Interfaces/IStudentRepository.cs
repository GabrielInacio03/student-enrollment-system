using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Data.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<PagedResult<Student>> GetPagedAsync(int page, int pageSize, string search);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByRAAsync(string ra);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Student student);
        Task<bool> SaveChangesAsync();


    }
}
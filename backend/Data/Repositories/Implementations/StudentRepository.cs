using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Repositories.Interfaces;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync() =>
            await _context.Students.ToListAsync();
        public async Task<PagedResult<Student>> GetPagedAsync(int page, int pageSize, string search)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.Name.Contains(search) ||
                    s.RA.Contains(search) ||
                    s.CPF.Contains(search));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Student>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<Student?> GetByIdAsync(int id) =>
            await _context.Students.FindAsync(id);

        public async Task<Student?> GetByRAAsync(string ra) =>
            await _context.Students.FirstOrDefaultAsync(s => s.RA == ra);

        public async Task AddAsync(Student student) =>
            await _context.Students.AddAsync(student);

        public async Task UpdateAsync(Student student) =>
            _context.Students.Update(student);

        public async Task DeleteAsync(Student student) =>
            _context.Students.Remove(student);

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;


    }
}
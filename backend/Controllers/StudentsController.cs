using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Repositories.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repo;

        public StudentsController(IStudentRepository repo) {
            _repo = repo;
        }

[HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll() {
            var students = await _repo.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id) {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Student student) {
            await _repo.AddAsync(student);
            var success = await _repo.SaveChangesAsync();
            if (!success) return BadRequest("Erro ao salvar.");
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Student updatedStudent) {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return NotFound();

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.RA = updatedStudent.RA;
            student.CPF = updatedStudent.CPF;

            await _repo.UpdateAsync(student);
            var success = await _repo.SaveChangesAsync();
            if (!success) return BadRequest("Erro ao atualizar.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return NotFound();

            await _repo.DeleteAsync(student);
            var success = await _repo.SaveChangesAsync();
            if (!success) return BadRequest("Erro ao deletar.");
            return NoContent();
        }


    }
}
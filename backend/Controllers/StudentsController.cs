using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.Repositories.Interfaces;
using backend.DTOs.Aluno;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repo;

        public StudentsController(IStudentRepository repo)
        {
            _repo = repo;
        }
        
        /// <summary>
        /// Retorna todos os alunos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _repo.GetAllAsync();
            return Ok(students);
        }
        /// <summary>
        /// GetPaged lista de alunos com paginação
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 10, [FromQuery] int pageSize = 2, [FromQuery] string search = "")
        {
            var result = await _repo.GetPagedAsync(page, pageSize, search);
            return Ok(result);
        }
        /// <summary>
        /// Retorna um aluno específico pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        /// <summary>
        /// Cria um novo estudante
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(StudentCreateDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Erros = erros });
            }
            
            if (_repo.GetByRAAsync(studentDTO.RA).Result != null)
            {
                List<string> errors = new List<string>();
                errors.Add("RA já cadastrado. Use outro número.");
                return BadRequest(new { Erros = errors });
            }
            var student = new Student
            {
                Name = studentDTO.Name,
                Email = studentDTO.Email,
                RA = studentDTO.RA,
                CPF = studentDTO.CPF
            };
            await _repo.AddAsync(student);
            await _repo.SaveChangesAsync();
            var response = new StudentResponseDTO
            {
                Name = student.Name,
                Email = student.Email,
                RA = student.RA,
                CPF = student.CPF
            };

        return Ok(new { Mensagem = "Aluno cadastrado com sucesso!", Aluno = response });


        }

        /// <summary>
        /// Atualiza os dados de um estudante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedStudent"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] StudentUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Erros = erros });
            }

            var student = await _repo.GetByIdAsync(id);
            if (student == null)
                return NotFound(new { Erros = new[] { "Aluno não encontrado." } });

            student.Name = dto.Name;
            student.Email = dto.Email;

            await _repo.UpdateAsync(student);
            var success = await _repo.SaveChangesAsync();

            if (!success)
                return BadRequest(new { Erros = new[] { "Erro ao atualizar o cadastro." } });

            return Ok(new { Mensagem = "Cadastro atualizado com sucesso!" });
        }

        /// <summary>
        /// Exclui um estudando pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var student = await _repo.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound(new { Erros = new[] { "Aluno não encontrado." } });
            }

            await _repo.DeleteAsync(student);
            var success = await _repo.SaveChangesAsync();

            if (!success)
            {
                return BadRequest(new { Erros = new[] { "Erro ao excluir o cadastro do aluno." } });
            }

            return Ok(new { Mensagem = "Aluno excluído com sucesso!" });
        }

    }
}
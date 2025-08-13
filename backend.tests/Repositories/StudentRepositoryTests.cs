using Xunit;
using Moq;
using backend.Controllers;
using backend.Data.Repositories.Interfaces;
using backend.Models;
using backend.DTOs.Aluno;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using System.Text.Json;
using backend.tests.Models;

namespace Backend.Tests.Controllers
{
    public class StudentsControllerTests
    {
        private StudentsController CreateControllerWithMockRepo(out Mock<IStudentRepository> mockRepo)
        {
            mockRepo = new Mock<IStudentRepository>();
            return new StudentsController(mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfStudents()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Student>
            {
                new Student { Id = 1, Name = "João" },
                new Student { Id = 2, Name = "Maria" }
            });

            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var students = Assert.IsType<List<Student>>(okResult.Value);
            Assert.Equal(2, students.Count);
        }

        [Fact]
        public async Task GetPaged_ReturnsPagedResult()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
          
            var pagedResult = new PagedResult<Student>
            {
                Items = new List<Student> { new Student { Id = 1, Name = "João", Email = "joao@gmail.com", RA = "090924732947", CPF = "33333345678" } },
                TotalItems = 1,
                Page = 1,
                PageSize = 10
            };

            mockRepo.Setup(r => r.GetPagedAsync(1, 10, "")).ReturnsAsync(pagedResult);

            var result = await controller.GetPaged(1, 10, "");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pagedResult, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsStudent_WhenExists()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Student { Id = 1, Name = "João" });

            var result = await controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var student = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("João", student.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student)null);

            var result = await controller.GetById(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenRAExists()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            var dto = new StudentCreateDTO { Name = "João", Email = "joao@gmail.com", RA = "001", CPF = "11111111111" };

            mockRepo.Setup(r => r.GetByRAAsync(dto.RA)).ReturnsAsync(new Student());

            var result = await controller.Create(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var json = JsonSerializer.Serialize(badRequest.Value);
            var value = JsonSerializer.Deserialize<ErrorWrapper>(json);

            Assert.Contains("RA já cadastrado. Use outro número.", value.Erros);


        }

        [Fact]
        public async Task Create_ReturnsOk_WhenValid()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            var dto = new StudentCreateDTO { Name = "João", Email = "joao@gmail.com", RA = "001", CPF = "11111111111" };

            mockRepo.Setup(r => r.GetByRAAsync(dto.RA)).ReturnsAsync((Student)null);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Aluno cadastrado com sucesso", okResult.Value.ToString());
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenStudentMissing()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student)null);

            var dto = new StudentUpdateDTO { Name = "Novo", Email = "novo@email.com" };
            var result = await controller.Update(99, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var json = JsonSerializer.Serialize(notFound.Value);
            var value = JsonSerializer.Deserialize<ErrorWrapper>(json);

            Assert.Contains("Aluno não encontrado.", value.Erros);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccess()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            var student = new Student { Id = 1, Name = "João", Email = "joao@email.com" };

            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var dto = new StudentUpdateDTO { Name = "Atualizado", Email = "atualizado@email.com" };
            var result = await controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Cadastro atualizado com sucesso", okResult.Value.ToString());
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Student)null);

            var result = await controller.Delete(99);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var json = JsonSerializer.Serialize(notFound.Value);
            var value = JsonSerializer.Deserialize<ErrorWrapper>(json);

            Assert.Contains("Aluno não encontrado.", value.Erros);
        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenSuccess()
        {
            var controller = CreateControllerWithMockRepo(out var mockRepo);
            var student = new Student { Id = 1, Name = "João" };

            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);
            mockRepo.Setup(r => r.DeleteAsync(student)).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Aluno excluído com sucesso", okResult.Value.ToString());
        }
    }
}
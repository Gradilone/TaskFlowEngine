using Moq;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using TaskManager.Models.Enum;

namespace TaskManager.Tests
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _repositoryMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _repositoryMock = new Mock<ITarefaRepository>();
            _envMock = new Mock<IWebHostEnvironment>();
            _service = new TarefaService(_repositoryMock.Object, _envMock.Object);
        }

        [Fact]
        public async Task ConcluirTarefa_DeveRetornarFalse_QuandoTarefaNaoExistir()
        {
            _repositoryMock.Setup(r => r.ObterPorId(It.IsAny<string>()))
                           .ReturnsAsync((Tarefa)null);

            var resultado = await _service.ConcluirTarefa("id_inexistente");

            Assert.False(resultado);
        }

        [Fact]
        public async Task ConcluirTarefa_DeveRetornarTrue_QuandoSucesso()
        {
            string idExistente = "65b2f1234567890abcdef123";
            var tarefaFake = new Tarefa
            {
                Id = idExistente,
                Titulo = "Tarefa Teste",
                Status = StatusTarefa.Pendente
            };

            _repositoryMock.Setup(r => r.ObterPorId(idExistente))
                           .ReturnsAsync(tarefaFake);

            var resultado = await _service.ConcluirTarefa(idExistente);

            Assert.True(resultado);
            Assert.Equal(StatusTarefa.Concluida, tarefaFake.Status);

            _repositoryMock.Verify(r => r.SalvarAlteracoes(), Times.Once);
        }
    }
}
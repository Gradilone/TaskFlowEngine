using TaskManager.DTO;
using TaskManager.Models.Enum;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _repository;
        private readonly IMensagemService _mensagemService;

        public TarefaService(ITarefaRepository repository, IMensagemService mensagemService)
        {
            _repository = repository;
            _mensagemService = mensagemService;
        }

        public async Task<TarefaResponseDto> Criar(TarefaCreateDto dto)
        {
            var tarefa = new Tarefa
            {
                TipoTarefa = dto.TipoTarefa,
                Dados = dto.Dados,
                DataCriacao = DateTime.Now,
                Status = StatusTarefa.Pendente,
                Tentativas = 0
            };

            var criada = await _repository.Adicionar(tarefa);

            _mensagemService.PublicarTarefa(criada.Id.ToString());

            return MapToResponse(criada);
        }

        public async Task<IEnumerable<TarefaResponseDto>> Listar(StatusTarefa? status)
        {
            var tarefas = await _repository.ListarTodos(status);
            return tarefas.Select(MapToResponse);
        }

        public async Task<TarefaResponseDto?> ObterPorId(string id)
        {
            var tarefa = await _repository.ObterPorId(id);

            if (tarefa == null) return null;

            return MapToResponse(tarefa);
        }

        private TarefaResponseDto MapToResponse(Tarefa t) => new TarefaResponseDto
        {
            Id = t.Id,
            TipoTarefa = t.TipoTarefa,
            Dados = t.Dados,
            DataCriacao = t.DataCriacao,
            Status = t.Status,
            Tentativas = t.Tentativas
        };
    }
}

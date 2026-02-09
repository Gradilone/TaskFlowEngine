using TaskManager.DTO;
using TaskManager.Models.Enum;

namespace TaskManager.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<TarefaResponseDto> Criar(TarefaCreateDto dto);
        Task<IEnumerable<TarefaResponseDto>> Listar(StatusTarefa? status);
        Task<TarefaResponseDto?> ObterPorId(string id);
    }
}

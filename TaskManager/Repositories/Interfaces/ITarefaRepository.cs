using TaskManager.Models.Enum;
using TaskManager.Models;

namespace TaskManager.Repositories.Interfaces
{
    public interface ITarefaRepository
    {
        Task<Tarefa> Adicionar(Tarefa tarefa);
        Task<IEnumerable<Tarefa>> ListarTodos(StatusTarefa? status);
        Task<Tarefa?> ObterPorId(string id);
        Task<bool> Atualizar(string id, Tarefa tarefaAtualizada);
    }
}

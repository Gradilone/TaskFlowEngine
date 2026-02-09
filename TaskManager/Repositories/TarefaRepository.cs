using TaskManager.Models.Enum;
using TaskManager.Models;
using TaskManager.Data;
using TaskManager.Repositories.Interfaces;
using MongoDB.Driver;

namespace TaskManager.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly IMongoCollection<Tarefa> _tarefas;

        public TarefaRepository(MongoDbContext context)
        {
            _tarefas = context.Tarefas;
        }

        public async Task<Tarefa> Adicionar(Tarefa tarefa)
        {
            await _tarefas.InsertOneAsync(tarefa);
            return tarefa;
        }

        public async Task<IEnumerable<Tarefa>> ListarTodos(StatusTarefa? status)
        {
            var builder = Builders<Tarefa>.Filter;
            var filter = builder.Empty;

            if (status.HasValue)
            {
                filter = builder.Eq(t => t.Status, status.Value);
            }

            return await _tarefas.Find(filter).ToListAsync();
        }

        public async Task<Tarefa> ObterPorId(string id)
        {
            var filtro = Builders<Tarefa>.Filter.Eq("_id", MongoDB.Bson.ObjectId.Parse(id));
            return await _tarefas.Find(filtro).FirstOrDefaultAsync();
        }

        public async Task<bool> Atualizar(string id, Tarefa tarefaAtualizada)
        {
            var filtro = Builders<Tarefa>.Filter.Eq(t => t.Id, id);

            var resultado = await _tarefas.ReplaceOneAsync(filtro, tarefaAtualizada);

            return resultado.MatchedCount > 0;
        }
    }
}

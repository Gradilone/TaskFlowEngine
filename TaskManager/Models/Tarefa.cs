using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TaskManager.Models.Enum;

namespace TaskManager.Models
{
    public class Tarefa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; }
        public string? TipoTarefa { get; set; }
        public string? Dados { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        [BsonRepresentation(BsonType.String)]
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
        public int Tentativas { get; set; } = 0;
    }
}

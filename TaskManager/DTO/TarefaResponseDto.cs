using TaskManager.Models.Enum;

namespace TaskManager.DTO
{
    public class TarefaResponseDto
    {
        public string? Id { get; set; }
        public string? TipoTarefa { get; set; }
        public string? Dados { get; set; }
        public DateTime DataCriacao { get; set; }
        public StatusTarefa Status { get; set; }
        public int Tentativas { get; set; }
    }
}

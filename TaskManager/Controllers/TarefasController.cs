using Microsoft.AspNetCore.Mvc;
using TaskManager.DTO;
using TaskManager.Models.Enum;
using TaskManager.Services.Interfaces;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _service;

        public TarefasController(ITarefaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] TarefaCreateDto dto)
        {
            var resultado = await _service.Criar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] StatusTarefa? status)
        {
            var tarefas = await _service.Listar(status);
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(string id)
        {
            var tarefa = await _service.ObterPorId(id);

            if (tarefa == null)
                return NotFound(new { message = "Tarefa não encontrada" });

            return Ok(tarefa);
        }
    }
}

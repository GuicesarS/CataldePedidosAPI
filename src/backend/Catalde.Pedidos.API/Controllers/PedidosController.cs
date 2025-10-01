using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Services;
using Catalde.Pedidos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalde.Pedidos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IPedidoRepository _repository;
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoRepository repository, IPedidoService pedidoService)
    {
        _repository = repository;  
        _pedidoService = pedidoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PedidoDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PedidoDTO>>> GetAll()
    {
        var pedidos = await _repository.GetAllAsync();

        var pedidosDTO = pedidos.Select(_pedidoService.MapearParaDTO).ToList();

        return Ok (pedidosDTO);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<PedidoDTO>>> GetById(int id)
    {
        var pedido = await _repository.GetByIdAsync(id);

        if (pedido is null)
            return NotFound($"Pedido {id} não encontrado.");

        return Ok(_pedidoService.MapearParaDTO(pedido));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDTO>> CriarPedido([FromBody] CriarPedidoDTO dto)
    {
        var pedido =  _pedidoService.CriarPedido(dto);

        await _repository.CreateAsync(pedido);

        return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido}, _pedidoService.MapearParaDTO(pedido));

    }

    [HttpPost("{id}/ocorrencia")]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AdicionarOcorencia(int id, [FromBody] AdicionarOcorrenciaDTO dto)
    {
        var pedido = await _repository.GetByIdAsync(id);

        if (pedido is null)
            return NotFound($"Pedido {id} não encontrado.");

        _pedidoService.AdicionarOcorrencia(pedido, dto);
        await _repository.UpdateAsync(pedido);

        return NoContent();
    }

    [HttpDelete("{id}/ocorrencia/{ocorrenciaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult> ExcluirOcorrencia(int pedidoId, int ocorrenciaId)
    {
        var pedido = await _repository.GetByIdAsync(pedidoId);

        if (pedido is null)
            return NotFound($"Pedido {pedidoId} não encontrado.");

        var ocorrencia = pedido.Ocorrencias.Find(o => o.IdOcorrencia == ocorrenciaId);

        if (ocorrencia is null)
            return NotFound($"Ocorrência {ocorrenciaId} não encontrada para o pedido {pedidoId}.");

        try
        {
            pedido.ExcluirOcorrencia(ocorrencia);
            await _repository.UpdateAsync(pedido);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }
}

using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalde.Pedidos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<PedidoDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PedidoDTO>>> GetAll()
    {
        var pedidos = await _pedidoService.GetAllPedidosAsync();
        return Ok (pedidos);
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDTO>> GetById(int id)
    {
        var pedido = await _pedidoService.GetPedidoByIdAsync(id);
        return Ok(pedido);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDTO>> CriarPedido([FromBody] CriarPedidoDTO dto)
    {
        var pedido = await _pedidoService.CriarPedidoAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido}, _pedidoService.MapearParaDTO(pedido));

    }

    [Authorize]
    [HttpPost("{id}/ocorrencia")]
    [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AdicionarOcorrencia(int id, [FromBody] AdicionarOcorrenciaDTO dto)
    {      
            await _pedidoService.AdicionarOcorrenciaAsync(id, dto);
            return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}/ocorrencia/{ocorrenciaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult> ExcluirOcorrencia(int pedidoId, int ocorrenciaId)
    {
        await _pedidoService.ExcluirOcorrenciaAsync(pedidoId, ocorrenciaId);
        return NoContent();
    }
}

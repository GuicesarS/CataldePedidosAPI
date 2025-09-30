using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Services;
using Catalde.Pedidos.Domain.Enums;
using FluentAssertions;

namespace Catalde.Pedidos.UnitTests.Application.Services;

public class PedidoServiceTests
{
    private readonly PedidoService _service = new();

    [Fact]
    public void CriarPedidoValido()
    {
        var dto = new CriarPedidoDTO { NumeroPedido = 230 };

        var pedido = _service.CriarPedido(dto);

        pedido.Should().NotBeNull();
        pedido.NumeroPedido.Value.Should().Be(230);
        pedido.IndEntregue.Should().BeFalse();
    }

    [Fact]
    public void AdicionarOcorrenciaValida()
    {
        var pedido = _service.CriarPedido(new CriarPedidoDTO { NumeroPedido = 100 });
        var dto = new AdicionarOcorrenciaDTO { PedidoId = pedido.IdPedido, TipoOcorrencia = (int)ETipoOcorrencia.EmRotaDeEntrega };

        _service.AdicionarOcorrencia(pedido, dto);

        pedido.Ocorrencias.Should().HaveCount(1);
        pedido.Ocorrencias.First().TipoOcorrencia.Should().Be(ETipoOcorrencia.EmRotaDeEntrega);
    }

    [Fact]
    public void AdicionarOcorrencia_PedidoNull()
    {
        var dto = new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = 0 };

        Action act = () => _service.AdicionarOcorrencia(null!, dto);

        act.Should().Throw<ArgumentNullException>().WithParameterName("pedido");
    }

    [Fact]
    public void AdicionarOcorrencia_OcorrenciaNull()
    {
        var pedido = _service.CriarPedido(new CriarPedidoDTO { NumeroPedido = 30 });

        Action act = () => _service.AdicionarOcorrencia(pedido, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("ocorrenciaDto");
    }

    [Fact]
    public void MapearDadosDTO_Pedido()
    {
        var pedido = _service.CriarPedido(new CriarPedidoDTO { NumeroPedido = 44 });

        _service.AdicionarOcorrencia(pedido, new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = (int)ETipoOcorrencia.EmRotaDeEntrega});

        var map = _service.MapearParaDTO(pedido);

        map.Should().NotBeNull();
        map.NumeroPedido.Should().Be(44);
        map.Ocorrencias.Should().HaveCount(1);
        map.Ocorrencias.First().TipoOcorrencia.Should().Be(ETipoOcorrencia.EmRotaDeEntrega);

    }

    [Fact]
    public void MapearDadosDTO_PedidoNull()
    {
        Action act = () => _service.MapearParaDTO(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("pedido");
    }
}

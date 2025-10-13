using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Services;
using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Catalde.Pedidos.Domain.Exceptions;
using Catalde.Pedidos.Domain.ValueObjects;
using Catalde.Pedidos.Infrastructure.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace Catalde.Pedidos.UnitTests.Application.Services;

public class PedidoServiceTests
{
    private readonly PedidoService _service;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPedidoRepository> _repositoryMock;
    public PedidoServiceTests()
    {
        _repositoryMock = new Mock<IPedidoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _unitOfWorkMock.Setup(u => u.Pedidos).Returns(_repositoryMock.Object);

        _service = new PedidoService(_unitOfWorkMock.Object);
    }


    [Fact]
    public async Task CriarPedidoValido()
    {
        var dto = new CriarPedidoDTO { NumeroPedido = 230 };

        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var pedido = await _service.CriarPedidoAsync(dto);

        pedido.Should().NotBeNull();
        pedido.NumeroPedido.Value.Should().Be(230);
        pedido.IndEntregue.Should().BeFalse();

        _repositoryMock.Verify(r => r.Create(It.IsAny<Pedido>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);

    }

    [Fact]
    public async Task AdicionarOcorrenciaValida()
    {
        var pedido = await _service.CriarPedidoAsync(new CriarPedidoDTO { NumeroPedido = 100 });

        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(pedido);
        _unitOfWorkMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        var dto = new AdicionarOcorrenciaDTO { PedidoId = pedido.IdPedido, TipoOcorrencia = (int)ETipoOcorrencia.EmRotaDeEntrega };

        await _service.AdicionarOcorrenciaAsync(1, dto);

        pedido.Ocorrencias.Should().HaveCount(1);
        pedido.Ocorrencias.First().TipoOcorrencia.Should().Be(ETipoOcorrencia.EmRotaDeEntrega);
    }

    [Fact]
    public async Task AdicionarOcorrencia_PedidoNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Pedido?)null);

        var dto = new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = 0 };

        Func<Task> act = async () => await _service.AdicionarOcorrenciaAsync(1, dto);

        await act.Should().ThrowAsync<PedidoNotFoundException>();
    }

    [Fact]
    public async Task AdicionarOcorrencia_OcorrenciaNull()
    {
        var pedido = await _service.CriarPedidoAsync(new CriarPedidoDTO { NumeroPedido = 30 });

        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(pedido);

        Func<Task> act = async () => await _service.AdicionarOcorrenciaAsync(1, null!);

        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("dto");
    }

    [Fact]
    public void MapearDadosDTO_Pedido()
    {
        var pedido = new Pedido(new NumeroPedido(44));
        var dto = new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = (int)ETipoOcorrencia.EmRotaDeEntrega };

        pedido.AdicionarOcorrencia(new Ocorrencia((ETipoOcorrencia)dto.TipoOcorrencia, false));

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

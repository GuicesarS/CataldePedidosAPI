using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Validators;
using FluentAssertions;

namespace Catalde.Pedidos.UnitTests.Application.Validators;

public class CriarPedidoDTOValidatorTests
{
    private readonly CriarPeditoDTOValidator _validator = new();

    [Fact]
    public void Criar_NumeroPedido_Valido()
    {
        var dto = new CriarPedidoDTO { NumeroPedido = 10 };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]

    public void Rejeitar_NumeroPedido_Invalido(int valorInvalido)
    {
        var dto = new CriarPedidoDTO { NumeroPedido =  valorInvalido };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Número do pedido deve ser maior que zero.");
    }
}

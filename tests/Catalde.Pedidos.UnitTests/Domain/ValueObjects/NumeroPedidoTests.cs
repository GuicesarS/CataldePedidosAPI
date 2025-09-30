using Catalde.Pedidos.Domain.ValueObjects;
using FluentAssertions;

namespace Catalde.Pedidos.UnitTests.Domain.ValueObjects;

public class NumeroPedidoTests
{
    [Fact]
    public void Criar_NumeroPedido_Valido()
    {
        var numeroPedido = new NumeroPedido(123);

        numeroPedido.Value.Should().Be(123);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-4)]
    public void Nao_Aceitar_Valores_Invalidos(int valorInvalido)
    {
        Action act = () => new NumeroPedido(valorInvalido);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Número do pedido deve ser maior que zero."); ;
    }

    [Fact]
    public void Pedidos_Com_Mesmo_Valor_Devem_Ser_Iguais()
    {
        var pedido1 = new NumeroPedido(10);
        var pedido2 = new NumeroPedido(10);

        pedido1.Equals(pedido2).Should().BeTrue();
        pedido1.GetHashCode().Should().Be(pedido2.GetHashCode());
    }

}

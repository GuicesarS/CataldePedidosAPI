using Catalde.Pedidos.Domain.Entities;
using Catalde.Pedidos.Domain.Enums;
using Catalde.Pedidos.Domain.ValueObjects;
using FluentAssertions;

namespace Catalde.Pedidos.UnitTests.Domain.Entities;

public class PedidoTests
{
    [Fact]
    public void Nao_Permitir_OcorrenciaDuplicada_Em10Minutos()
    {
        var pedido = new Pedido(new NumeroPedido(1));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        pedido.AdicionarOcorrencia(ocorrencia1);

        var ocorrenciaDuplicada = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);

        ocorrenciaDuplicada.GetType().GetProperty("HoraOcorrencia")!.SetValue(ocorrenciaDuplicada, ocorrencia1.HoraOcorrencia.AddMinutes(5));

        Action act = () => pedido.AdicionarOcorrencia (ocorrenciaDuplicada);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Não é possível registrar a mesma ocorrência em menos de 10 minutos.");

    }

    [Fact]
    public void Segunda_Ocorrencia_Deve_Finalizar()
    {
        var pedido = new Pedido(new NumeroPedido(3));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        pedido.AdicionarOcorrencia(ocorrencia1);

        var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.AvariaNoProduto, false);
        pedido.AdicionarOcorrencia(ocorrencia2);

        ocorrencia2.IndFinalizadora.Should().BeTrue();
        
    }

    [Fact]
    public void Ocorrencia_Finalizada_EntregueComSucesso()
    {
        var pedido = new Pedido(new NumeroPedido(7));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        pedido.AdicionarOcorrencia(ocorrencia1);

        var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso, false);
        pedido.AdicionarOcorrencia(ocorrencia2);

        ocorrencia2.IndFinalizadora.Should().BeTrue();
        pedido.IndEntregue.Should().BeTrue();

    }

    [Fact]
    public void Ocorrencia_Finalizada_InSucesso()
    {
        var pedido = new Pedido(new NumeroPedido(2));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        pedido.AdicionarOcorrencia(ocorrencia1);

        var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.ClienteAusente, false);
        pedido.AdicionarOcorrencia(ocorrencia2);

        ocorrencia2.IndFinalizadora.Should().BeTrue();
        pedido.IndEntregue.Should().BeFalse();

    }


    [Fact]
    public void Adicionar_Ocorrencia_PedidoFinalizado()
    {
        var pedido = new Pedido(new NumeroPedido(10));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso, false);

        pedido.AdicionarOcorrencia(ocorrencia1);
        pedido.AdicionarOcorrencia(ocorrencia2);

        var ocorrencia3 = new Ocorrencia(ETipoOcorrencia.AvariaNoProduto, false);

        Action act = () => pedido.AdicionarOcorrencia(ocorrencia3);

        act.Should().Throw<InvalidOperationException>()
        .WithMessage("Não é possível adicionar ocorrências a um pedido finalizado.");

    }

    [Fact]
    public void Excluir_Ocorrencia_PedidoFinalizado()
    {
        var pedido = new Pedido(new NumeroPedido(10));

        var ocorrencia1 = new Ocorrencia(ETipoOcorrencia.EmRotaDeEntrega, false);
        var ocorrencia2 = new Ocorrencia(ETipoOcorrencia.EntregueComSucesso, false);

        pedido.AdicionarOcorrencia(ocorrencia1);
        pedido.AdicionarOcorrencia(ocorrencia2);

        Action act = () => pedido.ExcluirOcorrencia(ocorrencia1);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Não é possível excluir ocorrência de pedido concluído.");
    }

    [Fact]
    public void Excluir_Ocorrencia_Nula_DeveFalhar()
    {
        var pedido = new Pedido(new NumeroPedido(20));

        Action act = () => pedido.ExcluirOcorrencia(null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("ocorrencia");
    }

    [Fact]
    public void Excluir_Ocorrencia_NaoEncontrada_DeveFalhar()
    {
        var pedido = new Pedido(new NumeroPedido(21));
        var ocorrencia = new Ocorrencia(ETipoOcorrencia.AvariaNoProduto, false);

        Action act = () => pedido.ExcluirOcorrencia(ocorrencia);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Ocorrência não encontrada no pedido.");
    }


}

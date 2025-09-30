using Catalde.Pedidos.Application.DTOs;
using Catalde.Pedidos.Application.Validators;
using FluentAssertions;

namespace Catalde.Pedidos.UnitTests.Application.Validators;

public class AdicionarOcorrenciaDtoValidatorTests
{
    private readonly AdicionarOcorrenciaDTOValidator _validator = new();

    [Fact]
    public void Aceitar_Dados_Validos()
    {
        var dto = new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = 0 };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]

    public void Rejeitar_PedidoId_Invalido(int pedidoIdInvalido)
    {
        var dto = new AdicionarOcorrenciaDTO { PedidoId= pedidoIdInvalido, TipoOcorrencia = 0 };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PedidoId");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]

    public void Rejeitar_TipoOcorrencia_Invalido(int ocorrenciaInvalida)
    {
        var dto = new AdicionarOcorrenciaDTO { PedidoId = 1, TipoOcorrencia = ocorrenciaInvalida };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TipoOcorrencia");
    }
}

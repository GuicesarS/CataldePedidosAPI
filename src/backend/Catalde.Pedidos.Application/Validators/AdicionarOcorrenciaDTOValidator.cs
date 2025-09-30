using Catalde.Pedidos.Application.DTOs;
using FluentValidation;

namespace Catalde.Pedidos.Application.Validators;

public class AdicionarOcorrenciaDTOValidator : AbstractValidator<AdicionarOcorrenciaDTO>
{
    public AdicionarOcorrenciaDTOValidator()
    {
        RuleFor(x => x.PedidoId)
            .GreaterThan(0)
            .WithMessage("PedidoId deve ser maior que zero.");

        RuleFor(x => x.TipoOcorrencia)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tipo de ocorrência inválido.");
    }
}

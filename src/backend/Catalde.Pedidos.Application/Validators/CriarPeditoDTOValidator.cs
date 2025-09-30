using Catalde.Pedidos.Application.DTOs;
using FluentValidation;

namespace Catalde.Pedidos.Application.Validators;

public class CriarPeditoDTOValidator : AbstractValidator<CriarPedidoDTO>
{
    public CriarPeditoDTOValidator()
    {
        RuleFor(x => x.NumeroPedido)
            .GreaterThan(0)
            .WithMessage("Número do pedido deve ser maior que zero.");
    }
}

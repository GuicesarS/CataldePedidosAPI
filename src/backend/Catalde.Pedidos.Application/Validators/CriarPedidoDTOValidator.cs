using Catalde.Pedidos.Application.DTOs;
using FluentValidation;

namespace Catalde.Pedidos.Application.Validators;

public class CriarPedidoDTOValidator : AbstractValidator<CriarPedidoDTO>
{
    public CriarPedidoDTOValidator()
    {
        RuleFor(x => x.NumeroPedido)
            .GreaterThan(0)
            .WithMessage("Número do pedido deve ser maior que zero.");
    }
}

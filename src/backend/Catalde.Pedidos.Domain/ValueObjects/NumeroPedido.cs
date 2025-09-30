namespace Catalde.Pedidos.Domain.ValueObjects;

public class NumeroPedido
{
    public int Value { get; private set; }
    protected NumeroPedido() { }
    public NumeroPedido(int value)
    {
        if(value <= 0)
            throw new ArgumentException("Número do pedido deve ser maior que zero.");

        Value = value;
    }

    public override string ToString() => Value.ToString();

    public override bool Equals(object? obj)
    {
        if (obj is not NumeroPedido other) return false;
        return Value == other.Value;
    }
    public override int GetHashCode() => Value.GetHashCode();
    
}


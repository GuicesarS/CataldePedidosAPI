using Catalde.Pedidos.Domain.Enums;
using Catalde.Pedidos.Domain.ValueObjects;

namespace Catalde.Pedidos.Domain.Entities;

public class Pedido
{
    public int IdPedido { get; private set; }
    public NumeroPedido NumeroPedido { get; private set; }
    public DateTime HorarioPedido { get; private set; }
    public bool IndEntregue { get; private set; }
    public List<Ocorrencia> Ocorrencias { get; private set; }

    protected Pedido() { }
    public Pedido(NumeroPedido numeroPedido)
    {
       NumeroPedido = numeroPedido ?? throw new ArgumentNullException(nameof(numeroPedido)); 
       HorarioPedido = DateTime.UtcNow;
       IndEntregue = false;
       Ocorrencias = new List<Ocorrencia>();
               
    }

    public void AdicionarOcorrencia(Ocorrencia ocorrencia)
    {
        if(Ocorrencias.Any(o => o.TipoOcorrencia == ocorrencia.TipoOcorrencia &&
           (ocorrencia.HoraOcorrencia - o.HoraOcorrencia).TotalMinutes <10))
        {
            throw new InvalidOperationException("Não é possível registrar a mesma ocorrência em menos de 10 minutos.");
        }

        if(IndEntregue)
            throw new InvalidOperationException("Não é possível adicionar ocorrências a um pedido finalizado.");

        Ocorrencias.Add(ocorrencia);

        if(Ocorrencias.Count == 2)
        {
            ocorrencia.MarcarFinalizadora();
            IndEntregue = ocorrencia.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso;
        }
    }

    public void ExcluirOcorrencia (Ocorrencia ocorrencia)
    {
        if (ocorrencia is null)
            throw new ArgumentNullException(nameof(ocorrencia));

        if (IndEntregue)
            throw new InvalidOperationException("Não é possível excluir ocorrência de pedido concluído.");

        if (!Ocorrencias.Contains(ocorrencia))
            throw new InvalidOperationException("Ocorrência não encontrada no pedido.");


        Ocorrencias.Remove(ocorrencia);
    }
}




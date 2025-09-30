using Catalde.Pedidos.Domain.Enums;

namespace Catalde.Pedidos.Domain.Entities;

public class Pedido
{
    public int IdPedido { get; private set; }
    public int NumeroPedido { get; private set; }
    public DateTime HorarioPedido { get; private set; }
    public bool IndEntregue { get; private set; }
    public List<Ocorrencia> Ocorrencias { get; private set; }

    public Pedido(int numeroPedido)
    {
       NumeroPedido = numeroPedido;
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

        Ocorrencias.Add(ocorrencia);

        if(Ocorrencias.Count == 2)
        {
            ocorrencia.MarcarFinalizadora();
            IndEntregue = ocorrencia.TipoOcorrencia == ETipoOcorrencia.EntregueComSucesso;
        }
    }

    public void ExcluirOcorrencia (Ocorrencia ocorrencia)
    {
        if (IndEntregue)
            throw new InvalidOperationException("Não é possível excluir ocorrência de pedido concluído.");

        Ocorrencias.Remove(ocorrencia);
    }
}




using Catalde.Pedidos.Domain.Enums;

namespace Catalde.Pedidos.Domain.Entities;

public class Ocorrencia
{
    public int IdOcorrencia { get; private set; }
    public ETipoOcorrencia TipoOcorrencia { get; private set; }
    public DateTime HoraOcorrencia { get; private set; }
    public bool IndFinalizadora { get; private set; }

    protected Ocorrencia() { }
    public Ocorrencia(ETipoOcorrencia tipoOcorrencia, bool indFinalizadora)
    {
        TipoOcorrencia = tipoOcorrencia;
        HoraOcorrencia = DateTime.UtcNow;
        IndFinalizadora = indFinalizadora;
    }
    public void MarcarFinalizadora()
    {
        IndFinalizadora = true;
    }
}


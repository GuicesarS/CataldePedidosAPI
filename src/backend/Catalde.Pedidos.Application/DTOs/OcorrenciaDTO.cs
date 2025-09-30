using Catalde.Pedidos.Domain.Enums;

namespace Catalde.Pedidos.Application.DTOs;

public class OcorrenciaDTO
{
    public int IdOcorrencia { get; set; }
    public ETipoOcorrencia TipoOcorrencia { get; set; }
    public DateTime HoraOcorrencia { get; set; }
    public bool IndFinalizadora { get; set; }
}

namespace ControleLicenca.Api.Models;

public class SistemaControle
{
    public Guid ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public bool Bloqueado { get; set; }
}

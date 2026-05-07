using ControleLicenca.Api.Data;
using ControleLicenca.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleLicenca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LicencasController : ControllerBase
{
    private readonly SeuDbContext _context;

    public LicencasController(SeuDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todos os clientes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SistemaControle>>> GetClientes()
    {
        return await _context.SistemaControle.ToListAsync();
    }

    /// <summary>
    /// Obtém um cliente pelo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SistemaControle>> GetCliente(Guid id)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        return cliente;
    }

    /// <summary>
    /// Verifica se o cliente está bloqueado
    /// </summary>
    [HttpGet("{id}/status")]
    public async Task<ActionResult<object>> VerificarStatus(Guid id)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        return Ok(new
        {
            clienteId = cliente.ClienteId,
            nomeCliente = cliente.NomeCliente,
            bloqueado = cliente.Bloqueado,
            status = cliente.Bloqueado ? "Bloqueado" : "Liberado"
        });
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SistemaControle>> CreateCliente(CreateClienteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NomeCliente))
            return BadRequest(new { mensagem = "Nome do cliente é obrigatório" });

        var cliente = new SistemaControle
        {
            ClienteId = Guid.NewGuid(),
            NomeCliente = request.NomeCliente,
            Bloqueado = request.Bloqueado ?? false
        };

        _context.SistemaControle.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClienteId }, cliente);
    }

    /// <summary>
    /// Atualiza um cliente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(Guid id, UpdateClienteRequest request)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        if (!string.IsNullOrWhiteSpace(request.NomeCliente))
            cliente.NomeCliente = request.NomeCliente;

        if (request.Bloqueado.HasValue)
            cliente.Bloqueado = request.Bloqueado.Value;

        _context.SistemaControle.Update(cliente);
        await _context.SaveChangesAsync();

        return Ok(cliente);
    }

    /// <summary>
    /// Bloqueia um cliente
    /// </summary>
    [HttpPost("{id}/bloquear")]
    public async Task<IActionResult> BloquearCliente(Guid id)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        if (cliente.Bloqueado)
            return BadRequest(new { mensagem = "Cliente já está bloqueado" });

        cliente.Bloqueado = true;
        _context.SistemaControle.Update(cliente);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Cliente bloqueado com sucesso", cliente });
    }

    /// <summary>
    /// Desbloqueia um cliente
    /// </summary>
    [HttpPost("{id}/desbloquear")]
    public async Task<IActionResult> DesbloqueiaCliente(Guid id)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        if (!cliente.Bloqueado)
            return BadRequest(new { mensagem = "Cliente já está desbloqueado" });

        cliente.Bloqueado = false;
        _context.SistemaControle.Update(cliente);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Cliente desbloqueado com sucesso", cliente });
    }

    /// <summary>
    /// Deleta um cliente
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(Guid id)
    {
        var cliente = await _context.SistemaControle.FindAsync(id);

        if (cliente == null)
            return NotFound(new { mensagem = "Cliente não encontrado" });

        _context.SistemaControle.Remove(cliente);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Cliente deletado com sucesso" });
    }
}

public class CreateClienteRequest
{
    public string NomeCliente { get; set; } = string.Empty;
    public bool? Bloqueado { get; set; }
}

public class UpdateClienteRequest
{
    public string? NomeCliente { get; set; }
    public bool? Bloqueado { get; set; }
}

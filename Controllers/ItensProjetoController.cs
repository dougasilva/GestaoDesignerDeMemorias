using GestaoDesignerDeMemorias.Data;
using GestaoDesignerDeMemorias.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDesignerDeMemorias.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensProjetoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItensProjetoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ItemProjeto>> PostItem(ItemProjeto item)
        {
            _context.ItensProjeto.Add(item);
            await _context.SaveChangesAsync();

            // Atualiza o valor total do projeto automaticamente
            var projeto = await _context.Projetos.FindAsync(item.ProjetoId);
            if (projeto != null)
            {
                projeto.ValorTotal += item.Valor;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemProjeto>> GetItemById(int id)
        {
            var item = await _context.ItensProjeto.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    /*endpoint = URL
    https://localhost:5001
    http://localhost:5000
    */
    [Route("v1/Category")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        //User-Agent, localização qualquer e duração 30 minutos
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> GetById(int id,
            [FromServices] DataContext context)
        {
            var category = context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null)
                return NotFound($"Categoria {id} não encontrada.");

            return Ok(category);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();//aguardar as mudanças.

                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível criar a categoria {e.Message}." });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Category>> Put(int id,
            [FromBody] Category model,
            [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { Mensagem = $"Categoria {id} não encontrada." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                //passou para modificado
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(new { Mensagem = $"Não foi possível alterar a categoria deu concorrência {e.Message}." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensagem = $"Não foi possível alterar a categoria {e.Message}." });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Category>> Delete(int id,
            [FromServices] DataContext context)
        {
            var model = context.Categories.Find(id);

            if (model == null)
                return NotFound(new { Mensagem = $"Categoria {id} não encontrada." });

            try
            {
                context.Categories.Remove(model);
                await context.SaveChangesAsync();//aguardar as mudanças.

                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível remover a categoria {e.Message}." });
            }
        }
    }
}
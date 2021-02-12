using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Shop.Controllers
{
    /*endpoint = URL
    https://localhost:5001
    http://localhost:5000
    */
    [Route("v1/Product")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context
                .Products
                .Include(p => p.Category)
                .AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetById(int id,
            [FromServices] DataContext context)
        {
            var product = await context
                .Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound($"Produto {id} não encontrado.");

            return Ok(product);
        }

        [HttpGet]
        [Route("Categories/{id}")]
        public async Task<ActionResult<Product>> GetByCategoryId(int id,
            [FromServices] DataContext context)
        {
            var product = await context
                .Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            if (product == null)
                return NotFound($"Produto {id} não encontrado.");

            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post([FromBody] Product model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();//aguardar as mudanças.

                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível criar o produto {e.Message}." });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Product>> Put(int id,
            [FromBody] Product model,
            [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new { Mensagem = $"Produto {id} não encontrado." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                //passou para modificado
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(new { Mensagem = $"Não foi possível alterar o produto deu concorrência {e.Message}." });
            }
            catch (Exception e)
            {
                return BadRequest(new { Mensagem = $"Não foi possível alterar a categoria {e.Message}." });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Product>> Delete(int id,
            [FromServices] DataContext context)
        {
            var model = context.Products.Find(id);

            if (model == null)
                return NotFound(new { Mensagem = $"Produto {id} não encontrado." });

            try
            {
                context.Products.Remove(model);
                await context.SaveChangesAsync();//aguardar as mudanças.

                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível remover o produto. {e.Message}." });
            }
        }
    }
}
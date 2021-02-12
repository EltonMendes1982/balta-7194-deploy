using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shop.Services;

namespace Shop.Controllers
{
    /*endpoint = URL
    https://localhost:5001
    http://localhost:5000
    */
    [Route("v1/User")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        //[Authorize(Roles = "manager")] //
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous] //
        public async Task<ActionResult<User>> Post([FromBody] User model,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                model.Role = "employe";
                context.Users.Add(model);
                await context.SaveChangesAsync();//aguardar as mudanças.
                model.PassWord = string.Empty;
                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível criar a categoria {e.Message}." });
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous] //
        public async Task<ActionResult<dynamic>> Login(
            [FromBody] User model,
            [FromServices] DataContext context)
        {
            var user = await context
                .Users
                .AsNoTracking()
                .Where(x => x.UserName == model.UserName &&
                            x.PassWord == model.PassWord)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { Mensagem = "Usuário não localizado." });

            var token = TokenService.GenerateToken(user);

            user.PassWord = string.Empty;
            return new
            {
                user,
                token
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<User>> Delete(int id,
            [FromServices] DataContext context)
        {
            var model = context.Users.Find(id);

            if (model == null)
                return NotFound(new { Mensagem = $"Categoria {id} não encontrada." });

            try
            {
                context.Users.Remove(model);
                await context.SaveChangesAsync();//aguardar as mudanças.

                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(new { Mensagem = $"Não foi possível remover o usuário {e.Message}." });
            }
        }
        /*
        [Route("anonimo")]
        [HttpGet]
        [AllowAnonymous]
        public string Anonimo() => "Anonimo";

        [Route("autenticado")]
        [HttpGet]
        [Authorize]
        public string Autenticado() => "Autenticado";

        [Route("funcionario")]
        [HttpGet]
        [Authorize(Roles ="employee")]
        public string Funcionario() => "Funcionario";

        [Route("gerente")]
        [HttpGet]
        [Authorize(Roles = "manager")]
        public string Gerente() => "Gerente";
        */
    }
}
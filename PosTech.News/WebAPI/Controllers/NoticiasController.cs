using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.ApplicationCore.Entities;
using News.ApplicationCore.Repositories;

namespace News.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiasRepository repository;
        public NoticiasController(INoticiasRepository noticiasRepository)
        {
            repository = noticiasRepository;
        }
        
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Noticia>>> GetNoticias()
        {
            var Noticias = await repository.GetAll();

            if (Noticias == null)
            {
                return BadRequest("N�o existem Not�cias");
            }

            return Ok(Noticias.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Noticia>> GetNoticia(int id)
        {
            var Noticia = await repository.GetById(id);

            if (Noticia == null)
            {
                return NotFound("Not�cia n�o encontrada pelo id informado");
            }

            return Ok(Noticia);
        }

        [HttpPost]
        public async Task<IActionResult> PostNoticia([FromBody] Noticia Noticia)
        {
            if (Noticia == null)
            {
                return BadRequest("Dados inv�lidos");
            }

            await repository.Insert(Noticia);

            return CreatedAtAction(nameof(GetNoticia), new { Id = Noticia.Id }, Noticia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticia(int id, Noticia Noticia)
        {
            if (id != Noticia.Id)
            {
                return BadRequest($"O c�digo da Not�cia {id} n�o confere");
            }

            try
            {
                await repository.Update(id, Noticia);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok("Atualiza��o da Not�cia realizada com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Noticia>> DeleteNoticia(int id)
        {
            var Noticia = await repository.GetById(id);
            if (Noticia == null)
            {
                return NotFound($"Not�cia com Id {id} n�o foi encontrada");
            }

            await repository.Delete(id);

            return Ok(Noticia);
        }

    }
}
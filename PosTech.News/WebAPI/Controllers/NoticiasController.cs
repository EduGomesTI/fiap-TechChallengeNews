using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Application.News.Commands;
using News.Application.News.Queries;
using News.Domain.Repositories;

namespace News.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NoticiasController : ControllerBase
    {
        private readonly INewsRepository repository;
        private readonly ISender _sender;

        public NoticiasController(INewsRepository noticiasRepository, ISender sender)
        {
            repository = noticiasRepository;
            _sender = sender;
        }

        [HttpGet()]
        public async Task<IActionResult> GetNoticias(CancellationToken cancellationToken)
        {
            var Noticias = await _sender.Send(new GetAllNewsQuery(), cancellationToken);

            if (Noticias == null)
            {
                return BadRequest("Não existem Notícias");
            }

            return Ok(Noticias.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoticia(int id, CancellationToken cancellationToken)
        {
            var Noticia = await _sender.Send(new GetNewsByIdQuery(id), cancellationToken);

            if (Noticia == null)
            {
                return NotFound("Notícia não encontrada pelo id informado");
            }

            return Ok(Noticia);
        }

        [HttpPost]
        public async Task<IActionResult> PostNoticia([FromBody] AddNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos");
            }

            var result = await _sender.Send(request, cancellationToken);

            return CreatedAtAction(nameof(PostNoticia), result.Messages);
        }

        [HttpPut()]
        public async Task<IActionResult> PutNoticia([FromBody] UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos");
            }

            var result = await _sender.Send(request, cancellationToken);

            return Ok(result.Messages);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoticia(int id)
        {
            if (id == 0)
            {
                return BadRequest("Id inválido.");
            }

            var result = await _sender.Send(new DeleteNewsCommand(id));

            return Ok(result);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Domain.Entities;
using News.Security.JWT;
using System.Net;

namespace News.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult<Token> Post(
            [FromBody] User usuario,
            [FromServices] ILogger<LoginController> logger,
            [FromServices] AccessManager accessManager)
        {
            logger.LogInformation($"Recebida solicitação para o usuário: {usuario?.Username}");

            if (usuario is not null && accessManager.ValidateCredentials(usuario))
            {
                logger.LogInformation($"Sucesso na autenticação do usuário: {usuario.Username}");
                return accessManager.GenerateToken(usuario);
            }
            else
            {
                logger.LogError($"Falha na autenticação do usuário: {usuario?.Username}");
                return new UnauthorizedResult();
            }
        }
    }
}

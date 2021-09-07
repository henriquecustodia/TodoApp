using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Todo
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository usersRepository;
        private readonly AuthTokenService authTokenService;

        public UsersController(
            UsersRepository usersRepository,
            AuthTokenService authTokenService
        )
        {
            this.usersRepository = usersRepository;
            this.authTokenService = authTokenService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenModel>> CreateUser(UserPayloadModel payload)
        {
            var validatorResult = await new UserPayloadValidator().ValidateAsync(payload);

            if (!validatorResult.IsValid)
            {
                return BadRequest(ValidationResponse.CreateFieldsErrors(validatorResult.Errors));
            }

            var exists = await usersRepository.UserExists(payload.Email);

            if (!exists)
            {
                return BadRequest(ValidationResponse.CreateGenericError("Usuário já existe!"));
            }

            var user = await usersRepository.CreateUser(payload);

            return authTokenService.Create(user.Email);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenModel>> Login(UserPayloadModel payload)
        {
            var validatorResult = await new UserPayloadValidator().ValidateAsync(payload);

            if (!validatorResult.IsValid)
            {
                return BadRequest(ValidationResponse.CreateFieldsErrors(validatorResult.Errors));
            }

            var canAuthenticateUser = await usersRepository.canAuthenticateUser(payload);

            if (!canAuthenticateUser)
            {
                return Unauthorized();
            }

            return authTokenService.Create(payload.Email);
        }
    }
}

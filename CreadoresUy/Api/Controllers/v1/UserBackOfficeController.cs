using Application.Features.UserFreaturesBO.Commands;
using Application.Features.UserFreaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserBackOfficeController : BaseApiController
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateUserCommand>> CreateUser(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpGet("{pageNumber},{pageSize}")]
        [Authorize]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize}));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id }));
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserCommandBO command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
    }
}

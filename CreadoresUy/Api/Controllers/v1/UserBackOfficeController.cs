using Application.Features.UserFeaturesBO.Commands;
using Application.Features.UserFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserBackOfficeController : BaseApiController
    {

   
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateUserCommandBO>> CreateUser(CreateUserCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateUserCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

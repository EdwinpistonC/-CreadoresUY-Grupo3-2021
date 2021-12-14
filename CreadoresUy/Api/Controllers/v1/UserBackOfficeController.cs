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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdBOQuery { Id = id }));
        }
        
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersBOQuery()));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommandBO { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateUserCommandBO>> CreateUser(CreateUserCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }


        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser( UpdateUserCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

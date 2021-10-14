using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using Share.Dtos;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{

    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        /*
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        */

        [HttpPost]
        public async Task<ActionResult<UserSignInCommand>> CreateUser(UserSignInCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand { Id = id }));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetUserById { Id = id }));
        }
        [HttpGet("{search},{pageSize},{page}")]
        public async Task<IActionResult> GetSearch(string search,int pageSize,int page)
        {
            return Ok(await Mediator.Send(new GetUserBySearchQuery { SearchText = search, SizePage = pageSize, Page=page }));
        }
        
        [HttpGet("{email},{password}")]
        public async Task<IActionResult> GetGetPruebaSearch(string email,string password)
        {
            return Ok(await Mediator.Send(
                new GetLogingUserQuery { User = new LoginDto() { Email = email, Password = password } }));

        }
    }

}
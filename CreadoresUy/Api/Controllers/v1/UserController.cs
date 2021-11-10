using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.Dtos;
using System.Threading.Tasks;
using WebApi.Models;

namespace Api.Controllers.v1
{

    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        //tokens functions


        [AllowAnonymous]
        [HttpPost(nameof(Authenticate))]
        public async Task<IActionResult> Authenticate(GetLogingUserQuery command)
        {
            return Ok(await Mediator.Send(command));

        }
        [AllowAnonymous]
        [HttpPost(nameof(Token))]
        public async Task<ActionResult<GetTokenQuery>> Token(GetTokenQuery command)
        {
            return Ok(await Mediator.Send(command));

        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserSignUpCommand>> CreateUser(UserSignUpCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }
        [HttpGet("{id}")]
        [Authorize]
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
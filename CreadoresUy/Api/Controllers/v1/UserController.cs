using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.Dtos;
using System.Threading.Tasks;

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserSignUpCommand>> CreateUser(UserSignUpCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }
        
        [HttpGet("GetUserForChat/{nickname}")]
        [Authorize]
        public async Task<IActionResult> GetUserForChat(string nickname)
        {
            return Ok(await Mediator.Send(new GetUserForChatQuery { Nickname = nickname }));

        }
        [HttpGet("GetUsersChats/{users}")]
        [Authorize]
        public async Task<IActionResult> GetUsersChats(string users)
        {
            return Ok(await Mediator.Send(new GetUsersChatsQuery { Users = users }));

        }

        [HttpPost]
        [Route("Follow")]
        [Authorize]
        public async Task<ActionResult<FollowCommand>> FollowCreator(FollowCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Route("Unfollow")]
        [Authorize]
        public async Task<ActionResult<UnfollowCommand>> UnfollowCreator(UnfollowCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // A que esta suscripto
        [HttpGet]
        [Route("SubscribedTo")]
        [Authorize]
        public async Task<IActionResult> SubscribedTo(int idUser)
        {
            return Ok(await Mediator.Send(new SubscribedToQuery { IdUser = idUser }));
        }

        // A quien esta siguiendo
        [HttpGet]
        [Route("FollowingTo")]
        [Authorize]
        public async Task<IActionResult> FollowingTo(int idUser)
        {
            return Ok(await Mediator.Send(new FollowingToQuery { IdUser = idUser }));
        }

        //Suscribirse a
        [HttpPost]
        [Route("SubscribeTo")]
        [Authorize]
        public async Task<ActionResult<SubscribeToCommand>> SubscribeTo(SubscribeToCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Route("Unsubscribed")]
        [Authorize]
        public async Task<ActionResult<UnsubscribeCommand>> Unsubscribed(UnsubscribeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetUserProfile")]
        [Authorize]
        public async Task<ActionResult<GetUserProfileQuery>> GetUserProfile(int id)
        {
            return Ok(await Mediator.Send(new GetUserProfileQuery { IdUser = id }));
        }

    }

}
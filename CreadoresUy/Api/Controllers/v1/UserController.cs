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

        [AllowAnonymous]
        [HttpPost]
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

        [HttpPost]
        [Authorize]
        [Route("Follow")]
        public async Task<ActionResult<FollowCommand>> FollowCreator(FollowCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Authorize]
        [Route("Unfollow")]
        public async Task<ActionResult<UnfollowCommand>> UnfollowCreator(UnfollowCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // A que esta suscripto
        [HttpGet]
        [Authorize]
        [Route("SubscribedTo")]
        public async Task<IActionResult> SubscribedTo(int idUser)
        {
            return Ok(await Mediator.Send(new SubscribedToQuery { IdUser = idUser }));
        }

        // A quien esta siguiendo
        [HttpGet]
        [Authorize]
        [Route("FollowingTo")]
        //[Authorize]
        public async Task<IActionResult> FollowingTo(int idUser)
        {
            return Ok(await Mediator.Send(new FollowingToQuery { IdUser = idUser }));
        }

        //Suscribirse a
        [HttpPost]
        [Authorize]
        [Route("SubscribeTo")]
        public async Task<ActionResult<SubscribeToCommand>> SubscribeTo(SubscribeToCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Authorize]
        [Route("Unsubscribed")]
        public async Task<ActionResult<UnsubscribeCommand>> Unsubscribed(UnsubscribeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserProfile")]
        public async Task<ActionResult<GetUserProfileQuery>> GetUserProfile(int id)
        {
            return Ok(await Mediator.Send(new GetUserProfileQuery { IdUser = id }));
        }

    }

}
using Application.Features.CreatorFeatures.Commands;
using Application.Features.CreatorFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]

    public class CreatorController : BaseApiController
    {
        
        [HttpPost]
        [Route("SignUp")]
        [Authorize(Roles = "user,admin,creator")]
        public async Task<ActionResult<CreatorSignUpCommand>> CreateCreator(CreatorSignUpCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetCategoryes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryes()
        {
            return Ok(await Mediator.Send(new GetCategoryes { }));
        }

        [HttpPost]
        [Route("CreatePlanAndBenefits")]
        [Authorize]
        public async Task<ActionResult<SetPlanAndBenefitsCommand>> CreatePlanAndBenefits(SetPlanAndBenefitsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Route("UpdatePlansAndBenefits")]
        [Authorize]
        public async Task<IActionResult> Update(UpdatePlanAndBenefitsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetCreatorPlanBenefitsById")]
        [Authorize]
        public async Task<IActionResult> GetBenefitCantByPlanQuery(int idp, string nickname)
        {
            return Ok(await Mediator.Send(new GetBenefitCantByPlanQuery { IdPlan = idp, Nickname = nickname }));
        }

        [HttpGet]
        [Route("GetDefaultBenefits")]
        [Authorize]
        public async Task<IActionResult> GetDefaultBenefits()
        {
            return Ok(await Mediator.Send(new GetDefaultBenefitsQuery { }));
        }

        [HttpGet]
        [Route("GetCreatorPlansById")]
        [Authorize]
        public async Task<IActionResult> GetCategGetCreatorPlansoryesById(int id)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansByIdQuery { CreatorId = id }));
        }

        [HttpGet]
        [Route("GetCreatorPlansSubsc")]
        [Authorize]
        public async Task<IActionResult> GetCreatorPlansSubsc(int idUser, string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansByNicknameQuery { IdUser = idUser, Nickname = nickname }));
        }

        [HttpGet]
        [Route("GetSubscribers")]
        [Authorize]
        public async Task<IActionResult> GetSubscribers(int idCreator)
        {
            return Ok(await Mediator.Send(new GetSubscribersQuery { IdCreator = idCreator }));
        }

        [HttpGet]
        [Route("GetFollowers")]
        [Authorize]
        public async Task<IActionResult> GetFollowers(int idCreator)
        {
            return Ok(await Mediator.Send(new GetFollowersQuery { IdCreator = idCreator }));
        }

        [HttpGet]
        [Route("GetCreatorsByCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCreatorsByCategory(string category, int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetCreatorByCategoryQuery { SearchCategory = category, Page = pageNumber, SizePage = pageSize }));
        }

        [HttpGet]
        [Route("GetProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCreatorProfile(string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorProfile { Nickname = nickname }));
        }

        [HttpGet]
        [Route("GetContentByUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContentByUser(string nickname,int idUser, int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetCreatorContentByUser { 
                                                Nickname = nickname, 
                                                IdUser = idUser, 
                                                PageNumber = pageNumber,
                                                PageSize = pageSize
                                            }));
        }
        [HttpGet]
        [Route("GetCreatorBySearch")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCreatorBySearch(string searchText, int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetCreatorBySearchQuery { SearchText = searchText, SizePage = pageSize, Page = pageNumber }));
        }

        [HttpGet]
        [Route("GetCreatorPlansBasic")]
        [Authorize]
        public async Task<IActionResult> GetCreatorPlansQuery(string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansQuery { Nickname = nickname}));
        }

        [HttpGet]
        [Route("GetBoolSubsc")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBoolSubsc(int id, string nickname)
        {
            return Ok(await Mediator.Send(new GetBoolSubscQuery { IdUser = id,Nickname = nickname }));
        }

        [HttpGet]
        [Route("GetInfoPlan")]
        [Authorize]
        public async Task<IActionResult> GetInfoPlan(int idPlan)
        {
            return Ok(await Mediator.Send(new GetInfoPlanQuery { IdPlan = idPlan}));
        }

    }

}

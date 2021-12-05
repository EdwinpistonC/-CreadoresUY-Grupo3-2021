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
        public async Task<ActionResult<CreatorSignUpCommand>> CreateCreator(CreatorSignUpCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetCategoryes")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> GetCategoryes()
        {
            return Ok(await Mediator.Send(new GetCategoryes { }));
        }

        [HttpPost]
        [Route("SetPlansAndBenefits")]
        public async Task<ActionResult<SetPlanAndBenefitsCommand>> SetPlansAndBenefits(SetPlanAndBenefitsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Route("UpdatePlansAndBenefits")]
        public async Task<IActionResult> Update(UpdatePlanAndBenefitsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetCreatorPlansById")]
        //[Authorize]
        public async Task<IActionResult> GetCategGetCreatorPlansoryesById(int id)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansByIdQuery { CreatorId = id }));
        }

        [HttpGet]
        [Route("GetCreatorPlansByNickname")]
        //[Authorize]
        public async Task<IActionResult> GetCategGetCreatorPlansoryesByNickname(string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansByNicknameQuery { Nickname = nickname }));
        }

        [HttpGet]
        [Route("GetSubscribers")]
        //[Authorize]
        public async Task<IActionResult> GetSubscribers(int idCreator)
        {
            return Ok(await Mediator.Send(new GetSubscribersQuery { IdCreator = idCreator }));
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCreatorsByCategory")]
        //[Authorize]
        public async Task<IActionResult> GetCreatorsByCategory(string category, int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetCreatorByCategoryQuery { SearchCategory = category, Page = pageNumber, SizePage = pageSize }));
        }


        [HttpPost]
        [Route("Basic")]
        public async Task<ActionResult<CreateCreatorCommand>> CreateCreator(CreateCreatorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPatch]
        [Route("Update")]
        public async Task<ActionResult<UpdateCreatorCommand>> Update(UpdateCreatorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult<DeleteCreatorCommand>> Delete(DeleteCreatorCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCreatorQuery { }));
        }

        [HttpGet]
        [Route("GetBool")]
        public async Task<IActionResult> IsValidNickname(string nickname)
        {
            return Ok(await Mediator.Send(new IsValidNicknameQuery { Nickname = nickname}));
        }

        [HttpGet]
        [Route("GetProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCreatorProfile(string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorProfile { Nickname = nickname }));
        }

        [HttpGet]
        [AllowAnonymous]

        [Route("GetContentByUser")]
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
        [AllowAnonymous]
        [Route("GetCreatorBySearch")]
        public async Task<IActionResult> GetCreatorBySearch(string searchText, int pageNumber, int pageSize)
        {
            return Ok(await Mediator.Send(new GetCreatorBySearchQuery { SearchText = searchText, SizePage = pageSize, Page = pageNumber }));
        }

        [HttpGet]
        [Route("GetCreatorPlansBasic")]
        public async Task<IActionResult> GetCreatorPlansQuery(string nickname)
        {
            return Ok(await Mediator.Send(new GetCreatorPlansQuery { Nickname = nickname}));
        }

        //GetCreatorProfile

        /* TRAE los planes asociados a un creador
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlansByIdUser(int id)
        {
            return Ok(await Mediator.Send(new GetPlansByIdCreatorQuery { IdCreator = id }));
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

        */
    }


}

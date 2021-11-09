using Application.Features.CreatorFeatures.Commands;
using Application.Features.CreatorFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]

    public class CreatorController : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<CreatorSignUpCommand>> CreateCreator(CreatorSignUpCommand command)
        {
            return Ok(await Mediator.Send(command));
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

    }


}

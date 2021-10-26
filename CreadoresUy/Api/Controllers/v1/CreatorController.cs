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

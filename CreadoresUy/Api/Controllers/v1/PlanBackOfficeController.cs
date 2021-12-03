using Application.Features.DefaultPlanFeaturesBO.Commands;
using Application.Features.DefaultPlanFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PlanBackOfficeController : BaseApiController
    {

 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetDefaultPlanByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllDefaultPlanBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteDefaultPlanByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateDefaultPlanCommandBO>> CreateUser(CreateDefaultPlanCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateDefaultPlanCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

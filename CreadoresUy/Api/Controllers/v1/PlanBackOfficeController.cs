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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetDefaultPlanByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllDefaultPlanBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteDefaultPlanByIdCommandBO { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateDefaultPlanCommandBO>> CreateUser(CreateDefaultPlanCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser( UpdateDefaultPlanCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

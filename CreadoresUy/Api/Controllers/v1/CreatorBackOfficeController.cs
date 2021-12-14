using Application.Features.AdminFeaturesBO.Commands;
using Application.Features.CreatorFeaturesBO.Commands;
using Application.Features.CreatorFeaturesBO.Queries;
using Application.Features.UserFeaturesBO.Commands;
using Application.Features.UserFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CreatorBackOfficeController : BaseApiController
    {
   
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCreatorByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCreatorBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCreatorByIdCommandBO { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateCreatorCommandBO>> CreateUser(CreateCreatorCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser( UpdateCreatorCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

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
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCreatorByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCreatorBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCreatorByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateCreatorCommandBO>> CreateUser(CreateCreatorCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateCreatorCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

using Application.Features.AdminFeaturesBO.Commands;
using Application.Features.AdminFeaturesBO.Queries;
using Application.Features.CreateFeaturesBO.Commands;
using Application.Features.CreatorFeaturesBO.Commands;
using Application.Features.UserFeaturesBO.Commands;
using Application.Features.UserFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AdminBackOfficeController : BaseApiController
    {

   
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetAdminByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllAdminBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteAdminByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateAdminCommandBO>> CreateUser(CreateAdminCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateAdminCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

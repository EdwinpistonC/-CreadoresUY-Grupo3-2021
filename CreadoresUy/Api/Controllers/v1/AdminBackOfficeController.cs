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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetAdminByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllAdminBOQuery()));
        }

        [HttpDelete("Disable/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisableAdmin(int id)
        {
            return Ok(await Mediator.Send(new DisableAdminByIdCommandBO { Id = id }));
        }

        [HttpDelete("Remove/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteAdminByIdCommandBO { Id = id }));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateAdminCommandBO>> CreateUser(CreateAdminCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAdmin( UpdateAdminCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

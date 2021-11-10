using Application.Features.AdminFeatures.Commands;
using Application.Features.AdminFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    public class AdminController :BaseApiController
    {
        private IConfiguration _config;
        public AdminController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateAdminCommand>> CreateAdmin(CreateAdminCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [AllowAnonymous]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(int id, UpdateAdminCommand command)
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
            return Ok(await Mediator.Send(new DeleteAdminCommand { Id = id }));
        }

        [HttpPost]
        [Route("GetAll")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllAdminQuery { }));
        }
    }
}


using Application.Features.BenefitFeaturesBO.Commands;
using Application.Features.BenefitFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BenefitBackOfficeController : BaseApiController
    {

   
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetBenefitByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllBenefitBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteBenefitByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateBenefitCommandBO>> CreateUser(CreateBenefitCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateBenefitCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

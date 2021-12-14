
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetBenefitByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllBenefitBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteBenefitByIdCommandBO { Id = id }));
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateBenefitCommandBO>> CreateUser(CreateBenefitCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser( UpdateBenefitCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

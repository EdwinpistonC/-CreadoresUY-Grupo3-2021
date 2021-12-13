using Application.Features.UserFeaturesBO.Commands;
using Application.Features.UserFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PaymentsBackOfficeController : BaseApiController
    {

   
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdBOQuery { Id = id }));
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> endAllPayment()
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommandBO()
                ));
        }

        [HttpPut("ByOne/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> endOnePayment(int id)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommandBO { Id = id }));
        }
       
    }
}

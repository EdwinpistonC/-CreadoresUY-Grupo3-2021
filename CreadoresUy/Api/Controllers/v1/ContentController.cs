using Application.Features.ContentFeature.Commands;
using Application.Features.ContentFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]

    public class ContentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateContentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await Mediator.Send(new GetAllContentQuery()));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Feed(int page, int usuario)
        {
            return Ok(await Mediator.Send(new GetAllContentQuery()));
        } 
    }
}

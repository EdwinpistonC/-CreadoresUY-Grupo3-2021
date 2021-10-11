using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Features.MessageFeatures.Commands;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class MessageController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("{idc},{idm}")]
        //[HttpDelete]
        public async Task<IActionResult> Delete(int idc, int idm)
        {
            return Ok(await Mediator.Send(new DeleteMessageCommand { IdChat=idc, IdMessage=idm}));
        }
    }
}

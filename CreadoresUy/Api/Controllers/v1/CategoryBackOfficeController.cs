
using Application.Features.CategoryFeaturesBO.Commands;
using Application.Features.CategoryFeaturesBO.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CategoryBackOfficeController : BaseApiController
    {

   
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCategoryByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCategoryBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryByIdCommandBO { Id = id }));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<CreateCategoryCommandBO>> CreateUser(CreateCategoryCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateUser( UpdateCategoryCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

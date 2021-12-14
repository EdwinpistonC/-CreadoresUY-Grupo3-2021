
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetCategoryByIdBOQuery { Id = id }));
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllCategoryBOQuery()));
        }
       
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryByIdCommandBO { Id = id }));
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CreateCategoryCommandBO>> CreateUser(CreateCategoryCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

        
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser( UpdateCategoryCommandBO command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}

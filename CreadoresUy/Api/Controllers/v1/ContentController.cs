using Application.Features.ContentFeature.Commands;
using Application.Features.ContentFeature.Queries;
using Application.Features.CreatorFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]

    public class ContentController : BaseApiController
    {
       
        [HttpPost]
        [Route("CreateNewDraftContent")]
        [Authorize(Roles = "creator,admin")]
        public async Task<IActionResult> CreateNewDraftContent(CreateDraftContentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        
        [HttpGet]
        [Route("GetContentDraft")]
        [Authorize(Roles = "creator,admin")]
        public async Task<IActionResult> GetContentDraftQuery(string nickname)
        {
            return Ok(await Mediator.Send(new GetContentDraftQuery { Nickname = nickname }));
        }

        
        [HttpPut]
        [Route("UpdateContent")]
        [Authorize(Roles = "creator,admin")]
        public async Task<IActionResult> Update(UpdateContentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        [Route("DeleteContent")]
        [Authorize(Roles = "creator,admin")]
        public async Task<IActionResult> Delete(int idCre, int idCont)
        {
            return Ok(await Mediator.Send(new DeleteContentCommand { IdCreator = idCre, IdContent = idCont }));
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> Feed(int IdUser,int Page,int ContentPerPage)
        {
            return Ok(await Mediator.Send(new GetFeedQuery { IdUser=IdUser,Page=Page, ContentPerPage = ContentPerPage }));
        }


        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> FeedById(int IdUser,int IdCreator, int Page, int ContentPerPage)
        {
            return Ok(await Mediator.Send(new GetFeedByIdQuery { IdUser = IdUser,IdCreator= IdCreator, Page = Page, ContentPerPage = ContentPerPage }));
        }
    }
}

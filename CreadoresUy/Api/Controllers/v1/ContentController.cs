﻿using Application.Features.ContentFeature.Commands;
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateContentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        [Authorize]
        [Route("CreateNewContent")]
        public async Task<IActionResult> CreateNewContent(CreateNewContentCommand command)
        {
            return Ok(await Mediator.Send(command));
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

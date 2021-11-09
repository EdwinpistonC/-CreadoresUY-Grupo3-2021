﻿using Application.Features.ContentFeature.Commands;
using Application.Features.CreatorFeatures.Queries;
using Application.Features.ContentFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> Feed(int IdUser,int Page,int ContentPerPage)
        {
            return Ok(await Mediator.Send(new GetFeedQuery { IdUser=IdUser,Page=Page, ContentPerPage = ContentPerPage }));
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await Mediator.Send(new GetAllContentQuery()));
        }

    }
}

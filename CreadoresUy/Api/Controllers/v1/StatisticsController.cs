using Application.Features.PaymentFeaturesBO.Queries;
using Application.Features.StatisticsFeaturesBO.Queries;
using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.Dtos;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{

    [ApiVersion("1.0")]
    public class StatisticsController : BaseApiController
    {
        [HttpGet("GetFinances")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetFinances()
        {
            return Ok(await Mediator.Send(new GetFinancesQuery()));

        }

        [HttpGet("GetNewUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetNewUsers()
        {
            return Ok(await Mediator.Send(new GetNewUsersQuery()));

        }

        [HttpGet("CreatorsSubs")]
        [Authorize(Roles = "admin,creator")]
        public async Task<IActionResult> CreatorsSubs()
        {
            return Ok(await Mediator.Send(new GetCreatorsSubsQuery()));

        }

        [HttpGet("CreatorsUnsub")]
        [Authorize(Roles = "admin,creator")]
        public async Task<IActionResult> CreatorsUnsubs()
        {
            return Ok(await Mediator.Send(new GetUnsubscribersQuery()));

        }

        [HttpGet("CreatorsFollowers")]
        [Authorize(Roles = "admin,creator")]
        public async Task<IActionResult> CreatorsFollowers()
        {
            return Ok(await Mediator.Send(new GetCreatorFollowersQuery()));

        }

        [HttpGet("CreatorsUnfollowers")]
        [Authorize(Roles = "admin,creator")]
        public async Task<IActionResult> CreatorsUnfollowers()
        {
            return Ok(await Mediator.Send(new GetCreatorUnfollowersQuery()));
        }

        [HttpGet("CreatorCategory")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreatorCategory()
        {
            return Ok(await Mediator.Send(new GetCreatorCategoryQuery()));


        }
        
        [HttpGet("GetLogs")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetLogs()
        {
            return Ok(await Mediator.Send(new GetLogsQuery()));


        }

        [HttpGet("GetFinancesCreator/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetLogs(int id)
        {
            return Ok(await Mediator.Send(new GetFinancesCreatorQuery() { idCreator=id}));

        }

    }

}
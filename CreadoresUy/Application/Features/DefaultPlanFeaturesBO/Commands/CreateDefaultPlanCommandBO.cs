using Application.Features.Validators;
using Application.Interface;
using Application.Service;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DefaultPlanFeaturesBO.Commands
{
    public class CreateDefaultPlanCommandBO : IRequest<Response<String>>
    {
        public DefaultPlanBODto CreatePlanDto { get; set; }
        public class CreateDefaultPlanCommandBOHandler : IRequestHandler<CreateDefaultPlanCommandBO, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;

            public CreateDefaultPlanCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost;
            }

            public async Task<Response<String>> Handle(CreateDefaultPlanCommandBO command, CancellationToken cancellationToken)
            {
                var dto = command.CreatePlanDto;

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };


                var Plan = new DefaultPlan();
                Plan.Name = dto.Name;
                Plan.Description = dto.Description;

                Plan.SubscriptionMsg = dto.SubscriptionMsg;
                Plan.WelcomeVideoLink = dto.WelcomeVideoLink;

                if (dto.Image != "") {
                    ImageDto dtoImgPrf = new(dto.Image, "defaultPlan"+DateTime.Now.ToString() + "photo", "Planes");
                    var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    Plan.Image = urlCreatorImg;
                }



                Plan.Benefits = new List<DefaultBenefit> ();
                foreach (var item in dto.Benefits)
                {
                    Plan.Benefits.Add(_mapper.Map<DefaultBenefit>(item)) ;
                }

                _context.DefaultPlans.Add(Plan);



                await _context.SaveChangesAsync();
                res.CodStatus = HttpStatusCode.Created;
                res.Success = true;
                var msg1 = "Plan ingresado correctamente";
                res.Message.Add(msg1);
                return res;
            }
        }
    }
}


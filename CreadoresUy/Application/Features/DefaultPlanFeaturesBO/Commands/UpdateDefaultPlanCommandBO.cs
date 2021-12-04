using Application.Interface;
using Application.Service;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DefaultPlanFeaturesBO.Commands
{
    public class UpdateDefaultPlanCommandBO : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }
        public ICollection<BenefitBODto> Benefits { get; set; }


        public class UpdateDefaultPlanCommandBOHandler : IRequestHandler<UpdateDefaultPlanCommandBO, Response<string>>
        {
            private readonly ICreadoresUyDbContext _context;
            private readonly IMapper _mapper;
            private readonly ImagePostService _imagePost;

            public UpdateDefaultPlanCommandBOHandler(ICreadoresUyDbContext context, IMapper mapper, ImagePostService imagePost)
            {
                _context = context;
                _mapper = mapper;
                _imagePost = imagePost;
            }
            public async Task<Response<string>> Handle(UpdateDefaultPlanCommandBO command, CancellationToken cancellationToken)
            {
                var defaultPlan = _context.DefaultPlans.Include(c=>c.Benefits).Where(c => c.Id == command.Id ).FirstOrDefault();
                Response<string> res = new()
                {
                    Message = new List<string>(),
                    Obj = ""
                };
                if (defaultPlan == null )
                {
                    res.Message.Add("No se ha encontrado el plan por defecto con el id: " + command.Id);
                    res.Success = false;
                    res.CodStatus = HttpStatusCode.BadRequest;
                    return res;
                }

                defaultPlan.Name = command.Name;
                defaultPlan.Description = command.Description;
                defaultPlan.Price = command.Price;
                if (defaultPlan.Image != command.Image)
                {
                    ImageDto dtoImgPrf = new(command.Image, "defaultPlan" + DateTime.Now.ToString() + "photo", "Planes");
                        var urlCreatorImg = await _imagePost.postImage(dtoImgPrf);
                    defaultPlan.Image = urlCreatorImg;
                }

                defaultPlan.SubscriptionMsg = command.SubscriptionMsg;
                defaultPlan.WelcomeVideoLink = command.WelcomeVideoLink;

                foreach (var b in defaultPlan.Benefits)
                {
                    _context.DefaultBenefits.Remove(b);
                }


                foreach (var b in command.Benefits)
                {

                    if (defaultPlan.Benefits == null) defaultPlan.Benefits = new List<DefaultBenefit>();

                    defaultPlan.Benefits.Add(_mapper.Map<DefaultBenefit>(b));
                }


                await _context.SaveChangesAsync();
                res.Message.Add("Exito");
                res.Success = true;
                res.CodStatus = HttpStatusCode.OK;
                return res;
            }

        }
        
    }
   
}

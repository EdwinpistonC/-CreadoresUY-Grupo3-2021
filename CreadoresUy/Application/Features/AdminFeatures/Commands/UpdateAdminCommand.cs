using Application.Features.UserFeatures.Validators;
using Application.Interface;
using FluentValidation.Results;
using MediatR;
using Share;
using Share.Dtos;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdminFeatures.Commands
{

    public class UpdateAdminCommand : IRequest<Response<String>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LasLogin { get; set; }
        public string? ImgProfile { get; set; }
        public int CreatorId { get; set; }


        public class UpdateAdminHandler : IRequestHandler<UpdateAdminCommand, Response<String>>
        {
            private readonly ICreadoresUyDbContext _context;
            public UpdateAdminHandler(ICreadoresUyDbContext context)
            {
                _context = context;
            }
            public async Task<Response<String>> Handle(UpdateAdminCommand command, CancellationToken cancellationToken)
            {
                var user = _context.Users.Where(a => (a.Id == command.Id && a.IsAdmin == true)).FirstOrDefault();

                Response<string> res = new Response<String>
                {
                    Obj = "",
                    Message = new List<String>()
                };



                if (user == null)
                {
                    res.CodStatus = HttpStatusCode.BadRequest;
                    res.Success = false;
                    res.Message.Add("Id de usuario no pertenece a un admin");
                    return res;
                }
                else

                {
                    var validator = new AdminCommandValidator(_context);

                    ValidationResult result = validator.Validate(user);

                    if (!result.IsValid)
                    {
                        res.CodStatus = HttpStatusCode.BadRequest;
                        res.Success = false;
                        foreach (var error in result.Errors)
                        {
                            var msg = error.ErrorMessage;
                            res.Message.Add(msg);
                        }
                        return res;
                    }
                    user.Name = command.Name;
                    user.Email = command.Email;
                    user.Password = command.Password;
                    user.Description = command.Description;
                    user.ImgProfile = command.ImgProfile;
                    if (command.CreatorId != 0)
                    {
                        user.CreatorId = command.CreatorId;
                    }
                    await _context.SaveChangesAsync();

                    res.Success = true;
                    res.CodStatus = HttpStatusCode.OK;
                    var msg1 = "Admin modificado";
                    res.Message.Add(msg1);
                    return res;
                }
            }
        }
    }
}

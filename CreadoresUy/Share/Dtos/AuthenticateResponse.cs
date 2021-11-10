using Share.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        private JwtSecurityToken token;

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LasLogin { get; set; }
        public string? ImgProfile { get; set; }
        public int? CreatorId { get; set; }
        public string Token { get; set; }
        public bool IsAdmin { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            UserId = user.Id;
            Name = user.Name;
            Email = user.Email;
            if (user.Description == null)
            {
                Description = "";
            }
            else
            {
                Description = user.Description;
            }
            if (user.Created == null)
            {
                Created = DateTime.Now;
            }
            else
            {
                Created = user.Created;
            }
            if (user.LasLogin == null)
            {
                LasLogin = DateTime.Now;
            }
            else
            {
                LasLogin = user.LasLogin;
            }
            if (user.ImgProfile == null)
            {
                ImgProfile = "";
            }
            else
            {
                ImgProfile = user.ImgProfile;
            }
            if (user.CreatorId == null)
            {
                CreatorId = 0;
            }
            else
            {
                CreatorId = user.CreatorId;
            }

            Token = token;
            IsAdmin = (bool)user.IsAdmin;
        }

        public AuthenticateResponse(User user, JwtSecurityToken token)
        {
            UserId = user.Id;
            Name = user.Name;
            Email = user.Email;
            if (user.Description == null)
            {
                Description = "";
            }
            else
            {
                Description = user.Description;
            }
            if (user.Created == null)
            {
                Created = DateTime.Now;
            }
            else
            {
                Created = user.Created;
            }
            if (user.LasLogin == null)
            {
                LasLogin = DateTime.Now;
            }
            else
            {
                LasLogin = user.LasLogin;
            }
            if (user.ImgProfile == null)
            {
                ImgProfile = "";
            }
            else
            {
                ImgProfile = user.ImgProfile;
            }
            if (user.CreatorId == null)
            {
                CreatorId = 0;
            }
            else
            {
                CreatorId = user.CreatorId;
            }


            IsAdmin = (bool)user.IsAdmin;

            this.token = token;
        }
    }
}
using Share.Entities;
using System;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
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
            Description = user.Description;
            Created = user.Created;
            LasLogin = user.LasLogin;
            ImgProfile = user.ImgProfile;
            CreatorId = user.CreatorId;
            Token = token;
            IsAdmin = (bool)user.IsAdmin;
        }
    }
}
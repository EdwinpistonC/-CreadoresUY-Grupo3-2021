﻿using AutoMapper;
using Share.Dtos;
using Share.Entities;

namespace Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Content, ContentDto>();
            CreateMap<ContentDto, Content>();

            CreateMap<Plan, PlanDto>();
            CreateMap<PlanDto, Plan>();


            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<User, LoginDto>();
            CreateMap<LoginDto, User>();


        }
    }
}
